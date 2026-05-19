using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class GovernorateConfig : IEntityTypeConfiguration<Governorate>
{
    public void Configure(EntityTypeBuilder<Governorate> builder)
    {
        builder
            .HasKey(g => g.Id);
        builder
            .Property(g => g.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasIndex(g => g.Name)
            .IsUnique();
        builder
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new Governorate { Id = 1, Name = "Aswan" },
            new Governorate { Id = 2, Name = "Asyut" },
            new Governorate { Id = 3, Name = "Luxor" },
            new Governorate { Id = 4, Name = "Alexandria" },
            new Governorate { Id = 5, Name = "Ismailia" },
            new Governorate { Id = 6, Name = "Suez" },
            new Governorate { Id = 7, Name = "Dakahlia" },
            new Governorate { Id = 8, Name = "Faiyum" },
            new Governorate { Id = 9, Name = "Cairo" },
            new Governorate { Id = 10, Name = "Giza" },
            new Governorate { Id = 11, Name = "Beheira" },
            new Governorate { Id = 12, Name = "Sharqia" },
            new Governorate { Id = 13, Name = "Gharbia" },
            new Governorate { Id = 14, Name = "Qalyubia" },
            new Governorate { Id = 15, Name = "Monufia" },
            new Governorate { Id = 16, Name = "Minya" },
            new Governorate { Id = 17, Name = "New Valley" },
            new Governorate { Id = 18, Name = "Beni Suef" },
            new Governorate { Id = 19, Name = "Port Said" },
            new Governorate { Id = 20, Name = "South Sinai" },
            new Governorate { Id = 21, Name = "Damietta" },
            new Governorate { Id = 22, Name = "Sohag" },
            new Governorate { Id = 23, Name = "North Sinai" },
            new Governorate { Id = 24, Name = "Qena" },
            new Governorate { Id = 25, Name = "Kafr El Sheikh" },
            new Governorate { Id = 26, Name = "Matruh" },
            new Governorate { Id = 27, Name = "Red Sea" }
        );
    }
}
