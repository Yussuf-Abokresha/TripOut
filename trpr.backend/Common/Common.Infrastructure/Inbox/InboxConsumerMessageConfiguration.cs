using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infrastructure.Inbox;

public class InboxConsumerMessageConfiguration : IEntityTypeConfiguration<InboxConsumerMessage>
{
    public void Configure(EntityTypeBuilder<InboxConsumerMessage> builder)
    {
        builder.ToTable("inbox_consumer_messages");
        builder.HasKey(msg => new { msg.Id, msg.HandlerName });
    }
}
