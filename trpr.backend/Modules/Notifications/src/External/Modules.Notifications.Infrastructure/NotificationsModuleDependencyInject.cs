using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents;
using Common.Domain.IntragationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Notifications.Application;
using Modules.Notifications.Contracts.Contracts;
using Modules.Notifications.Infrastructure.Inbox;
using Modules.Notifications.Infrastructure.Outbox;
using Modules.Notifications.Presentation;
using Rebus.Handlers;

namespace Modules.Notifications.Infrastructure
{
    public static class NotificationsModuleDependencyInject
    {
        public static IServiceCollection AddNotificationsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddPresentation();

            #region Integration Events Subscription
            services
                .AddTransient<IHandleMessages<UserCreatedIntegrationEvent>, BaseIngtegrationEventHandler<UserCreatedIntegrationEvent>>();

            services
                .AddTransient<IHandleMessages<SendMessageIntegrationEvent>,
                BaseIngtegrationEventHandler<SendMessageIntegrationEvent>>();
            services
                .AddTransient<IHandleMessages<SendSystemMessageIntegrationEvent>,
                BaseIngtegrationEventHandler<SendSystemMessageIntegrationEvent>>();
            #endregion

            services.AddIntegrationEventHandlers();

            services.AddDomainEventHandlerDecorators(
                cfg =>
                    cfg.AddAssemblies(Application.AssemblyRefrence.Assembly)
                    .AddPipeline(typeof(OutboxIdempotentDomainEventHandlerDecorator<>)));
            #region Contracts
            services.AddScoped<INotifiyContract, NotifyContract>();
            #endregion
            return services;
        }

        private static void AddIntegrationEventHandlers(this IServiceCollection services)
        {
            Type[] integrationEventHandlers = Notifications.Presentation.AssemblyRefrence.Assembly
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
