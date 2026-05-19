using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Users.Presentation
{
    public static class PresentationDependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            return services;
        }
    }
}