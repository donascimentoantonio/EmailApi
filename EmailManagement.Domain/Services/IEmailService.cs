using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Models.Email;
using System.Linq.Expressions;

namespace EmailManagement.Domain.Services
{
    public interface IEmailService
    {
        Task<Email> CreateEmailAsync(EmailPostParametersRequest request);
        Task<Email> UpdateEmailAsync(Guid emailId, EmailPostParametersRequest request);
        Task<bool> DeleteEmailAsync(Guid emailId);
        Task<Email?> GetEmailByIdAsync(Guid emailId);
        Task<IEnumerable<Email>> GetEmailsByDateAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Email>> SearchEmailsAsync(Expression<Func<Email, bool>> predicate);
        Task<(IEnumerable<Email>, int)> SearchEmailsWithPaginationAsync(
            Expression<Func<Email, bool>> predicate, int pageNumber, int pageSize);
    }
}
