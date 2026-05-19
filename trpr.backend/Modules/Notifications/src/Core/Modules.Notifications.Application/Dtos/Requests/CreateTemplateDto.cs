using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Requests
{
    public class CreateTemplateDto
    {
        public ICollection<TemplateTranslationDto> Translations { get; set; } = [];
        public TemplateType TemplateType { get; set; }
        public ContentType ContentType { get; set; }
    }
}
