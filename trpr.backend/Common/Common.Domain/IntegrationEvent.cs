namespace Common.Domain;

public class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedOnUtc { get; init; } = DateTime.UtcNow;
    public string CorrelationId { get; set; } = Guid.Empty.ToString();
    public IntegrationEvent()
    {

    }
    public IntegrationEvent(Guid Id, DateTime CreatedOnUtc)
    {
        this.Id = Id;
        this.CreatedOnUtc = CreatedOnUtc;
    }
}