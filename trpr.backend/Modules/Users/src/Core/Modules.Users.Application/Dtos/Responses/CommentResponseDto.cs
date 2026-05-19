using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Users.Application.Dtos.Responses
{
    public class CommentResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string? ParentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}