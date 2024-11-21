
using EmailManagement.Domain.Models.Email;
using Microsoft.EntityFrameworkCore;

namespace EmailManagement.Infrastructure.Persistence.Abstractions
{
    public interface IApplicationDbContext
    {
        public DbSet<Email> Emails { get; set; }
        public DbSet<Atachment> Atachments { get; set; }
    }
}
