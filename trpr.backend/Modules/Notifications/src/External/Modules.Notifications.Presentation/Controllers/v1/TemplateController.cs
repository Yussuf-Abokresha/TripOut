using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application.Dtos;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Application.Services;
using Modules.Notifications.Application.Validators;
using Modules.Notifications.Domain.Entities;
// TODO WHEN WE ADD THE COMPANY TEMPLATES WE WANT TO ADD VALIDATION TO CHECK IF THE TEMPLATE TYPE IS UNIQUE FOR THE COMPANY AND ALSO CHECK IF THE TEMPLATE TYPE IS UNIQUE FOR THE USER TEMPLATES
// AND CHECK IF THE TEMPLATE TYPE IS UNIQUE FOR THE ADMIN TEMPLATES
namespace Modules.Notifications.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/template")]
    public class TemplateController(TemplateService templateService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<ActionResult<TemplateResponseDto>> CreateAdminTemplate(CreateTemplateDto dto, CancellationToken cancellationToken)
        {
            var id = await templateService.CreateTemplate(null, dto, cancellationToken);
            return Ok(id);
        }

        [Authorize]
        [HttpPost("own")]
        public async Task<ActionResult<TemplateResponseDto>> CreateTemplate(CreateTemplateDto dto, CancellationToken cancellationToken)
        {
            var id = await templateService.CreateTemplate(UserId, dto, cancellationToken);
            return Ok(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/{id}")]
        public async Task<ActionResult<TemplateResponseDto>> UpdateAdminTemplate([FromRoute] Guid id, UpdateTemplateDto dto, CancellationToken cancellationToken)
        {
            var Id = await templateService.UpdateTemplate(id, null, dto, cancellationToken);
            return Ok(Id);
        }


        [Authorize]
        [HttpPut("own/{id}")]
        public async Task<ActionResult<TemplateResponseDto>> UpdateTemplate([FromRoute] Guid id, UpdateTemplateDto dto, CancellationToken cancellationToken)
        {
            var Id = await templateService.UpdateTemplate(id, UserId, dto, cancellationToken);
            return Ok(Id);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<ActionResult<PaginationDto<TemplatePaginationResponseDto>>> GetPaginatedTemplates([FromQuery] PaginateRequestDto dto, [FromHeader(Name = "X-Language")] string LangCode, CancellationToken cancellationToken = default)
        {
            var templatesList = await templateService.TemplatesPagination(dto, null, LangCode, cancellationToken);
            return Ok(templatesList);
        }

        [Authorize]
        [HttpGet("own")]
        public async Task<ActionResult<PaginationDto<TemplatePaginationResponseDto>>> GetOwnPaginatedTemplates([FromQuery] PaginateRequestDto dto, [FromHeader(Name = "X-Language")] string LangCode, CancellationToken cancellationToken = default)
        {
            var templatesList = await templateService.TemplatesPagination(dto, UserId, LangCode, cancellationToken);
            return Ok(templatesList);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("own/{id}")]
        public async Task<ActionResult<TemplateResponseDto>> GetTemplateDetails([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var template = await templateService.TemplateDetails(id, UserId, cancellationToken);
            return Ok(template);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{id}")]
        public async Task<ActionResult<TemplateResponseDto>> GetAdminTemplateDetails([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var template = await templateService.TemplateDetails(id, null, cancellationToken);
            return Ok(template);
        }

    }
}
