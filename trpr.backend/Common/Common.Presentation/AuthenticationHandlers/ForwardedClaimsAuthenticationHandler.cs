using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Common.Presentation.AuthenticationHandlers;

public class ForwardedClaimsAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public ForwardedClaimsAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue("X-User-Id", out var userIdValue);
        if (string.IsNullOrEmpty(userIdValue))
        {
            return Task.FromResult(AuthenticateResult.Fail("X-User-Id header is missing"));
        }
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, userIdValue.ToString()));
        Request.Headers.TryGetValue("X-User-Role", out var rolesValue);
        if (!string.IsNullOrEmpty(rolesValue))
        {
            string[] deserialized = JsonSerializer.Deserialize<string[]>(rolesValue.ToString()) ?? Array.Empty<string>();

            foreach (var role in deserialized)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }
        }
        var identity = new ClaimsIdentity(claims, SchemaDefaults.ForwardedClaimsSchema);
        var principal = new ClaimsPrincipal(identity);
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, SchemaDefaults.ForwardedClaimsSchema)));
    }
}
