using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos.Responses
{
    public class DocumentResponseDto
    {
        public DocumentType DocumentType { get; set; }
        public string Document { get; set; } = string.Empty;
    }
}
