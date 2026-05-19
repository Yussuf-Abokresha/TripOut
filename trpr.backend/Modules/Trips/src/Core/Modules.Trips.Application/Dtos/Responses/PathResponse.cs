namespace Modules.Trips.Application.Dtos.Responses;

public class PathResponse
{
    public List<List<Coordinates>> Coordinates { get; set; } = [];
    public double Distance { get; set; }
    public double Duration { get; set; }
}