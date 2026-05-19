using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Infrastructure.EntityConfigs;

public class TemplateLangConfig : IEntityTypeConfiguration<TemplateLang>
{
    public void Configure(EntityTypeBuilder<TemplateLang> builder)
    {
        builder.HasKey(x => new { x.TemplateId, x.LangCode });
        builder.HasData(
            new TemplateLang
            {
                TemplateId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                LangCode = "en",
                Title = "Approval Message",
                Content = "Your request has been approved."
            },
            new TemplateLang
            {
                TemplateId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                LangCode = "en",
                Title = "Rejection Message",
                Content = "Your request has been rejected. Rejection reason: {{reason}}"
            },
            new TemplateLang
            {
                TemplateId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                LangCode = "en",
                Title = "OTP Message",
                Content = "Your OTP code is: {{code}}"
            },
            new TemplateLang
            {
                TemplateId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                LangCode = "en",
                Title = "Forget Password OTP Message",
                Content = "Your OTP code is: {{code}}"
            }
        );
    }
}