namespace Modules.Users.Application.Dtos.Requests;

public class UpdateProfileNotificationSettingsDto
{
    public bool? TripUpdates { get; set; }
    public bool? Messages { get; set; }
    public bool? Promotions { get; set; }
}

public class UpdateProfileBulkRequestDto
{
    public string? Bio { get; set; }
    public List<int>? LanguageIds { get; set; }
    public List<int>? InterestIds { get; set; }
    public List<int>? VibeIds { get; set; }
    public UpdateProfileNotificationSettingsDto? NotificationSettings { get; set; }
}
