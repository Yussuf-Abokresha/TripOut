using Modules.Users.Domain.Enums;

namespace Modules.Users.Domain.Entities;

public class UserRole
{
    public Guid UserId { get; set; }
    public Role Role { get; set; }
    public virtual User User { get; set; } = default!;
}
