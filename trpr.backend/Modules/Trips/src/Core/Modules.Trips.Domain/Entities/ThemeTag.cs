namespace Modules.Trips.Domain.Entities
{
    public class ThemeTag
    {
        public int Id { get; set; }
        public int ThemeId { get; set; }
        public int TagId { get; set; }
        public int Score { get; set; }
    }
}