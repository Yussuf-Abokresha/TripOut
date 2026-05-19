using Microsoft.Extensions.DependencyInjection;
using Common.Application.DomainEvents.Extensions;
using Modules.Trips.Application.Services;

namespace Modules.Trips.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        # region Options
        #endregion

        #region  factories
        #endregion

        #region  services
        services.AddScoped<PlaceService>();
        services.AddScoped<TripService>();
        #endregion

        return services;
    }


}
