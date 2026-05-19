using Microsoft.Extensions.Options;
using Modules.Users.Application.Options;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Factories;

public class TokenFactory(IOptionsMonitor<TokenExpirationInMinutesOption> tokenExpirationOptions)
{
    public Token CreateToken(TokenType type, User user)
    {
        var token = new Token
        {
            IsRevoked = false,
            Id = Guid.NewGuid(),
            Type = type,
            Value = GenerateTokenValue(type),
            Expiration = GetExpirationTime(type),
            UserId = user.Id,
            User = user
        };
        token.RaiseDomainEvent(new TokenCreatedDomainEvent(token.Id));
        return token;
    }
    private DateTime GetExpirationTime(TokenType type = TokenType.Otp)
    {
        return type switch
        {
            TokenType.Otp =>
                DateTime.UtcNow.AddMinutes(tokenExpirationOptions.CurrentValue.Otp),
            TokenType.ForgetPasswordOtp =>
                DateTime.UtcNow.AddMinutes(tokenExpirationOptions.CurrentValue.ForgetPasswordOtp),
            _ => DateTime.UtcNow.AddMinutes(tokenExpirationOptions.CurrentValue.Otp),
        };
    }
    private static string GenerateTokenValue(TokenType type = TokenType.Otp)
    {
        return type switch
        {
            TokenType.Otp => GenerateRandomDigits(6),
            TokenType.ForgetPasswordOtp => GenerateRandomDigits(6),
            _ => GenerateRandomDigits(6),
        };
    }

    private static string GenerateRandomDigits(int length)
    {
        var random = new Random();
        var digits = new char[length];
        for (int i = 0; i < length; i++)
            digits[i] = (char)('0' + random.Next(0, 10));
        return new string(digits);
    }
}
