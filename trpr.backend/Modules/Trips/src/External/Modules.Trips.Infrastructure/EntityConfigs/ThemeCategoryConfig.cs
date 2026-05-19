using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class ThemeCategoryConfig : IEntityTypeConfiguration<ThemeCategory>
{
    public void Configure(EntityTypeBuilder<ThemeCategory> builder)
    {
        builder.HasKey(tc => tc.Id);

        builder.HasIndex(tc => tc.ThemeId);
        builder.HasIndex(tc => tc.CategoryId);

        builder.Property(tc => tc.MaxLimit)
            .IsRequired();

        builder
            .HasOne<Theme>()
            .WithMany()
            .HasForeignKey(tc => tc.ThemeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(tc => tc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
