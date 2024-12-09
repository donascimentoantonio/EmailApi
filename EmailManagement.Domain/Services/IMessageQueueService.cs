using EmailManagement.Domain.Models.Email;

namespace EmailManagement.Domain.Services
{
    public interface IMessageQueueService
    {
        Task EnqueueEmail(Email email);
    }
}
