using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Responses;

public class TemplateResponseDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public bool Active { get; set; }
    public ContentType ContentType { get; set; }
    public TemplateType TemplateType { get; set; }
    public ICollection<TemplateTranslationDto> Translations { get; set; } = [];
    public NotificationUserResponseDto? User { get; set; } = default!;
}
