using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Dtos.Responses
{
    public class TripResponseDto
    {
        public Guid CreatedByUser { get; set; }
        public int ThemeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public double ExpectedDuration { get; set; }
        public ICollection<string> ImagesUrls { get; set; } = [];
        public TripVisibility TripVisibility { get; set; }
        public List<PlaceDto> Segments { get; set; } = [];
        public int MaxParticipantsCount { get; set; }
        public Guid? GuideId { get; set; }
        public string? RejectionReason { get; set; }
    }
}
