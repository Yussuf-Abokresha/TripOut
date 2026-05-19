namespace Modules.Trips.Domain.Entities
{
    public class TripGovernorate
    {
        public Trip Trip { get; set; } = default!;
        public Governorate Governorate { get; set; } = default!;
        public Guid TripId { get; set; }
        public int GovernorateId { get; set; }
    }
}
