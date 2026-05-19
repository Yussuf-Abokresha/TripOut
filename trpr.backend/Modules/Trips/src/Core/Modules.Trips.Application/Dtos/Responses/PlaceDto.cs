using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Dtos.Responses;

public class PlaceDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int GovernorateId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public GovernorateDto Governorate { get; set; } = default!;
    public CategoryDto Category { get; set; } = default!;
    public ICollection<TagDto> Tags { get; set; } = [];
}
