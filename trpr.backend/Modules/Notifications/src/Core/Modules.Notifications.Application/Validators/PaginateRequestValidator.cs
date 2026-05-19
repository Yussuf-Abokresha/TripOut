using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Modules.Notifications.Application.Dtos.Requests;

namespace Modules.Notifications.Application.Validators
{
    public class PaginateRequestValidator : AbstractValidator<PaginateRequestDto>
    {
        public PaginateRequestValidator()
        {
            RuleFor(x => x.Search)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search cannot exceed 200 characters.");

            // Validate enums only if provided
            RuleFor(x => x.sortBy)
                .IsInEnum()
                .When(x => x.sortBy.HasValue)
                .WithMessage("Invalid sortBy value.");

            RuleFor(x => x.TemplateType)
                .IsInEnum()
                .When(x => x.TemplateType.HasValue)
                .WithMessage("Invalid TemplateType value.");
        }
    }
}
