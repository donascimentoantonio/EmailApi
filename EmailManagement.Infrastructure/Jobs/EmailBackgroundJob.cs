using EmailManagement.Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace EmailManagement.Infrastructure.Jobs
{
    public class EmailBackgroundJob : IJob
    {
        private readonly ILogger<EmailBackgroundJob> _logger;
        private readonly IEmailService _emailService;

        public EmailBackgroundJob(
            ILogger<EmailBackgroundJob> logger,
            IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{UtcNow} Execução dos emails pendentes", DateTime.UtcNow);

            await _emailService.SendPendingEmailsAsync();
        }
    }
}
