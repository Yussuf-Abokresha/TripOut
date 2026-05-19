namespace Common.Domain.IntragationEvents;

public class SendMessageIntegrationEvent(
    Guid FromUserId,
    bool NotifyEmail,
    bool NotifyPhone,
    bool NotifySystem,
    string TemplateType,
    ICollection<Guid> ToUserIds,
    IDictionary<string, string> KeyValuePairs) : IntegrationEvent
{
    public Guid FromUserId { get; set; } = FromUserId;
    public ICollection<Guid> ToUserIds { get; set; } = ToUserIds;
    public string TemplateType { get; set; } = TemplateType;
    public bool NotifyEmail { get; set; } = NotifyEmail;
    public bool NotifyPhone { get; set; } = NotifyPhone;
    public bool NotifySystem { get; set; } = NotifySystem;
    public IDictionary<string, string> KeyValuePairs { get; set; } = KeyValuePairs;
}
