using System.Security.Cryptography.X509Certificates;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Domain.Entities;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Application.Services;

public class ChatService(
    RepositoryFactory repositoryFactory,
    INotificationSender notificationSender,
    IUnitOfWork unitOfWork)
{
    // todo : add attachment support later
    public async Task StartDirectMessage(Guid userId, SendMessageRequestDto request, CancellationToken cancellationToken = default)
    {
        // Check if the recipient user exists
        var recipientUser = await repositoryFactory.Repository<User>()
            .GetFirstOrDefaultByFilter(u => u.Id == request.RecipientId)
                ?? throw new NotFoundException("User.NotFound");

        // check if conversation already exists between the two users
        var existingConversation = await repositoryFactory.Repository<Conversation>()
            .GetQueryable()
            .Include(c => c.Participants)
            .Where(c =>
                c.Type == ConversationType.Direct &&
                c.Participants.Any(p => p.UserId == userId) &&
                c.Participants.Any(p => p.UserId == request.RecipientId))
            .FirstOrDefaultAsync(cancellationToken);

        if (existingConversation != null)
        {
            // If a conversation already exists, use it
            request.RecipientId = existingConversation.Id;
            await SendMessage(userId, request, cancellationToken);
            return;
        }

        // Create a new conversation
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Type = ConversationType.Direct,
            CreateByUserId = userId,
        };

        repositoryFactory.Repository<Conversation>().Add(conversation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Add participants to the conversation
        var participants = new List<ConversationParticipant>
        {
            new() {
                ConversationId = conversation.Id,
                Conversation = conversation,
                UserId = userId,
            },
            new() {
                ConversationId = conversation.Id,
                Conversation = conversation,
                UserId = request.RecipientId,
            }
        };

        foreach (var participant in participants)
            repositoryFactory.Repository<ConversationParticipant>().Add(participant);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Set the recipient ID to the conversation ID for sending the first message
        request.RecipientId = conversation.Id;
        // Send the first message
        await SendMessage(userId, request, cancellationToken);
    }
    public async Task SendMessage(Guid userId, SendMessageRequestDto request, CancellationToken cancellationToken = default)
    {
        // Check if the conversation exists and the user is a participant
        var conversation = await repositoryFactory.Repository<Conversation>()
            .GetQueryable().Where(x =>
                x.Id == request.RecipientId &&
                (x.Type == ConversationType.Direct || x.Type == ConversationType.Group))
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Conversation.NotFound");

        // Check if the user is a participant of the conversation
        var participant = conversation.Participants.FirstOrDefault(p => p.UserId == userId)
            ?? throw new NotFoundException("Conversation.ParticipantNotFound");

        var messageEntity = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            Conversation = conversation,
            SenderUserId = userId,
            Content = request.MessageContent,
        };
        // Save the message to the database
        repositoryFactory.Repository<Message>().Add(messageEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        // Send the message to the recipients via SignalR
        await notificationSender.SendMessageAsync(messageEntity, cancellationToken);
    }

    public async Task<ICollection<Message>> GetMessagesAsync(Guid conversationId, Guid? messageId, DateTime? lastSentAt, bool older = true, CancellationToken cancellationToken = default)
    {
        var messagesQuery = repositoryFactory.Repository<Message>()
            .GetQueryable()
            .Where(m => m.ConversationId == conversationId);

        if (lastSentAt.HasValue && messageId.HasValue)
            if (older)
                messagesQuery = messagesQuery
                    .Where(m =>
                            m.SentAtUtc < lastSentAt.Value || (m.SentAtUtc == lastSentAt.Value && m.Id < messageId.Value));
            else messagesQuery = messagesQuery
                    .Where(m =>
                            m.SentAtUtc > lastSentAt.Value || (m.SentAtUtc == lastSentAt.Value && m.Id > messageId.Value));

        var messages = await messagesQuery
            .OrderByDescending(m => m.SentAtUtc)
            .Take(50)
            .ToListAsync(cancellationToken);

        return messages;
    }

    public async Task<ICollection<Message>> GetRelayMessagesAsync(ICollection<LastConversationMessageDto> lastConversationsMessages, CancellationToken cancellationToken = default)
    {
        var messages = new List<Message>();

        foreach (var convoMsg in lastConversationsMessages)
        {
            var convoMessagesQuery = repositoryFactory.Repository<Message>()
                .GetQueryable()
                .Where(m => m.ConversationId == convoMsg.ConversationId);

            if (convoMsg.LastSentAtUtc != default && convoMsg.LastMessageId != default)
            {
                convoMessagesQuery = convoMessagesQuery
                    .Where(m =>
                        m.SentAtUtc > convoMsg.LastSentAtUtc ||
                        (m.SentAtUtc == convoMsg.LastSentAtUtc && m.Id > convoMsg.LastMessageId));
            }

            var convoMessages = await convoMessagesQuery
                .OrderBy(m => m.SentAtUtc)
                .ToListAsync(cancellationToken);

            messages.AddRange(convoMessages);
        }

        return messages;
    }

    public async Task<ICollection<Conversation>> GetUserConversationsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var conversations = await repositoryFactory.Repository<Conversation>()
            .GetQueryable()
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .Where(c => c.Type == ConversationType.Direct || c.Type == ConversationType.Group)
            .Include(c => c.Participants)
            .ToListAsync(cancellationToken);

        return conversations;
    }

    // TODO : FINSIH LATER
    public async Task<Dictionary<Conversation, ICollection<Message>>>
    GetRelayAsync(Guid userId, FetchMissingMessagesRequestDto request, CancellationToken cancellationToken = default)
    {
        ICollection<Conversation> conversations = await repositoryFactory.Repository<ConversationParticipant>()
            .GetQueryable()
            .Where(x => x.UserId == userId)
            .Include(x => x.Conversation)
            .Select(x => x.Conversation)
            .DistinctBy(x => x.Id)
            .ToListAsync(cancellationToken);
        Dictionary<Guid, Guid?> ConvLastMessageIds = request.Messages.ToDictionary(x => x.ConversationId, x => x.LastMessageId);
        Dictionary<Conversation, ICollection<Message>> result = new();
        foreach (Conversation conversation in conversations)
        {
            Guid? lastMessageId = ConvLastMessageIds.ContainsKey(conversation.Id) ? ConvLastMessageIds[conversation.Id] : null;
            var messagesQuery = repositoryFactory.Repository<Message>()
                .GetQueryable()
                .Where(m => m.ConversationId == conversation.Id);

            if (lastMessageId.HasValue)
            {
                messagesQuery = messagesQuery.Where(m => m.Id > lastMessageId.Value);
            }

            var messages = await messagesQuery
                .OrderBy(m => m.SentAtUtc)
                .ToListAsync(cancellationToken);

            result[conversation] = messages;
        }
        return result;
    }
}
