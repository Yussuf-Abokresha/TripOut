using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Dtos.Requests
{
    public class SearchTripRequestDto
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public TripPublishMode? PublishMode { get; set; }
        public TripStatus? Status { get; set; }
        public int? ThemeId { get; set; }
        public int? GovernorateId { get; set; }
        public string? Title { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
}
