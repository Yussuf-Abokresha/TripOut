using System.Text.Json;
using Common.Application.IntegrationEvents;
using Common.Domain.IntragationEvents;
using Microsoft.Extensions.Logging;

namespace Modules.Notifications.Presentation.IntegrationEventHandlers;

public class SendMessageIntegrationEventHandler(ILogger<SendMessageIntegrationEventHandler> logger) : IntegrationEventHandler<SendMessageIntegrationEvent>
{
    public override Task HandleAsync(SendMessageIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("integration event json {integrationEvent}", integrationEvent);
        return Task.CompletedTask;
    }
}
