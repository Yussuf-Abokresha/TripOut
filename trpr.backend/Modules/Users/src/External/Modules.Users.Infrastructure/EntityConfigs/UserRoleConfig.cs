using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs;

public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => new { ur.UserId, ur.Role });

        builder.Property(ur => ur.Role)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();
    }
}
