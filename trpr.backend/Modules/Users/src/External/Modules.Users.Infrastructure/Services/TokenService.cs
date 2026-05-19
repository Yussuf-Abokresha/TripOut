using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Options;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.Services;

internal sealed class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
{
    private readonly string _secret = jwtOptions.Value.Secret;

    public LoginUserResponseDto GenerateToken(User user)
    {
        var token = GenerateJwtToken(user);
        return new LoginUserResponseDto()
        {
            AccessToken = token,
            RefreshToken = token,
            ProfileSetupCompleted = user.Profile != null
        };
    }

    public LoginUserResponseDto RefreshToken(string token)
    {
        return new LoginUserResponseDto()
        {
            AccessToken = token,
            RefreshToken = token,
            ProfileSetupCompleted = false
        };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(_secret);

        var claims = new Dictionary<string, object>
        {
            { "given_name", user.FirstName },
            { "family_name", user.LastName },
            { "identifier", user.UserName },
            { "sub", user.Id.ToString() },
            { "realm_access", new Dictionary<string, object> { { "roles", user.UserRoles.Select(ur => ur.Role.ToString()).ToArray() } } }
        };
        if (user.Profile != null)
        {
            if (user.Profile.Email != null)
                claims.Add("email", user.Profile.Email);
            if (user.Profile.PhoneNumber != null)
                claims.Add("phone_number", user.Profile.PhoneNumber);
            claims.Add("profile_id", user.Profile.Id.ToString());
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "trpr-backend",
            Audience = "trpr-clients"
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
