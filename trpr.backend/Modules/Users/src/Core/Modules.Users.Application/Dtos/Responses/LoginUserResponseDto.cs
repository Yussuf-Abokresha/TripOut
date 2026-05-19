using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Users.Application.Dtos.Responses
{
    public class LoginUserResponseDto
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public bool ProfileSetupCompleted { get; set; } = false;
    }
}