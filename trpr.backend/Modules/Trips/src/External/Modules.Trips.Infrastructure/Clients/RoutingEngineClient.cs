using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Infrastructure.Dtos.Responses;
using Modules.Trips.Infrastructure.Options;

namespace Modules.Trips.Infrastructure.Clients;

public class RoutingEngineClient
{
    public HttpClient HttpClient { get; set; }
    public RoutingEngineOptions RoutingEngineOptions { get; set; }
    public RoutingEngineClient(IOptions<RoutingEngineOptions> options, HttpClient httpClient)
    {
        this.RoutingEngineOptions = options.Value;
        this.HttpClient = httpClient;
    }
    public async Task<RouteResponseDto> GetRoutePathAsync(ICollection<Coordinates> coordinates,
        PathType pathType, CancellationToken cancellationToken)
    {
        string baseUrl = pathType == PathType.Driving
            ? RoutingEngineOptions.Driving
            : RoutingEngineOptions.Walking;

        baseUrl = baseUrl.TrimEnd('/'); // Ensure no trailing slash

        string coordinatesPath = string.Join(";", coordinates.Select(c => $"{c.Longitude},{c.Latitude}"));

        IDictionary<string, string?> queryParams = new Dictionary<string, string?>
        {
            {"overview", "full"},
            {"geometries", "geojson"}
        };

        var url = QueryHelpers.AddQueryString($"{baseUrl}/{coordinatesPath}", queryParams);

        HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(url, cancellationToken);
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        return await httpResponseMessage.Content.ReadFromJsonAsync<RouteResponseDto>(cancellationToken) ?? throw new InvalidOperationException("Failed to deserialize the response content.");
    }
}
