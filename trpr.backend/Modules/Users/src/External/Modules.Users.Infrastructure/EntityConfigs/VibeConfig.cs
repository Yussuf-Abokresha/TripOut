using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs;

public class VibeConfig : IEntityTypeConfiguration<Vibe>
{
    public void Configure(EntityTypeBuilder<Vibe> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .ValueGeneratedNever();
        builder.Property(v => v.Thumbnail).IsRequired();
        builder.Property(v => v.Description).IsRequired();
        builder.Property(v => v.Name).IsRequired();
        // reference data seeding
        builder.HasData(
            new Vibe
            {
                Id = 1,
                Name = "Solo",
                Description = "Self Discovery and Personal Growth.",
                Thumbnail = "@/assets/vibes/solo.png",
            },
            new Vibe
            {
                Id = 2,
                Name = "Friends",
                Description = "Night out with friends and socializing.",
                Thumbnail = "@/assets/vibes/friends.png",
            },
            new Vibe
            {
                Id = 3,
                Name = "Romantic",
                Description = "Intimate and romantic setting.",
                Thumbnail = "@/assets/vibes/romantic.png",
            },
            new Vibe
            {
                Id = 4,
                Name = "Family",
                Description = "Kid friendly and family-oriented atmosphere.",
                Thumbnail = "@/assets/vibes/family.png",
            }
        );
    }
}
