using Common.Domain;

namespace Common.Application.IntegrationEvents;

public abstract class IntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public abstract Task HandleAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);

    public Task HandleAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default) =>
        HandleAsync((TIntegrationEvent)integrationEvent, cancellationToken);
}
