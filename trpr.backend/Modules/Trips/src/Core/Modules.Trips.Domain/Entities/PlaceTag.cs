namespace Modules.Trips.Domain.Entities
{
    public class PlaceTag
    {
        public int TagId { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; } = default!;
        public Tag Tag { get; set; } = default!;
    }
}