using EmailManagement.Domain.Models.Email;

namespace EmailManagement.Domain.Services
{
    public interface IHttpClientService
    {
        Task SendEmailToApiAsync(Email email);
    }
}
