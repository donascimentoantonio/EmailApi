
using Microsoft.Extensions.Options;
using Quartz;

namespace EmailManagement.Infrastructure.Jobs
{
    public class EmailBackgroundJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(EmailBackgroundJob));
            options
                .AddJob<EmailBackgroundJob>(jobBuilder => 
                    jobBuilder.WithIdentity(jobKey)
                                .DisallowConcurrentExecution()
                    )
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInMinutes(3).RepeatForever()));
        }
    }
}
