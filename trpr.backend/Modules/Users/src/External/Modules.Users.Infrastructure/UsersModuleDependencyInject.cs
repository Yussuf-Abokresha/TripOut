using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Users.Application;
using Modules.Users.Infrastructure.Inbox;
using Modules.Users.Infrastructure.Outbox;
using Modules.Users.Presentation;

namespace Modules.Users.Infrastructure
{
    public static class UsersModuleDependencyInject
    {
        public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddPresentation();

            services.AddIntegrationEventHandlers();

            services.AddDomainEventHandlerDecorators(
                cfg =>
                    cfg.AddAssemblies(Application.AssemblyRefrence.Assembly)
                    .AddPipeline(typeof(OutboxIdempotentDomainEventHandlerDecorator<>)));

            return services;
        }
        private static void AddIntegrationEventHandlers(this IServiceCollection services)
        {
            Type[] integrationEventHandlers = Users.Presentation.AssemblyRefrence.Assembly
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
