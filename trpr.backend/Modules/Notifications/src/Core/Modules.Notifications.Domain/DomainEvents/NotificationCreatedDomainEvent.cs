using Common.Domain;

namespace Modules.Notifications.Domain.DomainEvents;

public class NotificationCreatedDomainEvent : DomainEvent
{
    public Guid NotificationId { get; set; }
}
