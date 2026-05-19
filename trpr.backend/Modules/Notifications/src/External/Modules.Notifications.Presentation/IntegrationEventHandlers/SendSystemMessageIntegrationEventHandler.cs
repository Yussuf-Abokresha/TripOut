using Common.Application.IntegrationEvents;
using Common.Domain.IntragationEvents;
using Microsoft.Extensions.Logging;

namespace Modules.Notifications.Presentation.IntegrationEventHandlers;

public class SendSystemMessageIntegrationEventHandler(ILogger<SendSystemMessageIntegrationEventHandler> logger) : IntegrationEventHandler<SendSystemMessageIntegrationEvent>
{
    public override Task HandleAsync(SendSystemMessageIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("integration event json {integrationEvent}", integrationEvent);
        return Task.CompletedTask;
    }
}
