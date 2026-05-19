using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Interfaces;

namespace Modules.Trips.Infrastructure.Services;

public class MockRoutingService : IRoutingService
{
    public Task<PathResponse> GetPathAsync(IList<Coordinates> coordinates, PathType pathType, CancellationToken cancellationToken = default)
    {
        double distance = 0;
        double duration = 0;

        for (int i = 1; i < coordinates.Count; i++)
        {
            distance += Math.Sqrt(
                Math.Pow(coordinates[i].Latitude - coordinates[i - 1].Latitude, 2) +
                Math.Pow(coordinates[i].Longitude - coordinates[i - 1].Longitude, 2));
            duration += 600;
        }
        var mockResponse = new PathResponse
        {
            Coordinates = [[.. coordinates]],
            Distance = distance, // Mock distance in meters
            Duration = duration  // Mock duration in seconds
        };

        return Task.FromResult(mockResponse);
    }
}
