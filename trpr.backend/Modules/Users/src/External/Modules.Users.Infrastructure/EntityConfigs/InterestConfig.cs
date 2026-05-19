using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs;

public class InterestConfig : IEntityTypeConfiguration<Interest>
{
    public void Configure(EntityTypeBuilder<Interest> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .ValueGeneratedNever();
        builder.Property(i => i.Name).IsRequired();
        builder.Property(i => i.Icon).IsRequired();
        // reference data seeding
        builder.HasData(
            new Interest
            {
                Id = 1,
                Name = "History",
                Icon = "@/assets/interests/history.png",
            },
            new Interest
            {
                Id = 2,
                Name = "Adventure",
                Icon = "@/assets/interests/adventure.png",
            },
            new Interest
            {
                Id = 3,
                Name = "Nature",
                Icon = "@/assets/interests/nature.png",
            },
            new Interest
            {
                Id = 4,
                Name = "Culture",
                Icon = "@/assets/interests/culture.png",
            },
            new Interest
            {
                Id = 5,
                Name = "Foodie",
                Icon = "@/assets/interests/foodie.png",
            },
            new Interest
            {
                Id = 6,
                Name = "Wellness",
                Icon = "@/assets/interests/wellness.png",
            },
            new Interest
            {
                Id = 7,
                Name = "Romantic",
                Icon = "@/assets/interests/romantic.png",
            }
        );
    }
}
