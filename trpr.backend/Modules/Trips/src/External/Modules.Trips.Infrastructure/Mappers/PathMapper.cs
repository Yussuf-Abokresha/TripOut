using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Infrastructure.Dtos.Responses;

namespace Modules.Trips.Infrastructure.Mappers;

public static class PathMapper
{
    public static PathResponse ToPathResponse(this RouteResponseDto routeResponse)
    {
        var pathResponse = new PathResponse
        {
            Distance = routeResponse.Routes.Sum(x => x.Distance),
            Duration = routeResponse.Routes.Sum(x => x.Duration),
            Coordinates = [.. routeResponse
                .Routes
                .Select(x => x.Geometry.Coordinates)
                .Select(x => x.Select(c => new Coordinates
                {
                    Latitude = c[1],
                    Longitude = c[0]
                }).ToList())]
        };
        return pathResponse;
    }
}
