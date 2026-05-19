using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Modules.Notifications.Application.Abstractions;
using Quartz;
using Modules.Notifications.Infrastructure.Data;
using Modules.Notifications.Infrastructure.Repositories;
using Modules.Notifications.Infrastructure.Inbox;
using Modules.Notifications.Infrastructure.Outbox;

namespace Modules.Notifications.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("RommieDb")!;
        services.AddDbContext<NotificationsDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(dbConnectionString, op =>
                {
                    op.MigrationsAssembly(AssemblyRefrence.Assembly);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<PublishOutboxMessagesInterceptor>());
        });
        services.AddScoped<PublishOutboxMessagesInterceptor>();
        services.Configure<OutBoxOptions>(configuration.GetSection("Notifications:OutBox"));
        services.Configure<InBoxOptions>(configuration.GetSection("Notifications:Inbox"));
        services.AddScoped<IDbConnectionFactory>(x => new DbConnectionFactory(dbConnectionString));
        services.AddScoped<RepositoryFactory>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<INotificationsDbContext>(x => x.GetRequiredService<NotificationsDbContext>());
        services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<NotificationsDbContext>());
        // adding quartz for background jobs 
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        services.ConfigureOptions<ConfigureProcessInboxJob>();
        return services;
    }
}
