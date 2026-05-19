using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Domain.Entities
{
    public class Trip : TripState
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User CreatedByUser { get; set; } = null!;
        public int ThemeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public double ActualDuration { get; set; }
        public double ExpectedDuration { get; set; }
        public ICollection<string> Images { get; set; } = [];
        public TripVisibility TripVisibility { get; set; } = TripVisibility.Public;
        public virtual ICollection<Day> Segments { get; set; } = [];
        public virtual ICollection<TripGovernorate> TripGovernorates { get; set; } = [];
        public int MaxParticipantsCount { get; set; }
        public Guid? GuideId { get; set; }
        public ICollection<TripParticipant> Participants { get; set; } = [];
        public static Trip Create(
            Guid userId,
            int themeId,
            string title,
            string description,
            double price,
            ICollection<string> images,
            TripVisibility tripVisibility,
            TripPublishMode publishMode,
            ICollection<ICollection<Place>> segments,
            int maxParticipantCount,
            Guid? guideId,
            List<double> duration,
            User user,
            ICollection<Governorate> governorates)
        {
            Trip newTrip = new()
            {
                PublishMode = publishMode,
                Id = Guid.NewGuid(),
                UserId = userId,
                ThemeId = themeId,
                Title = title,
                Description = description,
                Price = price,
                Images = images,
                TripVisibility = tripVisibility,
                MaxParticipantsCount = maxParticipantCount,
                GuideId = guideId,
                CreatedByUser = user,
                Status = TripStatus.UnderReview,
            };
            foreach (double dur in duration)
            {
                newTrip.ExpectedDuration += dur;
            }
            ICollection<Day> days = [];
            int order = 1;
            foreach (var segment in segments)
            {
                Day day = new()
                {
                    Order = order++,
                    Id = Guid.NewGuid(),
                    TripId = newTrip.Id,
                    Places = segment
                };
                days.Add(day);
            }
            newTrip.Segments = days;
            ICollection<TripGovernorate> tripGovernorates = [];
            foreach (var governorate in governorates)
            {
                TripGovernorate tripGovernorate = new TripGovernorate
                {
                    TripId = newTrip.Id,
                    GovernorateId = governorate.Id
                };
                tripGovernorates.Add(tripGovernorate);
            }
            newTrip.TripGovernorates = tripGovernorates;
            return newTrip;
        }

        // update method for adding/removing participants
        // update method for adding new places?
        // update method for trip details
    }
}
