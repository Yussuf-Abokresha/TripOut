using Modules.Conversations.Domain.Abstractions;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Domain.Entities
{
    public class Message : Entity
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; } = default!;
        public Guid? SenderUserId { get; set; }
        public virtual User? SenderUser { get; set; } = default!;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;
        public MessageType Type { get; set; } = MessageType.Text;
        public ICollection<MessageAttachment> Attachments { get; set; } = [];
    }
}