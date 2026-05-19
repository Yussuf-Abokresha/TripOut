using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Services
{
    public class UserService(INotificationsDbContext notificationDbContext, ILogger<UserService> logger, IUnitOfWork unitOfWork)
    {
        public async Task CreateUser(Guid userId, string userName, string firstName, string lastName, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating user with username : {userName} and id : {userId}", userName, userId);

            User? user = await notificationDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);
            if (user != null)
            {
                logger.LogInformation("User with username : {userName} already exist skipped creating the user", userName);
                return;
            }
            user = User.Create(userId, userName, firstName, lastName);
            notificationDbContext.Users.Add(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Created user with username : {userName} and id : {userId}", userName, userId);
        }
    }
}