using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Infrastructure.EntityConfigs;

public class ConversationParticipantConfig : IEntityTypeConfiguration<ConversationParticipant>
{
    public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
    {
        builder.HasKey(cp => cp.Id);

        builder.HasIndex(cp => cp.ConversationId);
        builder.HasIndex(cp => cp.UserId);

        builder.Property(cp => cp.JoinedAtUtc)
            .IsRequired();

        builder.Property(cp => cp.IsAdmin)
            .HasDefaultValue(false);

        builder.Property(cp => cp.IsArchived)
            .HasDefaultValue(false);

        builder
            .HasOne(cp => cp.Conversation)
            .WithMany(c => c.Participants)
            .HasForeignKey(cp => cp.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(cp => cp.User)
            .WithMany(u => u.ConversationParticipants)
            .HasForeignKey(cp => cp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
