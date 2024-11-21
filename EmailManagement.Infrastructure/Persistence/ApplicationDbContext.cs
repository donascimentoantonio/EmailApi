using EmailManagement.Domain.Models.Email;
using EmailManagement.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EmailManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : 
            base(options)
        {
        }

        public DbSet<Email> Emails { get; set; }
        public DbSet<Atachment> Atachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
