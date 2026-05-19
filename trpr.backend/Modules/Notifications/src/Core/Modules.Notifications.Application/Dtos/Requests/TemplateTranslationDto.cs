namespace Modules.Notifications.Application.Dtos.Requests
{
    public class TemplateTranslationDto
    {
        public string LangCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
