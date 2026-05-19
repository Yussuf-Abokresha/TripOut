using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Infrastructure.EntityConfigs;

public class MessageAttachmentConfig : IEntityTypeConfiguration<MessageAttachment>
{
    public void Configure(EntityTypeBuilder<MessageAttachment> builder)
    {
        builder.HasKey(ma => ma.Id);

        builder.HasIndex(ma => ma.MessageId);

        builder.Property(ma => ma.Url)
            .IsRequired();

        builder.Property(ma => ma.AttachmentName)
            .IsRequired();

        builder
            .HasOne(ma => ma.Message)
            .WithMany(m => m.Attachments)
            .HasForeignKey(ma => ma.MessageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
