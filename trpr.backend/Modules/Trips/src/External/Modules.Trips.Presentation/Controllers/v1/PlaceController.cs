using Microsoft.AspNetCore.Mvc;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Services;

namespace Modules.Trips.Presentation.Controllers.v1;

[ApiController]
[Route("api/v1/places")]
public class PlaceController(PlaceService placeService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PlaceDto>> CreatePlace([FromBody] CreatePlaceRequestDto request, CancellationToken cancellationToken = default)
    {
        var place = await placeService.CreatePlaceAsync(request, cancellationToken);
        return Ok(place);
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<PlaceDto>>> GetPlaces([FromQuery] GetPlacesQueryDto query, CancellationToken cancellationToken = default)
    {
        var places = await placeService.GetPlacesAsync(query, cancellationToken);
        return Ok(places);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PlaceDto>> UpdatePlace(int id, [FromBody] UpdatePlaceRequestDto request, CancellationToken cancellationToken = default)
    {
        var place = await placeService.UpdatePlaceAsync(id, request, cancellationToken);
        return Ok(place);
    }

    [HttpGet("form-data")]
    public async Task<ActionResult<PlaceFormDataDto>> GetPlaceFormData(CancellationToken cancellationToken = default)
    {
        var formData = await placeService.GetPlaceFormDataAsync(cancellationToken);
        return Ok(formData);
    }
}
