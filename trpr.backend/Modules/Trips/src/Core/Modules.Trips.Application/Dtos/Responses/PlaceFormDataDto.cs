namespace Modules.Trips.Application.Dtos.Responses;

public class PlaceFormDataDto
{
    public List<CategoryDto> Categories { get; set; } = [];
    public List<GovernorateDto> Governorates { get; set; } = [];
    public List<TagDto> Tags { get; set; } = [];
}
