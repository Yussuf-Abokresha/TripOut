using Modules.Trips.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Modules.Trips.Infrastructure.EntityConfigs
{
    public class TripGovernorateConfig : IEntityTypeConfiguration<TripGovernorate>
    {
        public void Configure(EntityTypeBuilder<TripGovernorate> builder)
        {
            builder.HasKey(tg => new { tg.TripId, tg.GovernorateId });

            builder.HasOne(tg => tg.Trip)
                .WithMany(t => t.TripGovernorates)
                .HasForeignKey(tg => tg.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tg => tg.Governorate)
                .WithMany(g => g.TripGovernorates)
                .HasForeignKey(tg => tg.GovernorateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
