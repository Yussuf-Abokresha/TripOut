using Microsoft.AspNetCore.Http;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos
{
    public class DocumentDto
    {
        public DocumentType Type { get; set; }
        public string File { get; set; } = default!;
    }
}
