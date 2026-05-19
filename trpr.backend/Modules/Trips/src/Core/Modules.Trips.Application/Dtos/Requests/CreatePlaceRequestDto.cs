namespace Modules.Trips.Application.Dtos.Requests;

public class CreatePlaceRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int GovernorateId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<int> TagIds { get; set; } = [];
}
