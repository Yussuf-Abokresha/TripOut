using System.Text.Json;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Presentation.Hubs;

[Authorize]
public class ChatHub(RepositoryFactory repositoryFactory) : Hub
{
    public Guid UserId => Context.User!.GetUserId();
    public async override Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("UserConnected", $"user id : {UserId}");
        var conversations = (await repositoryFactory
            .Repository<ConversationParticipant>()
            .GetByExpWhereAsync(x => x.UserId == UserId))
            .Select(x => x.ConversationId.ToString())
            .Distinct();

        foreach (var conversationId in conversations)
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

        await base.OnConnectedAsync();
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("UserDisconnected", $"user id : {UserId}");
        await base.OnDisconnectedAsync(exception);
    }

    public Task Typing(Guid conversationId)
    {
        return Clients
            .Group(conversationId.ToString())
            .SendAsync("UserTyping", UserId);
    }

    public Task StopTyping(Guid conversationId)
    {
        return Clients
            .Group(conversationId.ToString())
            .SendAsync("UserStopTyping", UserId);
    }
    public Task ReadMessages(Guid conversationId, Guid lastReadMessageId)
    {
        return Clients
            .Group(conversationId.ToString())
            .SendAsync("UserReadMessages", UserId, lastReadMessageId);
    }

}

