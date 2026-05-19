using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Mappers
{
    public static class UpgradePaginationMapper
    {
        public static UpgradePaginationResponseDto ToResponseDto(this GuideUpgradeRequest request)
        {
            return new UpgradePaginationResponseDto
            {
                Id = request.Id,
                UserId = request.userId,
                UserName = request.user.FirstName + " " + request.user.LastName,
                Subject = request.Subject,
                ApproveStatus = request.Status,
                CreatedAt = request.CreatedAtUTC
            };
        }
    }
}
