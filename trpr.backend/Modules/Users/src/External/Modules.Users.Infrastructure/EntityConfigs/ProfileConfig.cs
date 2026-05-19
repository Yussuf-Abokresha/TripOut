using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs
{
    public class ProfileConfig : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(x => x.Id);
            builder
                .HasOne(x => x.User)
                .WithOne(x => x.Profile)
                .HasForeignKey<Profile>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}