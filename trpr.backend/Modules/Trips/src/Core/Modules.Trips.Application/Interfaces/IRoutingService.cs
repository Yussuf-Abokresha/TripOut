using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Responses;

namespace Modules.Trips.Application.Interfaces;

public interface IRoutingService
{
    public Task<PathResponse> GetPathAsync(
        IList<Coordinates> coordinates,
        PathType pathType,
        CancellationToken cancellationToken = default);
}
