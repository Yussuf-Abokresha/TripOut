using Common.Application.Dtos;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Services;

namespace Modules.Trips.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/trip")]
    public class TripController(TripService tripService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [HttpPost("create-trip")]
        [Authorize]
        public async Task<ActionResult<TripResponseDto>> CreateTrip([FromBody] CreateTripRequestDto dto, CancellationToken cancellationToken)
        {
            var request = await tripService.CreateTrip(dto, UserId, cancellationToken);
            return Ok(request);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginationDto<TripResponseDto>>> GetTrips([FromQuery] SearchTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = await tripService.GetTrips(requestDto, null, cancellationToken);
            return Ok(request);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<PaginationDto<TripResponseDto>>> GetMyTrips([FromQuery] SearchTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = await tripService.GetTrips(requestDto, UserId, cancellationToken);
            return Ok(request);
        }

        [HttpPost("change-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ApproveTrip(UpdateTripStatusRequestDto dto, CancellationToken cancellationToken)
        {
            await tripService.UpdateStatus(dto, cancellationToken);
            return NoContent();
        }
    }
}
