namespace Modules.Users.Domain.Entities
{
    public class ProfileVibe
    {
        public Guid ProfileId { get; set; }
        public int VibeId { get; set; }
        public virtual Profile Profile { get; set; } = default!;
        public virtual Vibe Vibe { get; set; } = default!;
    }
}