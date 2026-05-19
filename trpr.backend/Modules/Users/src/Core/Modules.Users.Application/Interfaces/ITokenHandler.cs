using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Interfaces;

public interface ITokenHandler
{
    Task<LoginUserResponseDto> VerifyOtpAsync(Token token, User user, string value, CancellationToken cancellationToken = default);
}
