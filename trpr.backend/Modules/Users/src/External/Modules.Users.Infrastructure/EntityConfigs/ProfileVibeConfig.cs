using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs;

public class ProfileVibeConfig : IEntityTypeConfiguration<ProfileVibe>
{
    public void Configure(EntityTypeBuilder<ProfileVibe> builder)
    {
        builder.HasKey(pv => new { pv.ProfileId, pv.VibeId });

        builder
            .HasOne(pv => pv.Profile)
            .WithMany(p => p.Vibes)
            .HasForeignKey(pv => pv.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(pv => pv.Vibe)
            .WithMany()
            .HasForeignKey(pv => pv.VibeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
