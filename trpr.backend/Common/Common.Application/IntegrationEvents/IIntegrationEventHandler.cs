using Common.Domain;

namespace Common.Application.IntegrationEvents;

public interface IIntegrationEventHandler<in TEvent> : IIntegrationEventHandler
    where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent integrationEvent, CancellationToken cancellationToken = default);
}
public interface IIntegrationEventHandler
{
    Task HandleAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}
