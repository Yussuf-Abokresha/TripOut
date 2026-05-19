using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Modules.Trips.Infrastructure.Repositories;
using Modules.Trips.Infrastructure.Data;
using Modules.Trips.Infrastructure.Inbox;
using Modules.Trips.Infrastructure.Outbox;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Infrastructure.Options;
using Modules.Trips.Infrastructure.Clients;
using Modules.Trips.Application.Interfaces;
using Modules.Trips.Infrastructure.Services;
using Common.Application.Seeds;
using Modules.Trips.Infrastructure.Seeds;

namespace Modules.Trips.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        string dbConnectionString = configuration.GetConnectionString("RommieDb")!;
        services.AddDbContext<TripsDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(dbConnectionString, op =>
                {
                    op.MigrationsAssembly(AssemblyRefrence.Assembly);
                    op.UseNetTopologySuite();
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<PublishOutboxMessagesInterceptor>());
        });
        services.AddScoped<PublishOutboxMessagesInterceptor>();
        services.Configure<OutBoxOptions>(configuration.GetSection("Trips:OutBox"));
        services.Configure<InBoxOptions>(configuration.GetSection("Trips:InBox"));
        services.Configure<RoutingEngineOptions>(configuration.GetSection("Trips:RoutingEngine"));
        services.AddScoped<IDbConnectionFactory>(x => new DbConnectionFactory(dbConnectionString));
        services.AddScoped<RepositoryFactory>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ITripsDbContext>(x => x.GetRequiredService<TripsDbContext>());
        services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<TripsDbContext>());
        services.AddScoped<ISeed, PlacesCsvSeeder>();
        if (configuration.GetValue<bool>("Trips:UseMockRoutingService"))
            services.AddScoped<IRoutingService, MockRoutingService>(); // mock routing service for testing
        else
            services.AddScoped<IRoutingService, RoutingService>(); // real routing service implementation
        services.AddHttpClient<RoutingEngineClient>();
        // adding quartz for background jobs 
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        services.ConfigureOptions<ConfigureProcessInboxJob>();
        return services;
    }
}
