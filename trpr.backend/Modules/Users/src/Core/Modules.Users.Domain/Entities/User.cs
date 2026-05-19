using Modules.Users.Domain.Events;
using Modules.Users.Domain.Abstractions;
using Modules.Users.Domain.Enums;

namespace Modules.Users.Domain.Entities;

public class User : Entity
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool TwoFactorEnabled { get; set; } = false;
    public bool IsVerified { get; set; } = false;
    public virtual Profile Profile { get; set; } = default!;
    public virtual ICollection<Token> Tokens { get; set; } = [];
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    public static User Create(string userName, string firstName, string lastName, string passwordHash)
    {
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            UserName = userName,
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = passwordHash,
            TwoFactorEnabled = false,
            IsVerified = false,
        };
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }
}
