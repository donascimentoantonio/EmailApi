using EmailManagement.Domain.Models.Email;
using EmailManagement.Domain.Primitives;
using EmailManagement.Infrastructure.Persistence.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmailManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IPublisher publisher) :
            base(options)
        {
            _publisher = publisher;
        }

        public DbSet<Email> Emails { get; set; }
        public DbSet<Atachment> Atachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Email>(entity =>
            {
                entity.ToTable(Utils.ToSnakeCase(nameof(Email)!)); 

                entity.Property(e => e.Subject)
                      .HasColumnName(Utils.ToSnakeCase(nameof(Email.Subject)!)); 
                
                entity.Property(e => e.Recipients)
                      .HasColumnName(Utils.ToSnakeCase(nameof(Email.Recipients)!)); 
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var domainEvents = ChangeTracker.Entries<Entity>()
                .Select(e => e.Entity)
                .Where(e => e.GetDomainEvents().Any())
                .SelectMany(e =>
                {
                    var domainEvents = e.GetDomainEvents();

                    e.ClearDomainEvents();

                    return domainEvents;
                })
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
