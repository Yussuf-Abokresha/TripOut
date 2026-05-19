using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos.Requests
{
    public class UpgradePaginationRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool sortByUpdatedate { get; set; } = false;
        public ApproveStatus status { get; set; } = ApproveStatus.Pending;
    }
}
