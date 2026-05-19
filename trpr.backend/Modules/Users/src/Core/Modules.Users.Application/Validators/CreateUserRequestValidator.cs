using FluentValidation;
using Modules.Users.Application.Dtos.Requests;

namespace Modules.Users.Application.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty()
            .WithMessage("Identifier is required.");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters.")
            .MinimumLength(2);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters.")
            .MinimumLength(2);
    }
}
