
using Modules.Notifications.Domain.Abstractions;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Domain.Entities;

public class Notification : Entity
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public ContentType ContentType { get; set; } = ContentType.Pure;
    public bool NotifyEmail { get; set; }
    public bool NotifyPhone { get; set; }
    public bool NotifySystem { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public static Notification Create(
        string message,
        ContentType contentType,
        bool notifyEmail,
        bool notifyPhone,
        bool notifySystem,
        Guid userId)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Message = message,
            ContentType = contentType,
            NotifyEmail = notifyEmail,
            NotifyPhone = notifyPhone,
            NotifySystem = notifySystem,
            UserId = userId
        };

        notification.RaiseDomainEvent(
            new DomainEvents
                .NotificationCreatedDomainEvent
            {
                NotificationId = notification.Id
            });

        return notification;
    }
}
