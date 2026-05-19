namespace Modules.Users.Application.Dtos.Responses;

public class ChatResponseDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "SENT";
    public DateTime CreatedOnUtc { get; set; }
}
