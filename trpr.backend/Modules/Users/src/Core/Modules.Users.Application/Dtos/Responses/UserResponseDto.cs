using Modules.Users.Domain.Enums;

namespace Modules.Users.Application.Dtos.Responses
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public List<string> Roles { get; set; } = [];
        public ProfileResponseDto? Profile { get; set; }
    }
}