namespace Modules.Users.Domain.Entities
{
    public class ProfileLanguage
    {
        public Guid ProfileId { get; set; }
        public int LanguageId { get; set; }
        public virtual Profile Profile { get; set; } = default!;
        public virtual Language Language { get; set; } = default!;
    }
}