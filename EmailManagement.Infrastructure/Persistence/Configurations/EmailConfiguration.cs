

using EmailManagement.Domain.Models.Email;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmailManagement.Infrastructure.Persistence.Configurations
{
    public class EmailConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("Emails");

            builder.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new EmailId(value));
            builder.Property(e => e.Sender).IsRequired();
            builder.Property(e => e.Subject).IsRequired();
            builder.Property(e => e.Body).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.Attempts).IsRequired();

            // Índices para otimizar consultas
            builder.HasIndex(e => e.SentAt);
            builder.HasIndex(e => e.Sender);
            builder.HasIndex(e => e.Recipients); // Aqui você pode precisar ajustar 

            // Relacionamento com Atachment (um email para muitos anexos)
            builder.HasMany(e => e.Atachments)
                  .WithOne()
                  .HasForeignKey(a => a.EmailId) // Define a propriedade EmailId como chave estrangeira
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
