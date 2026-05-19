using System.Text.Json;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Services;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ProfileController(ProfileManagementService profileManagementService, UserService userService, ILogger<ProfileController> logger) : ControllerBase
    {
        public Guid UserId => User.GetUserId();
        /// <summary>
        /// Create a new profile for a user with languages, interests, and vibes
        /// </summary>
        [HttpPost("me")]
        public async Task<ActionResult<ProfileResponseDto>> CreateProfile(
            [FromBody] CreateProfileRequestDto createRequest,
            CancellationToken cancellationToken)
        {
            var profile = await profileManagementService.CreateProfileAsync(UserId, createRequest, cancellationToken);
            return Ok(profile);
        }

        /// <summary>
        /// Get user profile with all languages, interests, and vibes
        /// </summary>
        [HttpGet("me")]
        public async Task<ActionResult<ProfileResponseDto>> GetProfile(CancellationToken cancellationToken)
        {
            var profile = await profileManagementService.GetProfileByUserIdAsync(UserId, cancellationToken);
            return Ok(profile);
        }

        /// <summary>
        /// Update profile with languages, interests, and vibes in bulk
        /// </summary>
        [HttpPut("me")]
        public async Task<ActionResult<ProfileResponseDto>> UpdateProfile(
            [FromBody] UpdateProfileBulkRequestDto updateRequest,
            CancellationToken cancellationToken)
        {
            var updatedProfile = await profileManagementService.UpdateProfileAsync(UserId, updateRequest, cancellationToken);
            return Ok(updatedProfile);
        }

        /// <summary>
        /// Update password endpoint
        /// </summary>
        /// <param name="request">Update password</param>
        [Authorize]
        [HttpPut("me/password-reset")]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordRequestDto request)
        {
            await userService.UpdatePassword(UserId, request);
            return NoContent();
        }
        /// <summary>
        /// Get profile form data for dropdowns and selections (languages, interests, vibes)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("form-data")]
        public async Task<ActionResult<ProfileLookupResponseDto>> GetProfileFormData(CancellationToken cancellationToken)
        {
            logger.LogInformation("log headers {0}", JsonSerializer.Serialize(Request.Headers));
            var formData = await profileManagementService.GetProfileMetaDataAsync(cancellationToken);
            return Ok(formData);
        }

    }
}
