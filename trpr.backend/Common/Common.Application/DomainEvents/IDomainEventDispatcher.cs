using Common.Domain;

namespace Common.Application.DomainEvents
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    }

}