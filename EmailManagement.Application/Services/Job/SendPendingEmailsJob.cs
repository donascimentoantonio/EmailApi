using EmailManagement.Domain.Services;
using Quartz;

namespace EmailManagement.Application.Services.Job
{
    public class SendPendingEmailsJob : IJob
    {
        private readonly IEmailService _emailService;

        public SendPendingEmailsJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _emailService.SendPendingEmailsAsync();
        }
    }
}
