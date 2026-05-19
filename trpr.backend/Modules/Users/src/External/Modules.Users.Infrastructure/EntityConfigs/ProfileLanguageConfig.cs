using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs;

public class ProfileLanguageConfig : IEntityTypeConfiguration<ProfileLanguage>
{
    public void Configure(EntityTypeBuilder<ProfileLanguage> builder)
    {
        builder.HasKey(pl => new { pl.ProfileId, pl.LanguageId });

        builder
            .HasOne(pl => pl.Profile)
            .WithMany(p => p.Languages)
            .HasForeignKey(pl => pl.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(pl => pl.Language)
            .WithMany()
            .HasForeignKey(pl => pl.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
