using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using Serilog.Context;
using Modules.Notifications.Application.Abstractions;
using Common.Application;
using Common.Domain;
using Common.Application.IntegrationEvents;
using Common.Infrastructure.Inbox;

namespace Modules.Notifications.Infrastructure.Inbox;

[DisallowConcurrentExecution]
public class ProcessInboxJob(
    IOptions<InBoxOptions> options,
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ProcessInboxJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.CreateSqlConnection();
        await using DbTransaction dbTransaction = await dbConnection.BeginTransactionAsync();
        var inbox_messages = await GetinboxMessagesAsync(dbConnection, dbTransaction);
        logger.LogInformation("Beginning to process inbox messages");

        foreach (InboxMessage inboxMessage in inbox_messages)
        {
            Exception? exception = null;
            try
            {
                IntegrationEvent integrationEvent = JsonConvert.DeserializeObject<IntegrationEvent>(
                    inboxMessage.Content, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                )!;
                using (LogContext.PushProperty("CorrelationId", inboxMessage.CorrelationId))
                {
                    using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                    IEnumerable<IIntegrationEventHandler> handlers = IntegrationEventHandlersFactory.GetHandlers(
                    integrationEvent.GetType(),
                    serviceScope.ServiceProvider,
                    Presentation.AssemblyRefrence.Assembly);

                    foreach (IIntegrationEventHandler integrationEventHandler in handlers)
                    {
                        await integrationEventHandler.HandleAsync(integrationEvent, context.CancellationToken);
                    }
                }
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
                logger.LogError(exception, "Exception while processing inbox message {MessageId}", inboxMessage.Id);
            }
            finally
            {
                await UpdateinboxMessage(dbConnection, dbTransaction, inboxMessage, exception);
            }
        }
        await dbTransaction.CommitAsync();
        logger.LogInformation("Completed processing the inbox message ");
    }
    private async Task<IReadOnlyCollection<InboxMessage>> GetinboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction dbTransaction)
    {
        string query =
        $"""
        SELECT
        id AS {nameof(InboxMessage.Id)},
        correlation_id as {nameof(InboxMessage.CorrelationId)},
        type AS {nameof(InboxMessage.Type)},
        content AS {nameof(InboxMessage.Content)},
        occurred_on_utc AS {nameof(InboxMessage.OccurredOnUtc)},
        processed_on_utc AS {nameof(InboxMessage.ProcessedOnUtc)},
        error AS Error
        FROM {Schema.Notifications}.inbox_messages
        WHERE processed_on_utc IS NULL
        ORDER BY "occurred_on_utc"
        FOR UPDATE
        LIMIT {options.Value.BatchSize}
        """;
        var inboxMessages = await connection.QueryAsync<InboxMessage>(query, new { }, dbTransaction);
        return [.. inboxMessages];
    }

    private async Task UpdateinboxMessage(
        IDbConnection connection,
        IDbTransaction dbTransaction,
        InboxMessage inboxMessage,
        Exception? exception
    )
    {
        const string query =
        $"""
        UPDATE 
            {Schema.Notifications}.inbox_messages
        SET
            processed_on_utc = @ProcessedOnUtc ,
            error = @Error
        WHERE id = @Id
        """;

        await connection.ExecuteAsync(query, new
        {
            ProcessedOnUtc = DateTime.UtcNow,
            Error = exception?.Message,
            inboxMessage.Id
        }, dbTransaction);
    }

}

