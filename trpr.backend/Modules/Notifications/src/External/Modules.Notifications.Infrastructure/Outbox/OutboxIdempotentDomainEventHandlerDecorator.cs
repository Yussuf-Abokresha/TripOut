using Dapper;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Common.Application;
using Common.Domain;
using Common.Application.DomainEvents;

namespace Modules.Notifications.Infrastructure.Outbox;

public class OutboxIdempotentDomainEventHandlerDecorator<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> innerHandler,
    IDbConnectionFactory dbConnectionFactory,
    ILogger<OutboxIdempotentDomainEventHandlerDecorator<TDomainEvent>> logger) : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async Task HandleAsync(TDomainEvent notification, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateSqlConnection();

        IDomainEvent domainEvent = notification;
        var outboxConsumerMessage = new
        {
            Id = domainEvent.Id,
            HandlerName = innerHandler.GetType().Name
        };

        const string query = $"SELECT COUNT(1) FROM {Schema.Notifications}.outbox_consumer_messages WHERE id = @id AND handler_name = @HandlerName";
        var exists = await connection.ExecuteScalarAsync<int>(query, outboxConsumerMessage);

        if (exists > 0)
        {
            logger.LogWarning("Duplicate event detected: {EventType} with ID {EventId}. Skipping processing.", typeof(TDomainEvent).Name, outboxConsumerMessage.Id);
            return;
        }

        logger.LogInformation("Processing event {EventType} with ID {EventId}.", typeof(TDomainEvent).Name, outboxConsumerMessage.Id);
        await innerHandler.HandleAsync(notification, cancellationToken);

        const string insertQuery = $"INSERT INTO {Schema.Notifications}.outbox_consumer_messages (id, handler_name) VALUES (@Id, @HandlerName)";
        await connection.ExecuteAsync(insertQuery, outboxConsumerMessage);

        logger.LogInformation("Stored event {EventType} with ID {EventId} in OutboxConsumerMessages.", typeof(TDomainEvent).Name, outboxConsumerMessage.Id);

    }

}
