using System.Dynamic;
using Common.Domain;

namespace Common.Domain.IntragationEvents;

public class UserCreatedIntegrationEvent(Guid Id, DateTime CreatedOnUtc, Guid UserId, string UserName, string FirstName, string LastName) : IntegrationEvent(Id, CreatedOnUtc)
{
    public Guid UserId { get; set; } = UserId;
    public string UserName { get; set; } = UserName;
    public string FirstName { get; set; } = FirstName;
    public string LastName { get; set; } = LastName;
}
