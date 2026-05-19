namespace Modules.Users.Application.Dtos.Requests;

public class CreateProfileRequestDto
{
    public string Bio { get; set; } = string.Empty;
    public List<int> LanguageIds { get; set; } = [];
    public List<int> InterestIds { get; set; } = [];
    public List<int> VibeIds { get; set; } = [];
}
