using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Application.Buckets
{
    public interface IFileService
    {
        public Task<string> UploadFileAsync(IFormFile file);
        public Task<ICollection<string>> UploadFilesAsync(ICollection<IFormFile> files);
        public string ResolveUrl(string filePath);
        public ICollection<string> ResolveUrls(ICollection<string> filePaths);
    }
}