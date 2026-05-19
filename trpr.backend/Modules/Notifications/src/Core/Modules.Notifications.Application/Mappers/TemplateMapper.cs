using Common.Application.Exceptions;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Mappers;

public static class TemplateMapper
{
    public static TemplateResponseDto ToResponseDto(this Template template)
    {

        return new TemplateResponseDto()
        {
            Id = template.Id,
            UserId = template.UserId,
            Active = template.Active,
            Translations = template.TemplateLangs
                .Select(x => new TemplateTranslationDto()
                {
                    LangCode = x.LangCode,
                    Title = x.Title,
                    Content = x.Content
                })
                .ToList(),
            ContentType = template.ContentType,
            TemplateType = template.TemplateType,
            User = template.User?.ToResponseDto()
        };
    }
}
