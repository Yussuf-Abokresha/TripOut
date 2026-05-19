namespace Common.Domain;

public interface IDomainEvent
{
    public Guid Id { get; }
    public DateTime CreatedOnUtc { get; }
}
