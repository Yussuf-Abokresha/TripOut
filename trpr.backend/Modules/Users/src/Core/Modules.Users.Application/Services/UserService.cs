using Modules.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;
using Modules.Users.Domain.Entities;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Repositories;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Factories;
using Modules.Users.Domain.ValueObjects;
using System.Net.Mail;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Modules.Users.Application.Services;

public class UserService(
IRepository<User> userRepository,
IRepository<Token> tokenRepository,
TokenFactory tokenFactory,
ITokenService tokenService,
OtpHandlerFactory otpHandlerFactory,
ILogger<UserService> logger,
IUnitOfWork unitOfWork)
{
    public async Task<OtpResponseDto> CreateUserAsync(CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetFirstOrDefaultByFilter(x => x.UserName == createUserRequestDto.Identifier);
        if (user != null && user.IsVerified)
        {
            logger.LogWarning("User creation failed. Identifier already exists: {Identifier}", user.Id);
            throw new ConflictException("User.Conflict", user.Id);
        }
        await RemoveUnVerifiedUserAsync(user, cancellationToken);
        var emailAdress = MailAddress.TryCreate(createUserRequestDto.Identifier, out var _) ? createUserRequestDto.Identifier : string.Format("{0}@trpr.com", createUserRequestDto.Identifier);

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserRequestDto.Password);

        user = User.Create(
                createUserRequestDto.Identifier,
                createUserRequestDto.FirstName,
                createUserRequestDto.LastName,
                passwordHash);
        userRepository.Add(user);
        var token = tokenFactory.CreateToken(TokenType.Otp, user);
        tokenRepository.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new OtpResponseDto { OtpId = token.Id };
    }
    public async Task<OtpResponseDto> ForgetPasswordAsync(ForgetPasswordRequestIdentifierDto forgetPasswordRequestIdentifierDto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetFirstOrDefaultByFilter(x => x.UserName == forgetPasswordRequestIdentifierDto.Identifier) ?? throw new NotFoundException("User.NotFound", forgetPasswordRequestIdentifierDto.Identifier);
        var token = tokenFactory.CreateToken(TokenType.ForgetPasswordOtp, user);
        tokenRepository.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new OtpResponseDto { OtpId = token.Id };
    }

    public async Task<LoginUserResponseDto> LoginUserAsync(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken = default)
    {

        var user = await userRepository.GetFirstOrDefaultByFilter(x => x.UserName == loginUserRequestDto.Identifier && x.IsVerified, x => x.Include(x => x.UserRoles)) ?? throw new NotFoundException("User.NotFound", loginUserRequestDto.Identifier);

        if (!BCrypt.Net.BCrypt.Verify(loginUserRequestDto.Password, user.PasswordHash))
        {
            throw new NotAuthorizedException("Invalid.Creds");
        }

        var loginResponse = tokenService.GenerateToken(user);
        loginResponse.ProfileSetupCompleted = user.Profile != null;
        return loginResponse;
    }

    public Task<LoginUserResponseDto> RefreshUserAsnc(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(tokenService.RefreshToken(refreshTokenRequestDto.Token));
    }

    public async Task UpdatePassword(Guid userId, UpdatePasswordRequestDto updatePasswordRequest, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetFirstOrDefaultByFilter(x => x.Id == userId) ?? throw new NotFoundException("User.NotFound", userId);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatePasswordRequest.Password);
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<LoginUserResponseDto> VerifyOtpAsync(VerifyOtpRequestDto verifyOtpRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            var otpToken = await tokenRepository.GetByIdForUpdate(verifyOtpRequestDto.Identifier);
            if (
                otpToken == null ||
                otpToken.Expiration < DateTime.UtcNow ||
                otpToken.IsRevoked)
            {
                logger.LogWarning(
                    "OTP verification failed. Invalid or expired OTP ID: {OtpId}",
                    verifyOtpRequestDto.Identifier);
                throw new BadRequestException("Otp.Invalid", verifyOtpRequestDto.Identifier);
            }
            User user =
                await userRepository.GetFirstOrDefaultByFilter(x => x.Id == otpToken.UserId) ??
                throw new NotFoundException("User.NotFound", otpToken.UserId);
            await unitOfWork
                .SaveChangesAsync(cancellationToken);
            var otpHandler = otpHandlerFactory
                .CreateOtpHandler(otpToken.Type);
            var loginResponse = await otpHandler
                .VerifyOtpAsync(otpToken, user, verifyOtpRequestDto.Value, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return loginResponse;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private async Task RemoveUnVerifiedUserAsync(User? user, CancellationToken cancellationToken)
    {
        if (user == null || user.IsVerified) return;
        logger.LogInformation("User with identifier {Identifier} exists but is not verified. Proceeding to re-register.", user.Id);
        userRepository.Delete(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

}
