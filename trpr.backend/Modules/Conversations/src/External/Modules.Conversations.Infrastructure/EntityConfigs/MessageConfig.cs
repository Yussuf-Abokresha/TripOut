using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Infrastructure.EntityConfigs;

public class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasIndex(m => m.ConversationId);
        builder.HasIndex(m => m.SenderUserId);

        builder.Property(m => m.Content)
            .IsRequired();

        builder.Property(m => m.SentAtUtc)
            .IsRequired();

        builder.Property(m => m.Type)
            .IsRequired();

        builder
            .HasOne(m => m.Conversation)
            .WithMany()
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(m => m.SenderUser)
            .WithMany()
            .HasForeignKey(m => m.SenderUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
