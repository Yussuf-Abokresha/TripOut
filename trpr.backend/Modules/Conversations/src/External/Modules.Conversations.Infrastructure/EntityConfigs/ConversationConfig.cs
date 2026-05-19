using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Infrastructure.EntityConfigs;

public class ConversationConfig : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.CreateByUserId);

        builder.Property(c => c.Type)
            .IsRequired();

        builder.Property(c => c.Title);
        builder.Property(c => c.ImageUrl);

        builder
            .HasOne(c => c.CreateByUser)
            .WithMany()
            .HasForeignKey(c => c.CreateByUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
