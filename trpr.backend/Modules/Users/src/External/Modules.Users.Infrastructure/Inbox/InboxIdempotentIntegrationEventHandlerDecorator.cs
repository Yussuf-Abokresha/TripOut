using Common.Application;
using Common.Application.IntegrationEvents;
using Common.Domain;
using Dapper;
using Microsoft.Extensions.Logging;
using Modules.Users.Application.Abstractions;

namespace Modules.Users.Infrastructure.Inbox
{
    public class InboxIdempotentIntegrationEventHandlerDecorator<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> innerHandler,
    IDbConnectionFactory dbConnectionFactory,
    ILogger<InboxIdempotentIntegrationEventHandlerDecorator<TIntegrationEvent>> logger) : IntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
    {
        public async override Task HandleAsync(TIntegrationEvent notification, CancellationToken cancellationToken = default)
        {
            await using var connection = await dbConnectionFactory.CreateSqlConnection();

            IIntegrationEvent integrationEvent = notification;
            var inboxConsumerMessage = new
            {
                Id = integrationEvent.Id,
                HandlerName = innerHandler.GetType().Name
            };

            const string query = $"SELECT COUNT(1) FROM {Schema.Users}.inbox_consumer_messages WHERE id = @id AND handler_name = @HandlerName";
            var exists = await connection.ExecuteScalarAsync<int>(query, inboxConsumerMessage);

            if (exists > 0)
            {
                logger.LogWarning("Duplicate event detected: {EventType} with ID {EventId}. Skipping processing.", typeof(TIntegrationEvent).Name, inboxConsumerMessage.Id);
                return;
            }

            logger.LogInformation("Processing event {EventType} with ID {EventId}.", typeof(TIntegrationEvent).Name, inboxConsumerMessage.Id);
            await innerHandler.HandleAsync(notification, cancellationToken);

            const string insertQuery = $"INSERT INTO {Schema.Users}.inbox_consumer_messages (id, handler_name) VALUES (@Id, @HandlerName)";
            await connection.ExecuteAsync(insertQuery, inboxConsumerMessage);

            logger.LogInformation("Stored event {EventType} with ID {EventId} in inboxConsumerMessages.", typeof(TIntegrationEvent).Name, inboxConsumerMessage.Id);

        }
    }
}