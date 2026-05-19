using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Application.Dtos;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Mappers
{
    public static class DocumentResponseMapper
    {
        public static DocumentDto ToResponseDto(this Document document)
        {
            return new DocumentDto
            {
                File = document.FileUrl,
                Type = document.Type,
            };
        }
    }
}
