using Modules.Users.Domain.Abstractions;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Domain.Entities;

public class Token : Entity
{
    public Guid Id { get; set; }
    public bool IsRevoked { get; set; } = false;
    public TokenType Type { get; set; } = TokenType.None;
    public string Value { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
}
