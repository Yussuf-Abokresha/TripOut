using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Requests
{
    public class UpdateTemplateDto
    {
        public ContentType? ContentType { get; set; }
        public ICollection<TemplateTranslationDto>? Translations { get; set; }
        public TemplateType? TemplateType { get; set; }
        public bool? Active { get; set; }
    }
}