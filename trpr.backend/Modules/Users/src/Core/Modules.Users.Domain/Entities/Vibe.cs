using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities
{
    public class Vibe : Entity
    {
        public int Id { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}