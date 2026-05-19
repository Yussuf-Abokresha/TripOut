using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Infrastructure.EntityConfigs;

public class TemplateConfig : IEntityTypeConfiguration<Template>
{
    public void Configure(EntityTypeBuilder<Template> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Active);
        builder.HasMany(x => x.TemplateLangs)
               .WithOne(x => x.Template)
               .HasForeignKey(x => x.TemplateId);
        builder.Property(p => p.RowVersion).IsRowVersion();
        builder.HasData(
            new Template
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                UserId = null,
                Active = true,
                ContentType = Domain.ValueObjects.ContentType.Pure,
                TemplateType = Domain.ValueObjects.TemplateType.ApprovalMessage,
            },
            new Template
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                UserId = null,
                Active = true,
                ContentType = Domain.ValueObjects.ContentType.Pure,
                TemplateType = Domain.ValueObjects.TemplateType.RejectionMessage,
            },
            new Template
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                UserId = null,
                Active = true,
                ContentType = Domain.ValueObjects.ContentType.Pure,
                TemplateType = Domain.ValueObjects.TemplateType.OtpMessage,
            },
            new Template
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                UserId = null,
                Active = true,
                ContentType = Domain.ValueObjects.ContentType.Pure,
                TemplateType = Domain.ValueObjects.TemplateType.ForgetPasswordMessage,
            }
        );
    }
}
