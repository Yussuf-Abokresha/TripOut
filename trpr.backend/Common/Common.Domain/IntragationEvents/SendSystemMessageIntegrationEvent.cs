namespace Common.Domain.IntragationEvents;

public class SendSystemMessageIntegrationEvent(
    bool NotifyEmail,
    bool NotifyPhone,
    bool NotifySystem,
    string TemplateType,
    ICollection<Guid> ToUserIds,
    IDictionary<string, string> keyValuePairs) : IntegrationEvent
{
    public ICollection<Guid> ToUserIds { get; set; } = ToUserIds;
    public bool NotifyEmail { get; set; } = NotifyEmail;
    public bool NotifyPhone { get; set; } = NotifyPhone;
    public bool NotifySystem { get; set; } = NotifySystem;
    public string TemplateType { get; set; } = TemplateType;
    public IDictionary<string, string> KeyValuePairs { get; set; } = keyValuePairs;
}