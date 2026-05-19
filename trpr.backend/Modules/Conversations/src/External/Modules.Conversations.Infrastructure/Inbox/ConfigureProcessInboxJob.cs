using Microsoft.Extensions.Options;
using Quartz;

namespace Modules.Conversations.Infrastructure.Inbox;

public class ConfigureProcessInboxJob(IOptions<InBoxOptions> inboxOptions) : IConfigureOptions<QuartzOptions>
{
    private readonly InBoxOptions _inboxOptions = inboxOptions.Value;
    public void Configure(QuartzOptions options)
    {
        if (_inboxOptions.Enabled)
        {
            string jobName = typeof(ProcessInboxJob).FullName!;
            var jobKey = new JobKey(jobName);
            options
            .AddJob<ProcessInboxJob>(configure => configure.WithIdentity(jobKey))
            .AddTrigger(configure =>
                configure.ForJob(jobKey)
                .WithSimpleSchedule(
                    schedule => schedule.WithIntervalInSeconds(_inboxOptions.TimeSpanInSeconds)
                                        .RepeatForever()
                )
            );
        }
    }

}
