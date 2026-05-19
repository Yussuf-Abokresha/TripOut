using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs
{
    public class DayConfiguration : IEntityTypeConfiguration<Day>
    {
        public void Configure(EntityTypeBuilder<Day> builder)
        {
            builder.HasKey(d => d.Id);


            builder.Property(d => d.TripId)
                .IsRequired();

            // Many-to-Many: Day -> Places
            builder.HasMany(d => d.Places)
                .WithMany(p => p.Days)
                .UsingEntity(j => j.ToTable("place_day"));

            builder.HasIndex(d => d.TripId);

        }
    }
}
