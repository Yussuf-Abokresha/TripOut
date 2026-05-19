using Modules.Conversations.Domain.Abstractions;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Domain.Entities
{
    public class Conversation : Entity
    {
        public Guid Id { get; set; }
        public ConversationType Type { get; set; }
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }
        public Guid CreateByUserId { get; set; }
        public virtual User CreateByUser { get; set; } = default!;
        public Guid? LastMessageId { get; set; }
        public virtual ICollection<ConversationParticipant> Participants { get; set; } = [];
    }
}