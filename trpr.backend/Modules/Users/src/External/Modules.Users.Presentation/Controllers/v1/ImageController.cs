using System;
using System.Collections.Generic;
using System.Text;
using Common.Application.Buckets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Route("api/v1/user/images")]
    public class ImageController(IFileService fileService) : ControllerBase
    {
        [HttpPost("upload-images")]
        public async Task<ActionResult<ICollection<string>>> UploadImages([FromForm] ICollection<IFormFile> request)
        {
            var paths = await fileService.UploadFilesAsync(request);
            return Ok(paths);
        }
    }
}
