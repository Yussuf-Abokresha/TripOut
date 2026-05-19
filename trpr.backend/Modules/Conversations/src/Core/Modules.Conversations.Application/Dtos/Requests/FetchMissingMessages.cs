namespace Modules.Conversations.Application.Dtos.Requests;

public class FetchMissingMessagesRequestDto
{
    public ICollection<MessageRequestDto> Messages { get; set; } = [];
}
public class MessageRequestDto
{
    public Guid ConversationId { get; set; }
    public Guid? LastMessageId { get; set; }
}