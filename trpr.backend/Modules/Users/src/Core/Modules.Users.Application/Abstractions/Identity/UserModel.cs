namespace Modules.Users.Application.Abstractions.Identity;

public sealed record UserModel(string UserName, string Email, string Password, string FirstName, string LastName);
