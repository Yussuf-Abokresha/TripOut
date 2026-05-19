using Common.Domain;

namespace Modules.Users.Domain.Events;

public class UserCreatedDomainEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserCreatedDomainEvent(Guid userId)
    {
        UserId = userId;
    }
}
