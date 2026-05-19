using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class ThemeConfig : IEntityTypeConfiguration<Theme>
{
    public void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new Theme
            {
                Id = 1,
                Name = "History",
            },
            new Theme
            {
                Id = 2,
                Name = "Adventure",
            },
            new Theme
            {
                Id = 3,
                Name = "Nature",
            },
            new Theme
            {
                Id = 4,
                Name = "Culture",
            },
            new Theme
            {
                Id = 5,
                Name = "Foodie",
            },
            new Theme
            {
                Id = 6,
                Name = "Wellness",
            },
            new Theme
            {
                Id = 7,
                Name = "Romantic",

            });
    }
}
