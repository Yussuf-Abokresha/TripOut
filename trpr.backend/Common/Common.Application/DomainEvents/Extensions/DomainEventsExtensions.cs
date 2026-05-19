using Microsoft.Extensions.DependencyInjection;

namespace Common.Application.DomainEvents.Extensions
{
    public static class DomainEventsExtensions
    {
        public static void AddDomainEventHandlerDecorators(this IServiceCollection services, Action<DomainEventConfiguration> configure)
        {
            var config = new DomainEventConfiguration();
            configure(config);

            List<Type> domainEventHandlers = [];
            foreach (var assembly in config.Assemblies.Assemblies)
            {
                var assemblyDomainEventHandlers = assembly
                   .GetTypes()
                   .Where(t =>
                   !t.IsAbstract &&
                   !t.IsInterface &&
                   !t.IsGenericTypeDefinition &&
                   t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                       i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                   )
                   .ToList();
                domainEventHandlers.AddRange(assemblyDomainEventHandlers);
            }

            foreach (Type domainEventHandler in domainEventHandlers)
            {
                Type domainEvent = domainEventHandler
                    .GetInterfaces()
                    .Single(x => x.IsGenericType)
                    .GetGenericArguments()
                    .Single();
                Type handlerInterface = typeof(IDomainEventHandler<>)
                    .MakeGenericType(domainEvent);
                foreach (var decorator in config.Decorators.Decorators)
                {
                    services.TryDecorate(
                        handlerInterface,
                        decorator.MakeGenericType(domainEvent));
                }
            }
        }
        public static void AddDomainEventHandlers(this IServiceCollection services, Action<AssemblyConfiguration> configure)
        {
            var pipeline = new AssemblyConfiguration();
            configure(pipeline);

            List<Type> domainEventHandlers = [];
            foreach (var assembly in pipeline.Assemblies)
            {
                var assemblyDomainEventHandlers = assembly
                   .GetTypes()
                   .Where(t =>
                   !t.IsAbstract &&
                   !t.IsInterface &&
                   !t.IsGenericTypeDefinition &&
                   t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                       i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                   )
                   .ToList();
                domainEventHandlers.AddRange(assemblyDomainEventHandlers);
            }

            foreach (Type domainEventHandler in domainEventHandlers)
            {
                Type domainEvent = domainEventHandler
                    .GetInterfaces()
                    .Single(x => x.IsGenericType)
                    .GetGenericArguments()
                    .Single();
                Type handlerInterface = typeof(IDomainEventHandler<>)
                    .MakeGenericType(domainEvent);

                services.AddTransient(handlerInterface, domainEventHandler);
            }
        }
    }
}