using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs;

public class LanguageConfig : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .ValueGeneratedNever();
        builder.Property(l => l.Name).IsRequired();
        builder.Property(l => l.Code).IsRequired();
        builder.Property(l => l.NativeName).IsRequired();
        builder.Property(l => l.Icon).IsRequired();
        // reference data seeding
        builder.HasData(
            new Language
            {
                Id = 1,
                Name = "English",
                Code = "en",
                NativeName = "English",
                Icon = "@/assets/languages/english.png",
            },
            new Language
            {
                Id = 2,
                Name = "Spanish",
                Code = "es",
                NativeName = "Español",
                Icon = "@/assets/languages/spanish.png",
            },
            new Language
            {
                Id = 3,
                Name = "French",
                Code = "fr",
                NativeName = "Français",
                Icon = "@/assets/languages/french.png",
            },
            new Language
            {
                Id = 4,
                Name = "German",
                Code = "de",
                NativeName = "Deutsch",
                Icon = "@/assets/languages/german.png",
            },
            new Language
            {
                Id = 5,
                Name = "Chinese",
                Code = "zh",
                NativeName = "中文",
                Icon = "@/assets/languages/chinese.png",
            },
            new Language
            {
                Id = 6,
                Name = "Japanese",
                Code = "ja",
                NativeName = "日本語",
                Icon = "@/assets/languages/japanese.png",
            },
            new Language
            {
                Id = 7,
                Name = "Korean",
                Code = "ko",
                NativeName = "한국어",
                Icon = "@/assets/languages/korean.png",
            },
            new Language
            {
                Id = 8,
                Name = "Portuguese",
                Code = "pt",
                NativeName = "Português",
                Icon = "@/assets/languages/portuguese.png",
            },
            new Language
            {
                Id = 9,
                Name = "Russian",
                Code = "ru",
                NativeName = "Русский",
                Icon = "@/assets/languages/russian.png",
            },
            new Language
            {
                Id = 10,
                Name = "Arabic",
                Code = "ar",
                NativeName = "العربية",
                Icon = "@/assets/languages/arabic.png",
            }
        );
    }
}
