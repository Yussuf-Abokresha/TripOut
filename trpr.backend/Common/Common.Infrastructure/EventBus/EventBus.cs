using Common.Application.Correlation;
using Common.Application.EventBus;
using Common.Domain;
using Rebus.Bus;

namespace Common.Infrastructure.EventBus;

internal class EventBus(IBus bus, ICorrelationIdAccessor correlationIdAccessor) : IEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        integrationEvent.CorrelationId = correlationIdAccessor.CorrelationId ?? Guid.Empty.ToString();
        await bus.Publish(integrationEvent);
    }

}
