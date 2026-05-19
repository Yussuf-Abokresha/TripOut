namespace Modules.Trips.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<PlaceTag> PlaceTags { get; set; } = [];
    }
}