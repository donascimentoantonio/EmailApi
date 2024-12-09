using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Dtos.v1.Response;
using EmailManagement.Domain.Models.Email;
using System.Linq.Expressions;

namespace EmailManagement.Domain.Services
{
    public interface IEmailService
    {
        Task<IEnumerable<EmailPostParametersResponse>> SendEmailsAsync(List<EmailPostParametersRequest> requests);
        Task<EmailPostParametersResponse?> UpdateEmailAsync(Guid emailId, EmailPostParametersRequest request);
        Task<bool> DeleteEmailAsync(Guid emailId);
        Task<EmailGetParametersResponse?> GetEmailByIdAsync(Guid emailId);
        Task<IEnumerable<EmailGetParametersResponse>> GetEmailsByDateAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EmailGetParametersResponse>> SearchEmailsAsync(SearchEmailFilterRequest filter);
        Task<(IEnumerable<Email>, int)> SearchEmailsWithPaginationAsync(
            Expression<Func<Email, bool>> predicate, int pageNumber, int pageSize);
        Task<int> SendPendingEmailsAsync();
    }
}
