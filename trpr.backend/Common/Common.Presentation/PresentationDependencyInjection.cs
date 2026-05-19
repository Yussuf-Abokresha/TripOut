using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authentication;
using Common.Presentation.AuthenticationHandlers;
using Rebus.Config;
using Microsoft.AspNetCore.HttpLogging;
using System.Text.Json.Serialization;

namespace Common.Presentation;

public static class PresentationDependencyInjection
{
    public static IServiceCollection AddCommonPresentation(this IServiceCollection services, params Assembly[] assemblies)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            })
            .AddApplicationParts(assemblies);
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.Response;
        });
        services.AddAuthentication(SchemaDefaults.ForwardedClaimsSchema)
        .AddScheme<AuthenticationSchemeOptions, ForwardedClaimsAuthenticationHandler>(SchemaDefaults.ForwardedClaimsSchema, null);
        services.AddAuthorization();

        return services;
    }
}
