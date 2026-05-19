using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Mappers;

public static class PlaceMapper
{
    public static PlaceDto ToPlaceDto(this Place place)
    {
        return new PlaceDto
        {
            Id = place.Id,
            Title = place.Title,
            Description = place.Description,
            CategoryId = place.CategoryId,
            GovernorateId = place.GovernorateId,
            Latitude = place.Location.Y,
            Longitude = place.Location.X,
            Governorate = new GovernorateDto
            {
                Id = place.Governorate.Id,
                Name = place.Governorate.Name
            },
            Category = new CategoryDto
            {
                Id = place.Category.Id,
                Name = place.Category.Name
            },
            Tags = [.. place.PlaceTags.Select(pt => new TagDto
            {
                Id = pt.Tag.Id,
                Name = pt.Tag.Name
            })]
        };
    }

}
