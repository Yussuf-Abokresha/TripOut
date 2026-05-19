namespace Modules.Users.Infrastructure.Dtos.Requests;

internal sealed record CredentialRepresentation(string Type, string Value, bool Temporary);
