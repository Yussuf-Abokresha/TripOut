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
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            
            builder.HasMany(t => t.CreatedTrips)
                .WithOne(t => t.CreatedByUser)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.JoinedTrips)
                .WithOne(tp => tp.User)
                .HasForeignKey(tp => tp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
