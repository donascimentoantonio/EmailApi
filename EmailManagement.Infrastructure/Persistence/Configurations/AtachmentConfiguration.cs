using EmailManagement.Domain.Models.Email;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmailManagement.Infrastructure.Persistence.Configurations
{
    public class AtachmentConfiguration : IEntityTypeConfiguration<Atachment>
    {
        public void Configure(EntityTypeBuilder<Atachment> builder)
        {
            builder.ToTable(Utils.ToSnakeCase(nameof(Atachment))); // "atachment"

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                   .HasColumnName(Utils.ToSnakeCase(nameof(Atachment.Id))) // "id"
                   .ValueGeneratedNever();

            builder.Property(a => a.File)
                   .HasColumnName(Utils.ToSnakeCase(nameof(Atachment.File))) // "file"
                   .IsRequired();

            builder.Property(a => a.FileName)
                   .HasColumnName(Utils.ToSnakeCase(nameof(Atachment.FileName))) // "file_name"
                   .IsRequired();

            builder.Property(a => a.FileExtension)
                   .HasColumnName(Utils.ToSnakeCase(nameof(Atachment.FileExtension))) // "file_extension"
                   .IsRequired();

            builder.Property(a => a.EmailId)
                   .HasColumnName(Utils.ToSnakeCase(nameof(Atachment.EmailId)))
                   .HasConversion(
                       emailId => emailId.Value, // Converte EmailId para string ao salvar no banco
                       value => new EmailId(value) // Converte string para EmailId ao ler do banco
                   );

            builder.HasIndex(a => a.EmailId);
        }
    }
}
