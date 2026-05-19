using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Interfaces;
using Modules.Trips.Infrastructure.Clients;
using Modules.Trips.Infrastructure.Dtos.Responses;
using Modules.Trips.Infrastructure.Mappers;

namespace Modules.Trips.Infrastructure.Services;

public class RoutingService(RoutingEngineClient routingEngineClient) : IRoutingService
{
    public async Task<PathResponse> GetPathAsync(IList<Coordinates> coordinates, PathType pathType, CancellationToken cancellationToken = default)
    {
        RouteResponseDto response = await routingEngineClient.GetRoutePathAsync(coordinates, pathType, cancellationToken);
        return response.ToPathResponse();
    }
}
