using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs;

public class ProfileInterestConfig : IEntityTypeConfiguration<ProfileInterest>
{
    public void Configure(EntityTypeBuilder<ProfileInterest> builder)
    {
        builder.HasKey(pi => new { pi.ProfileId, pi.InterestId });

        builder
            .HasOne(pi => pi.Profile)
            .WithMany(p => p.Interests)
            .HasForeignKey(pi => pi.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(pi => pi.Interest)
            .WithMany()
            .HasForeignKey(pi => pi.InterestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
