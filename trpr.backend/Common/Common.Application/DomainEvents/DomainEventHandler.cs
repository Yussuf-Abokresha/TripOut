using Common.Domain;

namespace Common.Application.DomainEvents;

public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    public abstract Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}