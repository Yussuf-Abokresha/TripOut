using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos.Requests
{
    public class UploadDocumentDto
    {
        public DocumentType Type { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
