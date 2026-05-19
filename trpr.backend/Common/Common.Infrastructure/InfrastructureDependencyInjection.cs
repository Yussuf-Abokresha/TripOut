using System.Reflection;
using Common.Application;
using Common.Application.Buckets;
using Common.Application.EventBus;
using Common.Infrastructure.Buckets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Minio;

namespace Common.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services,
    IConfiguration configuration, params Assembly[] assemblies)
    {
        services.TryAddSingleton<IEventBus, EventBus.EventBus>();
        services.Configure<MinioSettings>(configuration.GetSection("Minio"));
        services.AddSingleton<IMinioClient>(sp =>
        {
            MinioSettings settings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;
            return new MinioClient()
                .WithEndpoint(settings.Endpoint)
                .WithCredentials(settings.AccessKey, settings.SecretKey)
                .WithSSL(settings.UseSSL)
                .Build();
        });
        services.AddSingleton<IFileService, MinioFileService>();
        services.Scan(
            scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IMapper<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        return services;
    }
}
