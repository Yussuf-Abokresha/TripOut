using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class ThemeTagConfig : IEntityTypeConfiguration<ThemeTag>
{
    public void Configure(EntityTypeBuilder<ThemeTag> builder)
    {
        builder.HasKey(tt => tt.Id);

        builder.HasIndex(tt => tt.ThemeId);
        builder.HasIndex(tt => tt.TagId);

        builder.Property(tt => tt.Score)
            .IsRequired();

        builder
            .HasOne<Theme>()
            .WithMany()
            .HasForeignKey(tt => tt.ThemeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Tag>()
            .WithMany()
            .HasForeignKey(tt => tt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
