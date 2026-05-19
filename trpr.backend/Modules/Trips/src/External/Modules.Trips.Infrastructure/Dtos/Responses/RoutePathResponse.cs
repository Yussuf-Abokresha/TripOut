using System.Text.Json.Serialization;
using Modules.Trips.Application.Interfaces;

namespace Modules.Trips.Infrastructure.Dtos.Responses;

public class RouteResponseDto
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    [JsonPropertyName("routes")]
    public List<RouteDto> Routes { get; set; } = [];
    [JsonPropertyName("waypoints")]
    public List<WaypointDto> Waypoints { get; set; } = [];
}

public class RouteDto
{
    [JsonPropertyName("geometry")]
    public GeometryDto Geometry { get; set; } = default!;
    [JsonPropertyName("legs")]
    public List<LegDto> Legs { get; set; } = default!;
    [JsonPropertyName("distance")]
    public double Distance { get; set; }
    [JsonPropertyName("duration")]
    public double Duration { get; set; }
    [JsonPropertyName("weight_name")]
    public string WeightName { get; set; } = string.Empty;
    [JsonPropertyName("weight")]
    public double Weight { get; set; }
}

public class GeometryDto
{
    [JsonPropertyName("coordinates")]
    public List<List<double>> Coordinates { get; set; } = [];
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

public class LegDto
{
    [JsonPropertyName("steps")]
    public List<object> Steps { get; set; } = []; // Empty array in your JSON
    [JsonPropertyName("distance")]
    public double Distance { get; set; }
    [JsonPropertyName("duration")]
    public double Duration { get; set; }
    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;
    [JsonPropertyName("weight")]
    public double Weight { get; set; }
}

public class WaypointDto
{
    [JsonPropertyName("hint")]
    public string Hint { get; set; } = string.Empty;
    [JsonPropertyName("distance")]
    public double Distance { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("location")]
    public List<double> Location { get; set; } = [];
}