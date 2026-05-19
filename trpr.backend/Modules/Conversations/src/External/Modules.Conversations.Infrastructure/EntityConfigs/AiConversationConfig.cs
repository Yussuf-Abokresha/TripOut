using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Infrastructure.EntityConfigs;

public class AiConversationConfig : IEntityTypeConfiguration<AiConversation>
{
    public void Configure(EntityTypeBuilder<AiConversation> builder)
    {
        builder.HasKey(ac => ac.Id);

        builder.Property(ac => ac.Title);

        builder.Property(ac => ac.CreatedOnUtc)
            .IsRequired();

        builder
            .HasOne(ac => ac.User)
            .WithMany()
            .HasForeignKey(ac => ac.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
