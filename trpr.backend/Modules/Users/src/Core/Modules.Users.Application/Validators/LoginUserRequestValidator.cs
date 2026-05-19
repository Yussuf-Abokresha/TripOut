using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Modules.Users.Application.Dtos.Requests;

namespace Modules.Users.Application.Validators
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequestDto>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}