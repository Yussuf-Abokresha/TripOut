using Modules.Conversations.Domain.Abstractions;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Domain.Entities
{
    public class MessageAttachment : Entity
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public virtual Message Message { get; set; } = default!;
        public string AttachmentName { get; set; } = string.Empty;
        public AttachmentType AttachmentType { get; set; } = AttachmentType.Image;
        public long AttachmentSize { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}