namespace Modules.Users.Application.Dtos.Responses;

public class LanguageResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string NativeName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public class InterestResponseDto
{
    public int Id { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class VibeResponseDto
{
    public int Id { get; set; }
    public string Thumbnail { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class ProfileNotificationSettingsDto
{
    public bool TripUpdates { get; set; }
    public bool Messages { get; set; }
    public bool Promotions { get; set; }
}

public class ProfileResponseDto
{
    public Guid Id { get; set; }
    public string Bio { get; set; } = string.Empty;
    public List<LanguageResponseDto> Languages { get; set; } = new();
    public List<InterestResponseDto> Interests { get; set; } = new();
    public List<VibeResponseDto> Vibes { get; set; } = new();
    public ProfileNotificationSettingsDto? NotificationSettings { get; set; }
}

public class ProfileLookupResponseDto
{
    public List<LanguageResponseDto> Languages { get; set; } = new();
    public List<InterestResponseDto> Interests { get; set; } = new();
    public List<VibeResponseDto> Vibes { get; set; } = new();
}