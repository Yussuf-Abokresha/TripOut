using Common.Application.Exceptions;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Mappers;

public static class TemplatePaginationMapper
{
    public static TemplatePaginationResponseDto ToPaginationResponseDto(this Template template)
    {

        return new TemplatePaginationResponseDto()
        {
            Id = template.Id,
            UserId = template.UserId,
            Active = template.Active,
            ContentType = template.ContentType,
            TemplateType = template.TemplateType,
            Content = template.TemplateLangs.Select(x => x.Content).First(),
            Title = template.TemplateLangs.Select(x => x.Title).First(),
        };
    }
}