using Microsoft.Extensions.Primitives;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Responses;

public class TemplatePaginationResponseDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public bool Active { get; set; }
    public ContentType ContentType { get; set; }
    public TemplateType TemplateType { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
