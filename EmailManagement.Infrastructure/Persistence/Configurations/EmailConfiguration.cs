

using EmailManagement.Domain.Models.Email;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace EmailManagement.Infrastructure.Persistence.Configurations
{
    public class EmailConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable(Utils.ToSnakeCase(nameof(Email)));

            builder.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new EmailId(value))
           .HasColumnName(Utils.ToSnakeCase(nameof(Email.Id)))
           .ValueGeneratedNever();

            builder.Property(e => e.Sender)
           .HasColumnName(Utils.ToSnakeCase(nameof(Email.Sender)))
           .IsRequired();

            builder.Property(e => e.Recipients)
       .HasConversion(
           v => JsonConvert.SerializeObject(v),
           v => JsonConvert.DeserializeObject<List<string>>(v)!)
       .HasColumnName(Utils.ToSnakeCase(nameof(Email.Recipients)))
       .HasColumnType("jsonb") // Adicionar esta linha
       .Metadata.SetValueComparer(
           new ValueComparer<List<string>>(
               (c1, c2) => c1!.SequenceEqual(c2!),
               c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
               c => c.ToList()
           )
       );

            builder.Property(e => e.Subject)
                .HasColumnName(Utils.ToSnakeCase(nameof(Email.Subject)))
                .IsRequired();

            builder.Property(e => e.Body)
                .HasColumnName(Utils.ToSnakeCase(nameof(Email.Body)))
                .IsRequired();

            builder.Property(e => e.SentAt)
                .HasColumnName(Utils.ToSnakeCase(nameof(Email.SentAt)));

            builder.Property(e => e.Status)
                .HasColumnName(Utils.ToSnakeCase(nameof(Email.Status)))
                .IsRequired();

            builder.Property(e => e.Attempts)
                .HasColumnName(Utils.ToSnakeCase(nameof(Email.Attempts)))
                .IsRequired();

            builder.Property(e => e.LastAttemptAt)
                .HasColumnName(Utils.ToSnakeCase(nameof(Email.LastAttemptAt)));

            // Índices para otimizar consultas
            builder.HasIndex(e => e.SentAt);
            builder.HasIndex(e => e.Sender);
            builder.HasIndex(e => e.Recipients);

            // Relacionamento com Atachment (um email para muitos anexos)
            builder.HasMany(e => e.Atachments)
                .WithOne()
                .HasForeignKey(a => a.EmailId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
