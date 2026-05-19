using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Modules.Notifications.Application.Dtos.Requests;

namespace Modules.Notifications.Application.Validators
{
    public class CreateTemplateDtoValidator : AbstractValidator<CreateTemplateDto>
    {
        public CreateTemplateDtoValidator()
        {
            RuleFor(x => x.ContentType)
                .IsInEnum()
                .WithMessage("Invalid content Type.");

            RuleFor(x => x.TemplateType)
                .IsInEnum()
                .WithMessage("Invalid Template Type.");

            RuleFor(x => x.Translations)
                .Must(x => x.Any(z => z.LangCode == "en"))
                .WithMessage("At least one translation must be in English.");

            RuleForEach(x => x.Translations).ChildRules(lang =>
            {
                lang.RuleFor(l => l.LangCode)
                    .Matches("^[a-z]{2}$")
                    .Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("Language is required.");

                lang.RuleFor(l => l.Title)
                    .MaximumLength(50)
                    .Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("Title is required.");

                lang.RuleFor(l => l.Content)
                    .MaximumLength(500)
                    .Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("Content is required.");
            });
        }
    }

}
