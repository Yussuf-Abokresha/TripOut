namespace Common.Infrastructure.Outbox;

public class OutboxConsumerMessage
{
    public Guid Id { get; set; }
    public string HandlerName { get; set; } = string.Empty;
}
