using Microsoft.Extensions.DependencyInjection;
using Common.Application.DomainEvents.Extensions;
using Modules.Conversations.Application.Services;

namespace Modules.Conversations.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        # region Options
        #endregion

        #region  factories
        #endregion

        #region  services
        services.AddScoped<ChatService>();
        #endregion

        return services;
    }


}
