namespace EmailManagement.Domain.Models.Email
{
    public record Atachment(Guid Id, string File, string FileName, string FileExtension, EmailId EmailId);
}
