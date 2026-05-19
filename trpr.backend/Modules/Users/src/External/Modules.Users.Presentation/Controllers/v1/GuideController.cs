using Common.Application.Buckets;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Services;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/guide")]
    public class GuideController(GuideService GuideService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();
        [Authorize]
        [HttpPost("upgrade-request")]
        public async Task<ActionResult<GuideUpgradeResponseDto>> UpgradeRequest([FromBody] GuideUpgradeRequestDto dto, CancellationToken cancellationToken)
        {
            var roles = User.GetRoles();
            foreach (var role in roles)
            {
                if (role == "Guide")
                {
                    throw new NotAuthorizedException("User.AlreadyGuide", UserId);
                }
            }
            ActionResult<GuideUpgradeResponseDto> request = await GuideService.UpgradeToGuide(UserId, dto, cancellationToken);
            return Ok(request);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("requests")]
        public async Task<ActionResult<PaginationDto<UpgradePaginationResponseDto>>> GetUpgradeRequests([FromQuery] UpgradePaginationRequestDto dto, CancellationToken cancellationToken)
        {
            var requests = await GuideService.AllUpgradeRequests(dto, cancellationToken);
            return Ok(requests);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("change-status")]
        public async Task<ActionResult<GuideUpgradeResponseDto>> UpdateGuideStatus(UpdateGuideStatusRequestDto dto, CancellationToken cancellationToken)
        {
            var request = await GuideService.ChangeGuideStatus(UserId, dto, cancellationToken);
            return Ok(request);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<GuideUpgradeResponseDto>>> GetUserRequests([FromRoute(Name = "id")] Guid upgradeRequestId, CancellationToken cancellationToken)
        {
            var request = await GuideService.UserUpgradeRequests(upgradeRequestId, cancellationToken);
            return Ok(request);
        }
    }
}