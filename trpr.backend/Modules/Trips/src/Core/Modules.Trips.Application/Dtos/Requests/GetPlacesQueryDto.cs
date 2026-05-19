namespace Modules.Trips.Application.Dtos.Requests;

public class GetPlacesQueryDto
{
    public int? GovernorateId { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public int? RadiusInMeters { get; set; }
}
