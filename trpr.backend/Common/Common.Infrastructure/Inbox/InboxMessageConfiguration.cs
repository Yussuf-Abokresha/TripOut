using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infrastructure.Inbox;

public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("inbox_messages");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Content);
    }
}
