using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Application.Dtos.Requests;

public class GetRelayMessageRequestDto
{
    public ICollection<LastConversationMessageDto> LastConversationsMessages { get; set; } = [];
}

public class LastConversationMessageDto
{
    public Guid ConversationId { get; set; }
    public Guid LastMessageId { get; set; }
    public DateTime LastSentAtUtc { get; set; }
}

public class SendMessageRequestDto
{
    public string MessageContent { get; set; } = string.Empty;
    public Guid RecipientId { get; set; }
}