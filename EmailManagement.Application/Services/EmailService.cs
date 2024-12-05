using AutoMapper;
using EmailManagement.Domain.Dtos.v1.Request;
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

        // Criação de um novo email
        public async Task<Email> CreateEmailAsync(EmailPostParametersRequest request)
        {
            var email = _mapper.Map<Email>(request);
            email.Id = new EmailId(Guid.NewGuid()); 
            await _emailRepository.SaveAsync(email);
            return email;
        }

        public Task<bool> DeleteEmailAsync(Guid emailId)
        {
            throw new NotImplementedException();
        }

        public Task<Email?> GetEmailByIdAsync(Guid emailId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Email>> GetEmailsByDateAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Email>> SearchEmailsAsync(Expression<Func<Email, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<Email>, int)> SearchEmailsWithPaginationAsync(Expression<Func<Email, bool>> predicate, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Email> UpdateEmailAsync(Guid emailId, EmailPostParametersRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
