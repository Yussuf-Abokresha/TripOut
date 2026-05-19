using Modules.Trips.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Modules.Trips.Infrastructure.EntityConfigs
{
    public class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {

            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.ThemeId)
                .IsRequired();

            builder.Property(t => t.Title)
                .IsRequired();

            builder.Property(t => t.Description)
                .IsRequired();

            builder.HasIndex(x => x.Title);

            builder.HasIndex(x => x.Price);

            builder.Property(t => t.Price)
                .HasPrecision(10, 2);

            builder.Property(t => t.ExpectedDuration)
                .IsRequired();

            builder.Property(t => t.MaxParticipantsCount)
                .IsRequired();

            builder.Property(t => t.TripVisibility)
                .IsRequired();

            builder.HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedTrips)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Trip -> Days
            builder.HasMany(t => t.Segments)
                .WithOne(d => d.Trip)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Trip -> TripParticipants
            builder.HasMany(t => t.Participants)
                .WithOne(tp => tp.Trip)
                .HasForeignKey(tp => tp.TripId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(t => t.UserId);
            builder.HasIndex(t => t.ThemeId);

        }
    }

}
