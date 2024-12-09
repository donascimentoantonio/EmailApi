using AutoMapper;
using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Dtos.v1.Response;
using EmailManagement.Domain.Enum;
using EmailManagement.Domain.Models.Email;
using EmailManagement.Domain.Services;
using EmailManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EmailManagement.Application.Services
{
    public class EmailServiceOld
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IMapper _mapper;
        private readonly IMessageQueueService _messageQueueService;

        private  ILogger<EmailService> _logger { get; }

        public EmailServiceOld(
            IEmailRepository emailRepository,
            IMapper mapper,
            ILogger<EmailService> logger,
            IMessageQueueService messageQueueService)
        {
            _emailRepository = emailRepository;
            _mapper = mapper;
            _logger = logger;
            _messageQueueService = messageQueueService;
        }

        public async Task<IEnumerable<EmailPostParametersResponse>> SendEmailsAsync(List<EmailPostParametersRequest> requests)
        {
            var responses = new List<EmailPostParametersResponse>();

            foreach (var request in requests)
            {
                var email = Email.Create(request.Sender, request.Recipients, request.Subject, request.Body, request.Atachments?.ToList());

                // Salva o e-mail no banco
                await _emailRepository.SaveAsync(email);

                // Se a flag 'Send' estiver marcada, tenta enviar o e-mail
                if (request.Send)
                {
                    //await SendEmailAsync(email); // Lógica de envio do e-mail

                    email.MarkAsSent();
                }
                else
                {
                    email.MarkAsPending();
                }

                await _emailRepository.UpdateAsync(email);

                // Adiciona a resposta com o status do e-mail
                var response = new EmailPostParametersResponse
                {
                    Id = email.Id.ToString(),
                    Status = email.Status
                };
                responses.Add(response);
            }

            return responses;
        }
        

        public Task<bool> DeleteEmailAsync(Guid emailId)
        {
            throw new NotImplementedException();
        }

        public async Task<EmailGetParametersResponse?> GetEmailByIdAsync(Guid emailId)
        {
            // Busca o e-mail no repositório
            var email = await _emailRepository.GetByIdAsync(new EmailId(emailId));

            if (email == null)
            {
                return null; // E-mail não encontrado
            }

            // Mapeia a entidade para o DTO de resposta
            var emailResponse = _mapper.Map<EmailGetParametersResponse>(email);

            return emailResponse;
        }

        public async Task<IEnumerable<EmailGetParametersResponse>> GetEmailsByDateAsync(DateTime startDate, DateTime endDate)
        {
            // Chama o repositório para buscar os e-mails
            var emails = await _emailRepository.GetEmailsByDateAsync(startDate, endDate);

            // Mapeia os e-mails para a resposta
            var response = _mapper.Map<IEnumerable<EmailGetParametersResponse>>(emails);

            return response;
        }

        public async Task<IEnumerable<EmailGetParametersResponse>> SearchEmailsAsync(SearchEmailFilterRequest filter)
        {
            // Cria o predicado de busca dinâmico baseado nos parâmetros
            Expression<Func<Email, bool>> predicate = email =>
               (string.IsNullOrEmpty(filter.Sender) || email.Sender.Contains(filter.Sender)) &&
               (!filter.Status.HasValue || email.Status == filter.Status) &&
               (filter.Recipients == null || email.Recipients.Any(r => filter.Recipients.Contains(r))) &&
               (string.IsNullOrEmpty(filter.Subject) || email.Subject.Contains(filter.Subject)) &&
               (string.IsNullOrEmpty(filter.Body) || email.Body.Contains(filter.Body)) &&
               (!filter.SentAfter.HasValue || email.SentAt >= filter.SentAfter) &&
               (!filter.SentBefore.HasValue || email.SentAt <= filter.SentBefore) &&
               (!filter.MinAttempts.HasValue || email.Attempts >= filter.MinAttempts) &&
               (!filter.MaxAttempts.HasValue || email.Attempts <= filter.MaxAttempts);

            var emails = await _emailRepository.SearchAsync(predicate);

            var response = _mapper.Map<IEnumerable<EmailGetParametersResponse>>(emails);

            return response;
        }

        public Task<(IEnumerable<Email>, int)> SearchEmailsWithPaginationAsync(Expression<Func<Email, bool>> predicate, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<EmailPostParametersResponse?> UpdateEmailAsync(Guid emailId, EmailPostParametersRequest request)
        {
            // Busca o e-mail pelo ID
            var existingEmail = await _emailRepository.GetByIdAsync(new EmailId(emailId));
            if (existingEmail == null)
            {
                return null; // Email não encontrado
            }

            // Atualiza os campos do e-mail com os dados da requisição
            existingEmail.Sender = request.Sender;
            existingEmail.Recipients = request.Recipients;
            existingEmail.Subject = request.Subject;
            existingEmail.Body = request.Body;

            // Salva as alterações no banco
            await _emailRepository.UpdateAsync(existingEmail);

            // Retorna o resultado como resposta
            return new EmailPostParametersResponse
            {
                Id = existingEmail.Id.ToString(),
                Status = existingEmail.Status
            };
        }

        public async Task<int> SendPendingEmailsAsync()
        {
            var pendingEmails = await _emailRepository.SearchAsync(e => e.Status == EmailStatus.Pending);

            if (!pendingEmails.Any())
                return 0; // Nenhum e-mail para enviar

            int sentCount = 0;

            foreach (var email in pendingEmails)
            {
                try
                {
                    // Enfileira o e-mail para envio no RabbitMQ
                    await _messageQueueService.EnqueueEmail(email);

                    // Marca o e-mail como enviado no domínio
                    email.MarkAsSent();

                    // Atualiza no banco de dados
                    await _emailRepository.UpdateAsync(email);

                    sentCount++;
                }
                catch
                {
                    email.MarkAsError();
                    await _emailRepository.UpdateAsync(email);

                    _logger.LogInformation("Houve uma falha na tentativa de envio");
                }
            }

            return sentCount;
        }

    }
}
