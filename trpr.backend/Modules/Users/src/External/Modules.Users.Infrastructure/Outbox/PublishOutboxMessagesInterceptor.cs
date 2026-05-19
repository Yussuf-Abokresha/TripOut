using Common.Domain;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Common.Application.Correlation;
using Modules.Users.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Common.Infrastructure.Outbox;

namespace Modules.Users.Infrastructure.Outbox;

public class PublishOutboxMessagesInterceptor(
    ICorrelationIdAccessor correlationIdAccessor
) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {

        var correlationId = correlationIdAccessor.CorrelationId ?? Guid.Empty.ToString();
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context, correlationId);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    public static void InsertOutboxMessages(DbContext context, string correlationId)
    {
        var outBoxMessages = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage()
            {
                CorrelationId = correlationId,
                Id = domainEvent.Id,
                OccurredOnUtc = domainEvent.CreatedOnUtc,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            }).ToList();

        context.Set<OutboxMessage>().AddRange(outBoxMessages);
    }
}
