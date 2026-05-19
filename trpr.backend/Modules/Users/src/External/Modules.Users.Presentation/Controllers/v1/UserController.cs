using Common.Application.Dtos;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Services;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize(Roles = "Admin")]
    public class UserController(AdminUserService adminUserService) : ControllerBase
    {
        /// <summary>
        /// Get paginated list of users with optional search by username
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PaginationDto<UserResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationDto<UserResponseDto>>> GetUsers(
            [FromQuery] GetUsersRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await adminUserService.GetUsersAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get a user by ID including their profile if created
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetUser(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await adminUserService.GetUserByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Update a user's first and last name
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(
            [FromRoute] Guid id,
            [FromBody] UpdateUserRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await adminUserService.UpdateUserAsync(id, request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Delete a user by ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            await adminUserService.DeleteUserAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Assign (replace) roles for a user
        /// </summary>
        [HttpPut("{id:guid}/roles")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> AssignRoles(
            [FromRoute] Guid id,
            [FromBody] AssignRolesRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await adminUserService.AssignRolesAsync(id, request, cancellationToken);
            return Ok(result);
        }
    }
}
