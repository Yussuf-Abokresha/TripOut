using Microsoft.AspNetCore.Http;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Dtos.Requests
{

    public class CreateTripRequestDto
    {
        public int ThemeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public List<string> Images { get; set; } = [];
        public TripVisibility TripVisibility { get; set; }
        public TripPublishMode PublishMode { get; set; }
        public List<DayDto> Segments { get; set; } = [];
        public int MaxParticipantsCount { get; set; }
        public Guid? GuideId { get; set; }

    }
}
