namespace Modules.Notifications.Contracts.Dtos;

public record SystemNotifyRequestDto(
    bool NotifyEmail,
    bool NotifyPhone,
    bool NotifySystem,
    string TemplateType,
    ICollection<Guid> ToUserIds,
    ICollection<string> ToEmails,
    ICollection<string> ToPhoneNumbers,
    IDictionary<string, string> KeyValuePairs,
    string LangCode = "en");
