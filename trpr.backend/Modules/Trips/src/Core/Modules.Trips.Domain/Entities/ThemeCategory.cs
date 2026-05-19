namespace Modules.Trips.Domain.Entities
{
    public class ThemeCategory
    {
        public int Id { get; set; }
        public int ThemeId { get; set; }
        public int CategoryId { get; set; }
        public int MaxLimit { get; set; }
    }
}