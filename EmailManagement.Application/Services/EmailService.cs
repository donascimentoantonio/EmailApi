using AutoMapper;
using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Dtos.v1.Response;
using EmailManagement.Domain.Enum;
using EmailManagement.Domain.Models.Email;
using EmailManagement.Domain.Services;
using EmailManagement.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace EmailManagement.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IMapper _mapper;

        public EmailService(
            IEmailRepository emailRepository,
            IMapper mapper)
        {
            _emailRepository = emailRepository;
            _mapper = mapper;
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

        public Task<EmailGetParametersResponse?> GetEmailByIdAsync(Guid emailId)
        {
            throw new NotImplementedException();
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

        public Task<EmailPostParametersResponse> UpdateEmailAsync(Guid emailId, EmailPostParametersRequest request)
        {
            throw new NotImplementedException();
        }

    }
}
