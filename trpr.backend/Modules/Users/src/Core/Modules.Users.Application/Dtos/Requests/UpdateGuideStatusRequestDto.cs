using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos.Requests
{
    public class UpdateGuideStatusRequestDto
    {
        public Guid UserId { get; set; }
        public Guid UpgradeRequestId { get; set; }
        public ApproveStatus Status { get; set; }
        public string? RejectionReason { get; set; }
    }
}
