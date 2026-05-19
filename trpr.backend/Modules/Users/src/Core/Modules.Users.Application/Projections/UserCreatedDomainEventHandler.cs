using Microsoft.Extensions.Logging;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.Entities;
using Common.Application.DomainEvents;
using Common.Application.EventBus;
using Common.Domain.IntragationEvents;
using Modules.Users.Application.Repositories;

namespace Modules.Users.Application.Projections;

public class UserCreatedDomainEventHandler(IRepository<User> userRepository, ILogger<UserCreatedDomainEventHandler> logger, IEventBus bus) : IDomainEventHandler<UserCreatedDomainEvent>
{
    public async Task HandleAsync(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetFirstOrDefaultByFilter(x => x.Id == domainEvent.UserId);
        if (user == null)
        {
            logger.LogError("User with ID {UserId} not found for UserCreatedDomainEvent", domainEvent.UserId);
            throw new InvalidOperationException($"User with ID {domainEvent.UserId} not found.");
        }
        logger.LogInformation("User created with ID: {UserId}, Identifier: {Identifier}", domainEvent.UserId, user.UserName);
        await bus.PublishAsync<UserCreatedIntegrationEvent>(new UserCreatedIntegrationEvent(domainEvent.Id, domainEvent.CreatedOnUtc, user.Id, user.UserName, user.FirstName, user.LastName), cancellationToken);
    }
}
