using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents;
using Common.Domain.IntragationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Trips.Application;
using Modules.Trips.Infrastructure.Inbox;
using Modules.Trips.Infrastructure.Outbox;
using Modules.Trips.Presentation;
using Rebus.Handlers;

namespace Modules.Trips.Infrastructure
{
    public static class TripsModuleDependencyInject
    {
        public static IServiceCollection AddTripsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddPresentation();

            #region Integration Events Subscription
            services.AddTransient<IHandleMessages<UserCreatedIntegrationEvent>, BaseIngtegrationEventHandler<UserCreatedIntegrationEvent>>();
            #endregion

            services.AddIntegrationEventHandlers();

            services.AddDomainEventHandlerDecorators(
                cfg =>
                    cfg.AddAssemblies(Application.AssemblyRefrence.Assembly)
                    .AddPipeline(typeof(OutboxIdempotentDomainEventHandlerDecorator<>)));

            return services;
        }

        private static void AddIntegrationEventHandlers(this IServiceCollection services)
        {
            Type[] integrationEventHandlers = Trips.Presentation.AssemblyRefrence.Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
                .ToArray();

            foreach (Type integrationEventHandler in integrationEventHandlers)
            {
                services.TryAddScoped(integrationEventHandler);

                Type integrationEvent = integrationEventHandler
                    .GetInterfaces()
                    .Single(i => i.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                Type closedIdempotentHandler =
                    typeof(InboxIdempotentIntegrationEventHandlerDecorator<>).MakeGenericType(integrationEvent);

                services.Decorate(integrationEventHandler, closedIdempotentHandler);
            }
        }
    }
}
