using Common.Application.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Domain.DomainEvents;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Projects;

public class NotificationCreatedDomainEventHandler(
    IRepository<Notification> notificationRepository
) : IDomainEventHandler<NotificationCreatedDomainEvent>
{
    public async Task HandleAsync(NotificationCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        Notification notification =
            await notificationRepository
                .GetFirstOrDefaultByFilter(x => x.Id == domainEvent.NotificationId, x => x.Include(x => x.User)) ??
                throw new Exception($"Notification with ID {domainEvent.NotificationId} not found.");

        if (notification.NotifyEmail)
        {            // Simulate sending email
            Console.WriteLine($"Sending email notification to {notification.User.Email} with message: {notification.Message}");
        }

        if (notification.NotifySystem)
        {            // Simulate sending in-app notification
            Console.WriteLine($"Sending in-app notification to user {notification.User.UserName} with message: {notification.Message}");
        }

        if (notification.NotifyPhone)
        {            // Simulate sending push notification
            Console.WriteLine($"Sending push notification to user {notification.User.PhoneNumber} with message: {notification.Message}");
        }
        await Task.CompletedTask; // Simulate async work
    }

}
