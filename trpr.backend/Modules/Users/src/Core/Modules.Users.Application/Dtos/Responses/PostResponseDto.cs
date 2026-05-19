using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Dtos.Responses
{

    public class PostResponseDto
    {
        public Guid Id { get; set; }
        public float Rating { get; set; }
        public uint TotalRatings { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
        public DateTime CreatedOnUtc { get; set; }
    }
}