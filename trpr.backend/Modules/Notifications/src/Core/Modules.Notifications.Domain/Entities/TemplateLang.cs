using Modules.Notifications.Domain.Abstractions;

namespace Modules.Notifications.Domain.Entities;

public class TemplateLang : Entity
{
    public Guid TemplateId { get; set; }
    public string LangCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public virtual Template Template { get; set; } = default!;
}
