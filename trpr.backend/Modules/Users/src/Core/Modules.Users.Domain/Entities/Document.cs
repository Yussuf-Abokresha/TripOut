using Modules.Users.Domain.Abstractions;
using Modules.Users.Domain.ValueObjects;


namespace Modules.Users.Domain.Entities
{
    public class Document : Entity
    {
        public Guid Id { get; set; }
        public Guid GuideRequestId { get; set; }
        public DocumentType Type { get; set; }
        public string FileUrl { get; set; } = null!;
    }
}