using Microsoft.AspNetCore.SignalR;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Domain.Entities;
using Modules.Conversations.Presentation.Hubs;

namespace Modules.Conversations.Infrastructure.Services;

public class NotificationSender(IHubContext<ChatHub> hubContext) : INotificationSender
{
    public async Task SendMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        await hubContext.Clients.Group(message.ConversationId.ToString()).SendAsync("ReceiveMessage", new
        {
            MessageId = message.Id,
            ConversationId = message.ConversationId,
            SenderUserId = message.SenderUserId,
            Content = message.Content,
            SentAtUtc = message.SentAtUtc,
            Type = message.Type,
            Attachments = message.Attachments.Select(a => new
            {
                type = a.AttachmentType.ToString(),
                a.Id,
                a.Url,
                a.AttachmentName
            }).ToList()
        }, cancellationToken);
    }

}
