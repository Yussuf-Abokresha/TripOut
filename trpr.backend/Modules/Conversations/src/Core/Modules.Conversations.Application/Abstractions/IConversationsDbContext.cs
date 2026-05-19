using Microsoft.EntityFrameworkCore;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Application.Abstractions;

public interface IConversationsDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageAttachment> MessageAttachments { get; set; }
}
