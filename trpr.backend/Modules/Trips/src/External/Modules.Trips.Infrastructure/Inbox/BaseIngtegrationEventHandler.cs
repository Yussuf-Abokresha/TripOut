using Common.Domain;
using Common.Infrastructure.Inbox;
using Modules.Trips.Infrastructure.Data;
using Newtonsoft.Json;
using Rebus.Handlers;

namespace Modules.Trips.Infrastructure.Inbox;

public class BaseIngtegrationEventHandler<T>(TripsDbContext context) : IHandleMessages<T> where T : IntegrationEvent
{
    public async Task Handle(T message)
    {
        var inBoxMessages = new InboxMessage()
        {
            CorrelationId = message.CorrelationId,
            Id = message.Id,
            OccurredOnUtc = message.CreatedOnUtc,
            Type = message.GetType().Name,
            Content = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            })
        };

        context.Set<InboxMessage>().Add(inBoxMessages);
        await context.SaveChangesAsync();
    }

}
