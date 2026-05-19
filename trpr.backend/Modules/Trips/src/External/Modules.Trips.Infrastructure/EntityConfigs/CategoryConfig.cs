using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasKey(c => c.Id);
        builder
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasIndex(c => c.Name)
            .IsUnique();
        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new Category { Id = 1, Name = "food_drink" },
            new Category { Id = 2, Name = "entertainment" },
            new Category { Id = 3, Name = "culture" },
            new Category { Id = 4, Name = "activities" },
            new Category { Id = 5, Name = "outdoor" },
            new Category { Id = 6, Name = "relaxation" },
            new Category { Id = 7, Name = "other" }
        );
    }
}
