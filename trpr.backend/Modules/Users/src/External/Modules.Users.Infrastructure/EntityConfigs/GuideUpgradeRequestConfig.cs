using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs
{
    public class GuideUpgradeRequestConfig : IEntityTypeConfiguration<GuideUpgradeRequest>
    {
        public void Configure(EntityTypeBuilder<GuideUpgradeRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.userId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.RejectionReason)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.adminId)
                .IsRequired(false);

            builder.HasOne(x => x.user)
                .WithMany()
                .HasForeignKey(x => x.userId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Documents)
                .WithOne()
                .HasForeignKey(d => d.GuideRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
