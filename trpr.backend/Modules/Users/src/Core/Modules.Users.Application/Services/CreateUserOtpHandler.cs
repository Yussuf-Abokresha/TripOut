using Common.Application.Exceptions;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Repositories;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Services;

public class CreateUserOtpHandler(IRepository<User> userRepository, IRepository<Token> tokenRepository, IUnitOfWork unitOfWork, ITokenService tokenService) : ITokenHandler
{
    public async Task<LoginUserResponseDto> VerifyOtpAsync(Token token, User user, string value, CancellationToken cancellationToken = default)
    {
        if (token.Value != value)
            throw new BadRequestException("Otp.Invalid", token.Id);

        token.IsRevoked = true;
        user.IsVerified = true;
        userRepository.Update(user);
        tokenRepository.Update(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return tokenService.GenerateToken(user);
    }

}
