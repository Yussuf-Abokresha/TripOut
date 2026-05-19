using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Abstractions.Identity;
using Rebus.Handlers;

namespace Modules.Notifications.Presentation
{
    public static class PresentationDependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            return services;
        }

    }
}