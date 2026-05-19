using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Infrastructure.EntityConfigs;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
