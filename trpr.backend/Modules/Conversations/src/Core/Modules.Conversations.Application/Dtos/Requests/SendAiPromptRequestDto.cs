namespace Modules.Conversations.Application.Dtos.Requests;

public class SendAiPromptRequestDto
{
    public Guid? ConversationId { get; set; }
    public string Prompt { get; set; } = string.Empty;
}
