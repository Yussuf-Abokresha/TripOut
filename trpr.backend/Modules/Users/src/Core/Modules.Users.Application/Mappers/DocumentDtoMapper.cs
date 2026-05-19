using System;
using System.Collections.Generic;
using System.Text;
using Common.Application;
using Common.Application.Buckets;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Modules.Users.Application.Dtos;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Mappers
{
    public class DocumenDtoMapper : IMapper<ICollection<DocumentDto>, Dictionary<string, DocumentType>>
    {
        public Dictionary<string, DocumentType> Map(ICollection<DocumentDto> source)
        {
            Dictionary<string, DocumentType> docs = [];
            foreach (DocumentDto document in source) 
            {
                docs.Add(document.File, document.Type);
            }
            return docs;
        }
    }
}
