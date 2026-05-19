using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using Serilog.Context;
using Common.Application;
using Common.Application.Correlation;
using Common.Application.DomainEvents;
using Common.Domain;
using Modules.Conversations.Application.Abstractions;
using Common.Infrastructure.Outbox;

namespace Modules.Conversations.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxJob(
    IOptions<OutBoxOptions> options,
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ProcessOutboxJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.CreateSqlConnection();
        await using DbTransaction dbTransaction = await dbConnection.BeginTransactionAsync();
        var outbox_messages = await GetOutboxMessagesAsync(dbConnection, dbTransaction);
        logger.LogInformation("Beginning to process outbox messages");

        foreach (OutboxMessage outboxMessage in outbox_messages)
        {
            Exception? exception = null;
            try
            {
                DomainEvent domainEvent = JsonConvert.DeserializeObject<DomainEvent>(
                    outboxMessage.Content, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                )!;

                using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                var correlationAccessor =
                    serviceScope.ServiceProvider.GetRequiredService<ICorrelationIdAccessor>();

                correlationAccessor.CorrelationId = outboxMessage.CorrelationId;

                using (LogContext.PushProperty("CorrelationId", outboxMessage.CorrelationId))
                {
                    IDomainEventDispatcher publisher = serviceScope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();
                    await publisher.DispatchAsync(domainEvent);
                }
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
                logger.LogError(exception, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
            }
            finally
            {
                await UpdateOutboxMessage(dbConnection, dbTransaction, outboxMessage, exception);
            }
        }
        await dbTransaction.CommitAsync();
        logger.LogInformation("Completed processing the outbox message ");
    }
    private async Task<IReadOnlyCollection<OutboxMessage>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction dbTransaction)
    {
        string query =
        $"""
        SELECT
        id AS {nameof(OutboxMessage.Id)},
        correlation_id as {nameof(OutboxMessage.CorrelationId)},
        type AS {nameof(OutboxMessage.Type)},
        content AS {nameof(OutboxMessage.Content)},
        occurred_on_utc AS {nameof(OutboxMessage.OccurredOnUtc)},
        processed_on_utc AS {nameof(OutboxMessage.ProcessedOnUtc)},
        error AS Error
        FROM {Schema.Conversations}.outbox_messages
        WHERE processed_on_utc IS NULL
        ORDER BY "occurred_on_utc"
        FOR UPDATE
        LIMIT {options.Value.BatchSize}
        """;
        var OutboxMessages = await connection.QueryAsync<OutboxMessage>(query, new { }, dbTransaction);
        return [.. OutboxMessages];
    }

    private async Task UpdateOutboxMessage(
        IDbConnection connection,
        IDbTransaction dbTransaction,
        OutboxMessage outboxMessage,
        Exception? exception
    )
    {
        const string query =
        $"""
        UPDATE 
            {Schema.Conversations}.outbox_messages
        SET
            processed_on_utc = @ProcessedOnUtc ,
            error = @Error
        WHERE id = @Id
        """;

        await connection.ExecuteAsync(query, new
        {
            ProcessedOnUtc = DateTime.UtcNow,
            Error = exception?.Message,
            outboxMessage.Id
        }, dbTransaction);
    }

}

