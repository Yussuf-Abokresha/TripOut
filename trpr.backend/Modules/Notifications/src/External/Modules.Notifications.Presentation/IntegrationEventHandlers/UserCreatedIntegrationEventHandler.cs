using Common.Application.IntegrationEvents;
using Common.Domain.IntragationEvents;
using Modules.Notifications.Application.Services;

namespace Modules.Notifications.Presentation.IntegrationEventHandlers;

public class UserCreatedIntegrationEventHandler(UserService userService) : IntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async override Task HandleAsync(UserCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await userService.CreateUser(integrationEvent.UserId, integrationEvent.UserName, integrationEvent.FirstName, integrationEvent.LastName, cancellationToken);
    }

}
