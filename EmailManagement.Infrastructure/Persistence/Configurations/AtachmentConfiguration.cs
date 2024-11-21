using EmailManagement.Domain.Models.Email;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmailManagement.Infrastructure.Persistence.Configurations
{
    public class AtachmentConfiguration
: IEntityTypeConfiguration<Atachment>
    {
        public void Configure(EntityTypeBuilder<Atachment> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedNever();

            builder.Property(a => a.File).IsRequired();
            builder.Property(a => a.FileName).IsRequired();
            builder.Property(a => a.FileExtension).IsRequired();

            builder.HasIndex(a => a.EmailId);
        }
    }
}
