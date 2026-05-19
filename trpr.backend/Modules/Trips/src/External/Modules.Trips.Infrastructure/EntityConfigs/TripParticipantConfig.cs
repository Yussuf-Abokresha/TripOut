using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs
{
    public class TripParticipantConfig : IEntityTypeConfiguration<TripParticipant>
    {
        public void Configure(EntityTypeBuilder<TripParticipant> builder)
        {
            builder.HasKey(tp => new { tp.TripId, tp.UserId });
        }
    }
}
