using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Application.Dtos;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Mappers
{
    public static class GuideUpgradeRequestMapper
    {
        public static GuideUpgradeResponseDto ToResponseDto(this GuideUpgradeRequest upgradeRequest, ICollection<DocumentDto> docs)
        {
            return new GuideUpgradeResponseDto
            {
                Id = upgradeRequest.Id,
                Documents = docs,
                Status = upgradeRequest.Status,
                CreatedAt = upgradeRequest.CreatedAtUTC,
                UpdatedAt = upgradeRequest.UpdatedAtUTC,
                ReviewedAt = upgradeRequest.ReviewedAtUTC ?? null,
                adminId = upgradeRequest.adminId ?? null,
                RejectionReason = upgradeRequest.RejectionReason ?? null,
                user = upgradeRequest.user.ToResponseDto(),
            };
        }
    }
}
