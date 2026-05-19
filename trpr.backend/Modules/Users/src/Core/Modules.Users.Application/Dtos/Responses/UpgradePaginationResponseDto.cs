using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos.Responses
{
    public class UpgradePaginationResponseDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string? Subject { get; set; }
        public ApproveStatus ApproveStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
