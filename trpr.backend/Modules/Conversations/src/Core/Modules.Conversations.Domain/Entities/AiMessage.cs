using Modules.Conversations.Domain.Abstractions;

namespace Modules.Conversations.Domain.Entities;

public class AiMessage : Entity
{
    public string Id { get; set; } = string.Empty;
    public string? ParentMessageId { get; set; }
    public virtual AiMessage? ParentAiMessage { get; set; } = default!;
    public virtual ICollection<AiMessage> SubMessages { get; set; } = [];
    public Guid ConversationId { get; set; }
    public virtual AiConversation AiConversation { get; set; } = default!;
    public string Contnet { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
    public static AiMessage Create(AiConversation conversation, string content, ICollection<string> ChildContents)
    {
        var parentMessage = new AiMessage()
        {
            Id = Ulid.NewUlid().ToString(),
            ConversationId = conversation.Id,
            AiConversation = conversation,
            Contnet = content,
            ParentMessageId = null,
            ParentAiMessage = null,
            SubMessages = [],
            CreatedOnUtc = DateTime.UtcNow
        };

        foreach (var childContent in ChildContents)
        {
            var childMessage = AiMessage
                .Create(
                    conversation,
                    childContent,
                    parentMessage);
            parentMessage.SubMessages.Add(childMessage);
        }

        return parentMessage;
    }

    private static AiMessage Create(AiConversation conversation, string content, AiMessage parentMessage)
    {
        return new AiMessage()
        {
            Id = Ulid.NewUlid().ToString(),
            ConversationId = conversation.Id,
            AiConversation = conversation,
            Contnet = content,
            ParentMessageId = parentMessage.Id,
            ParentAiMessage = parentMessage,
            SubMessages = [],
            CreatedOnUtc = DateTime.UtcNow
        };
    }
}

