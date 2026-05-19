using Modules.Conversations.Domain.Abstractions;

namespace Modules.Conversations.Domain.Entities
{
    public class User : Entity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public virtual ICollection<ConversationParticipant> ConversationParticipants { get; set; } = [];
    }
}