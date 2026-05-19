using System.ComponentModel.DataAnnotations;
using Modules.Notifications.Domain.Abstractions;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Domain.Entities;

public class Template : Entity
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public bool Active { get; set; }
    public ContentType ContentType { get; set; } = ContentType.Pure;
    public virtual User? User { get; set; }
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    public TemplateType TemplateType { get; set; }
    public virtual List<TemplateLang> TemplateLangs { get; set; } = [];
    public static Template Create(ContentType contentType, TemplateType templateType, Guid? userId, IDictionary<string, string> Contents, IDictionary<string, string> Titles)
    {
        var template = new Template
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ContentType = contentType,
            TemplateType = templateType,
        };
        var langCodes = Contents.Keys.ToHashSet();

        ICollection<TemplateLang> templatelangs = [];
        foreach (var langCode in langCodes)
        {
            var templateLang = new TemplateLang()
            {
                TemplateId = template.Id,
                Content = Contents[langCode],
                LangCode = langCode,
                Title = Titles[langCode]
            };
            templatelangs.Add(templateLang);
        }

        template.TemplateLangs.AddRange(templatelangs);
        return template;
    }
    public Template Update(
    ContentType? contentType,
    TemplateType? templateType,
    IDictionary<string, string>? contents,
    IDictionary<string, string>? titles,
    bool? active)
    {
        if (active == true)
            Active = true;

        if (contentType.HasValue)
            ContentType = contentType.Value;
        if (templateType.HasValue)
            TemplateType = templateType.Value;
        if (contents is null || titles is null)
            return this;

        var incomingLangCodes = contents.Keys.ToHashSet();

        var toRemove = TemplateLangs
            .Where(x => !incomingLangCodes.Contains(x.LangCode))
            .ToList();

        foreach (var lang in toRemove)
            TemplateLangs.Remove(lang);

        foreach (var langCode in incomingLangCodes)
        {
            var existing = TemplateLangs
                .FirstOrDefault(x => x.LangCode == langCode);

            if (existing is not null)
            {
                existing.Title = titles[langCode];
                existing.Content = contents[langCode];
            }
            else
            {
                TemplateLangs.Add(new TemplateLang
                {
                    TemplateId = Id,
                    LangCode = langCode,
                    Title = titles[langCode],
                    Content = contents[langCode]
                });
            }
        }

        return this;
    }

}
