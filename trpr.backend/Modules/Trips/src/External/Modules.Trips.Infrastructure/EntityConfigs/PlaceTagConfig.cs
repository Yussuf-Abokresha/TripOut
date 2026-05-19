using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class PlaceTagConfig : IEntityTypeConfiguration<PlaceTag>
{
    public void Configure(EntityTypeBuilder<PlaceTag> builder)
    {
        builder.HasKey(pt => new { pt.PlaceId, pt.TagId });

        builder
            .HasOne(x => x.Place)
            .WithMany(x => x.PlaceTags)
            .HasForeignKey(pt => pt.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Tag)
            .WithMany(x => x.PlaceTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
