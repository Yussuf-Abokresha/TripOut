using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application.Services;

namespace Modules.Notifications.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        #region Options
        #endregion

        #region  factories
        #endregion

        #region  services
        services.AddScoped<UserService>();
        services.AddScoped<TemplateService>();
        #endregion

        #region  contracts
        #endregion
        return services;
    }

}
