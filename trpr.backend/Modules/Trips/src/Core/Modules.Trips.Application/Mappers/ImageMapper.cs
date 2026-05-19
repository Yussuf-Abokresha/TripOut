using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application;
using Common.Application.Buckets;
using Microsoft.AspNetCore.Http;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Mappers
{
    public class ImageMapper(IFileService fileService) : IMapper<ICollection<IFormFile>, Task<ICollection<string>>>
    {
        public async Task<ICollection<string>> Map(ICollection<IFormFile> source)
        {
            ICollection<string> paths = [];
            foreach(var img in source)
            {
                paths.Add(await fileService.UploadFileAsync(img));
            }
            return paths;
        }
    }
}
