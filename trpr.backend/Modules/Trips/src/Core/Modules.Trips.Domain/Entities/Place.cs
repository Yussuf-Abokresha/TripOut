using NetTopologySuite.Geometries;

namespace Modules.Trips.Domain.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double? Rating { get; set; } = null;
        public double? AverageVisitTime { get; set; } = null;
        public string? OsrmId { get; set; } = null;
        public int VisitCount { get; set; } = 0;
        public int RateCount { get; set; } = 0;
        public int CategoryId { get; set; }
        public int GovernorateId { get; set; }
        public ICollection<Day> Days { get; set; } = [];
        public Point Location { get; set; } = default!;
        public Governorate Governorate { get; set; } = default!;
        public Category Category { get; set; } = default!;
        public ICollection<PlaceTag> PlaceTags { get; set; } = [];
        public static Place Create(
            string title,
            string description,
            int categoryId,
            int governorateId,
            double longitude,
            double latitude)
        {
            return new Place
            {
                Title = title,
                Description = description,
                CategoryId = categoryId,
                GovernorateId = governorateId,
                Location = new Point(longitude, latitude) { SRID = 4326 }
            };
        }

    }


}