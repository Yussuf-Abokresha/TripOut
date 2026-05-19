using Microsoft.Extensions.Options;
using Quartz;

namespace Modules.Notifications.Infrastructure.Outbox;

public class ConfigureProcessOutboxJob(IOptions<OutBoxOptions> outboxOptions) : IConfigureOptions<QuartzOptions>
{
    private readonly OutBoxOptions _outboxOptions = outboxOptions.Value;
    public void Configure(QuartzOptions options)
    {
        if (_outboxOptions.Enabled)
        {
            string jobName = typeof(ProcessOutboxJob).FullName!;
            var jobKey = new JobKey(jobName);
            options
            .AddJob<ProcessOutboxJob>(configure => configure.WithIdentity(jobKey))
            .AddTrigger(configure =>
                configure.ForJob(jobKey)
                .WithSimpleSchedule(
                    schedule => schedule.WithIntervalInSeconds(_outboxOptions.TimeSpanInSeconds)
                                        .RepeatForever()
                )
            );
        }
    }

}
