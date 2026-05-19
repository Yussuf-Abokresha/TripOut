using Common.Application.Exceptions;
using Common.Application.IntegrationEvents;
using Common.Domain.IntragationEvents;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Presentation.IntegrationEventHandlers;

public class UserCreatedIntegrationEventHandler(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork) : IntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async override Task HandleAsync(UserCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository
            .GetFirstOrDefaultByFilter(x =>
                x.Id == integrationEvent.UserId);
        if (user != null)
            throw new ConflictException("User.Conflict");
        user = new User
        {
            Id = integrationEvent.UserId,
            FirstName = integrationEvent.FirstName,
            LastName = integrationEvent.LastName,
            Identifier = integrationEvent.UserName,
            AvatarUrl = null
        };
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

}
