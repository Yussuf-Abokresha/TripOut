namespace Common.Infrastructure.Inbox;

public class InboxConsumerMessage
{
    public Guid Id { get; set; }
    public string HandlerName { get; set; } = string.Empty;
}
