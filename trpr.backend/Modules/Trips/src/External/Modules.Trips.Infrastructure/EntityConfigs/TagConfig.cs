using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class TagConfig : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        builder.HasIndex(t => t.Name)
            .IsUnique();
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasData(
            new Tag { Id = 1, Name = "cinema" },
            new Tag { Id = 2, Name = "theatre" },
            new Tag { Id = 3, Name = "live_shows" },
            new Tag { Id = 4, Name = "nightlife" },
            new Tag { Id = 5, Name = "concert_venue" },
            new Tag { Id = 6, Name = "festival" },
            new Tag { Id = 7, Name = "museum" },
            new Tag { Id = 8, Name = "historical_site" },
            new Tag { Id = 9, Name = "landmark" },
            new Tag { Id = 10, Name = "religious_site" },
            new Tag { Id = 11, Name = "art_gallery" },
            new Tag { Id = 12, Name = "restaurant" },
            new Tag { Id = 13, Name = "cafe" },
            new Tag { Id = 14, Name = "street_food" },
            new Tag { Id = 15, Name = "fine_dining" },
            new Tag { Id = 16, Name = "fast_food" },
            new Tag { Id = 17, Name = "rooftop" },
            new Tag { Id = 18, Name = "family_friendly" },
            new Tag { Id = 19, Name = "kid_friendly" },
            new Tag { Id = 20, Name = "group_activities" },
            new Tag { Id = 21, Name = "guided_tours" },
            new Tag { Id = 22, Name = "adventure" },
            new Tag { Id = 23, Name = "nature" },
            new Tag { Id = 24, Name = "park" },
            new Tag { Id = 25, Name = "beach" },
            new Tag { Id = 26, Name = "desert" },
            new Tag { Id = 27, Name = "river_view" },
            new Tag { Id = 28, Name = "spa" },
            new Tag { Id = 29, Name = "quiet_place" },
            new Tag { Id = 30, Name = "luxury" },
            new Tag { Id = 31, Name = "scenic" },
            new Tag { Id = 32, Name = "romantic" },
            new Tag { Id = 33, Name = "budget_friendly" },
            new Tag { Id = 34, Name = "tourist_hotspot" },
            new Tag { Id = 35, Name = "local_favorite" },
            new Tag { Id = 36, Name = "instagrammable" },
            new Tag { Id = 37, Name = "hidden_gem" },
            new Tag { Id = 38, Name = "accessible" }
            );
    }
}
