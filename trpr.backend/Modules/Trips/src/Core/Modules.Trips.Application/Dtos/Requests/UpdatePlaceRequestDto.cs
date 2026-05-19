namespace Modules.Trips.Application.Dtos.Requests;

public class UpdatePlaceRequestDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int? GovernorateId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public List<int>? TagIds { get; set; }

}