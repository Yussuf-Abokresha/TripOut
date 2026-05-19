namespace Modules.Users.Domain.Entities
{
    public class ProfileInterest
    {
        public Guid ProfileId { get; set; }
        public int InterestId { get; set; }
        public virtual Profile Profile { get; set; } = default!;
        public virtual Interest Interest { get; set; } = default!;
    }
}