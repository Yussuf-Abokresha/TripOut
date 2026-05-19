using System;
using System.Collections.Generic;
using System.Text;
using Common.Application;
using Common.Application.Buckets;
using Modules.Users.Application.Dtos;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Mappers
{
    public class DocumentMapper(IFileService fileService) : IMapper<ICollection<Document>, ICollection<DocumentDto>>
    {
        public ICollection<DocumentDto> Map(ICollection<Document> source)
        {
            ICollection<DocumentDto> docs = [];
            foreach (var item in source)
            {
                docs.Add(new DocumentDto
                {
                    Type = item.Type,
                    File = fileService.ResolveUrl(item.FileUrl)
                });
            }
            return docs;
        }
    }
}
