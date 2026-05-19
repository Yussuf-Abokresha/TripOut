using Common.Domain.IntragationEvents;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace Api.Extensions;

public static class IntegrationEventsExtensions
{
    public static void AddIntegrationEvents(this IServiceCollection services)
    {
        services.AddRebus(configure => configure
            .Routing(r => r.TypeBased()
                .MapAssemblyOf<Program>("modular-monolith-queue")
            )
            .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), "modular-monolith"))
            .Options(o => o.SetNumberOfWorkers(1)),
            onCreated: async bus =>
            {
                await bus.Subscribe<UserCreatedIntegrationEvent>();
                await bus.Subscribe<SendMessageIntegrationEvent>();
                await bus.Subscribe<SendSystemMessageIntegrationEvent>();
            }
        );
    }
}
