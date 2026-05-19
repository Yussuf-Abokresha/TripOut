using Common.Application.IntegrationEvents;
using Common.Domain.IntragationEvents;

namespace Modules.Notifications.Presentation.IntegrationEventHandlers;

public class UserCreatedIntegrationEventHandler() : IntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async override Task HandleAsync(UserCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }

}
