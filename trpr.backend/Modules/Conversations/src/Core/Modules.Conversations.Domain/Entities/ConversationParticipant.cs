using Modules.Conversations.Domain.Abstractions;

namespace Modules.Conversations.Domain.Entities
{
    public class ConversationParticipant : Entity
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; } = default!;
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = default!;
        public DateTime JoinedAtUtc { get; set; } = DateTime.UtcNow;
        public bool IsAdmin { get; set; } = false;
        public bool IsArchived { get; set; } = false;
    }
}