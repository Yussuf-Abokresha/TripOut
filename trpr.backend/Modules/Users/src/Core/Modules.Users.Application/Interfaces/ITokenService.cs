using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Interfaces;

public interface ITokenService
{
    LoginUserResponseDto GenerateToken(User user);
    LoginUserResponseDto RefreshToken(string token);
}
