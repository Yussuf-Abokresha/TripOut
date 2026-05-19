using Microsoft.AspNetCore.Http;


namespace Modules.Users.Application.Dtos.Requests
{
    public class UploadFileDto
    {
        public ICollection<IFormFile> Files { get; set; } = default!;
    }
}
