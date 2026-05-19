using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Modules.Users.Application.Dtos.Requests;

namespace Modules.Users.Application.Validators
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
        }
    }
}