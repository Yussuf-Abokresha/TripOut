using System.Globalization;
using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities
{
    public class Profile : Entity
    {
        public Guid Id { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public virtual User User { get; set; } = default!;

        // Navigation properties
        public virtual ICollection<ProfileLanguage> Languages { get; set; } = new List<ProfileLanguage>();
        public virtual ICollection<ProfileInterest> Interests { get; set; } = new List<ProfileInterest>();
        public virtual ICollection<ProfileVibe> Vibes { get; set; } = new List<ProfileVibe>();
    }
}