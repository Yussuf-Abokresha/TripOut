using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities
{
    public class Interest : Entity
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}