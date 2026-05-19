using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Modules.Notifications.Application.Dtos.Requests;

namespace Modules.Notifications.Application.Validators
{
    public class UpdateTemplateValidator : AbstractValidator<UpdateTemplateDto>
    {
        public UpdateTemplateValidator()
        {
            // Validate enums only if provided
            RuleFor(x => x.ContentType)
                .IsInEnum()
                .When(x => x.ContentType.HasValue)
                .WithMessage("Invalid ContentType value.");

            RuleFor(x => x.TemplateType)
                .IsInEnum()
                .When(x => x.TemplateType.HasValue)
                .WithMessage("Invalid TemplateType value.");

            RuleFor(x => x.Translations)
                .NotEmpty()
                .When(x => x.Translations is not null)
                .WithMessage("Translations cannot be empty if provided.");

            RuleFor(x => x.Translations)
                .NotEmpty()
                .Must(y => y != null && y.Any(z => z.LangCode == "en"))
                .WithMessage("At least one translation must be in English.");

            // Validate elements inside collection
            RuleForEach(x => x.Translations!).ChildRules(lang =>
            {
                lang.RuleFor(t => t.LangCode)
                    .Matches("^[a-z]{2}$")
                    .When(t => !string.IsNullOrWhiteSpace(t.LangCode));

                lang.RuleFor(t => t.Title)
                    .MaximumLength(50)
                    .When(t => !string.IsNullOrWhiteSpace(t.Title));

                lang.RuleFor(t => t.Content)
                    .MaximumLength(500)
                    .When(t => !string.IsNullOrWhiteSpace(t.Content));
            }).When(x => x.Translations is not null);
        }
    }
}
