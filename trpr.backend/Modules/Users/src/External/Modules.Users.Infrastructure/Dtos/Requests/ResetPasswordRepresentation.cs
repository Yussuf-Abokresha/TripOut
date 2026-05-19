namespace Modules.Users.Infrastructure.Dtos.Requests;

internal sealed record ResetPasswordRepresentation(
    string Type,
    bool Temporary,
    string Value
);