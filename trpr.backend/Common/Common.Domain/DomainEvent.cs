namespace Common.Domain;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        CreatedOnUtc = DateTime.UtcNow;
    }
    protected DomainEvent(Guid id, DateTime CreatedOn)
    {
        Id = id;
        CreatedOnUtc = CreatedOn;
    }
}
