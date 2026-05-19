using Modules.Users.Domain.Enums;

namespace Modules.Users.Application.Dtos.Requests
{
    public class AssignRolesRequestDto
    {
        public List<Role> Roles { get; set; } = [];
    }
}
