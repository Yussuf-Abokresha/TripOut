namespace Common.Domain;

public interface IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public string CorrelationId { get; set; }

}
