using System;
using System.Collections.Generic;
using System.Text;
using Common.Application.Buckets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Modules.Trips.Presentation.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Route("api/v1/trip/images")]
    public class ImageController(IFileService fileService) : ControllerBase
    {
        [HttpPost("upload-images")]
        public async Task<ActionResult<ICollection<string>>> UploadImage([FromForm] ICollection<IFormFile> request)
        {
            var paths = await fileService.UploadFilesAsync(request);
            return Ok(paths);
        }
    }
}
