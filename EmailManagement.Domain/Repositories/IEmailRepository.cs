using EmailManagement.Domain.Models.Email;
using System.Linq.Expressions;

namespace EmailManagement.Infrastructure.Repositories
{
    public interface IEmailRepository
    {
        Task<Email?> GetByIdAsync(EmailId id);
        void Add(Email email);
        Task SaveAsync(Email email);
        Task UpdateAsync(Email email);
        Task DeleteAsync(Email email);
        Task<IEnumerable<Email>> GetEmailsByDateAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Email>> SearchAsync(Expression<Func<Email, bool>> predicate);
        Task<(IEnumerable<Email>, int)> SearchWithPaginationAsync(
          Expression<Func<Email, bool>> predicate,
          int pageNumber,
          int pageSize);
    }
}
