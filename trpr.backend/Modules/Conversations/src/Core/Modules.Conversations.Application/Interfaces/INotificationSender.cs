using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Application.Interfaces;

public interface INotificationSender
{
    public Task SendMessageAsync(Message message, CancellationToken cancellationToken = default);
}
