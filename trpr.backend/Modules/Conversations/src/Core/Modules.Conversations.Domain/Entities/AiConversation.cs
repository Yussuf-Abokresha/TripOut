using System.Runtime.InteropServices;
using Modules.Conversations.Domain.Abstractions;

namespace Modules.Conversations.Domain.Entities;

public class AiConversation : Entity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public DateTime CreatedOnUtc { get; set; }
    public virtual ICollection<AiMessage> Messages { get; set; } = [];

    public static AiConversation Create(Guid UserId)
    {
        return new AiConversation
        {
            Id = Guid.NewGuid(),
            Title = null,
            UserId = UserId,
            CreatedOnUtc = DateTime.UtcNow
        };
    }
}

