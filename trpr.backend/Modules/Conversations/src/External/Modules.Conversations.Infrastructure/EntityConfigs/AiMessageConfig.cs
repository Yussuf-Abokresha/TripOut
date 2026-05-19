using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Infrastructure.EntityConfigs;

public class AiMessageConfig : IEntityTypeConfiguration<AiMessage>
{
    public void Configure(EntityTypeBuilder<AiMessage> builder)
    {
        builder.HasKey(am => am.Id);

        builder.Property(am => am.Contnet)
            .IsRequired();

        builder.Property(am => am.CreatedOnUtc)
            .IsRequired();

        builder.HasIndex(am => am.ConversationId);
        builder.HasIndex(am => am.ParentMessageId);

        builder
            .HasOne(am => am.AiConversation)
            .WithMany(ac => ac.Messages)
            .HasForeignKey(am => am.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(am => am.ParentAiMessage)
            .WithMany(pm => pm.SubMessages)
            .HasForeignKey(am => am.ParentMessageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
