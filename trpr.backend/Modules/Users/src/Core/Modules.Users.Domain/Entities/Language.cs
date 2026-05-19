using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities
{
    public class Language : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string NativeName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}