using EmailManagement.Domain.Models.Email;
using EmailManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmailManagement.Infrastructure.Repositories
{
    public sealed class EmailRepository : IEmailRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Email?> GetByIdAsync(EmailId id)
        {
            return await _context.Emails
                .Include(e => e.Atachments)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task SaveAsync(Email email)
        {
            await _context.Emails.AddAsync(email);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Email email)
        {
            _context.Emails.Update(email);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Email email)
        {
            _context.Emails.Remove(email);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Email>> GetEmailsByDateAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Emails
                .Include(e => e.Atachments) // Inclui os anexos
                .Where(e => e.SentAt >= startDate && e.SentAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Email>> SearchAsync(Expression<Func<Email, bool>> predicate)
        {
            return await _context.Emails
                .Include(e => e.Atachments)
                .Where(predicate)
                .ToListAsync();
        }
    }
}
