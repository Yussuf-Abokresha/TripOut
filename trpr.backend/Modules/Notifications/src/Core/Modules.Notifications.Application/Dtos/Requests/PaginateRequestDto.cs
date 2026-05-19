using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Requests
{
    public class PaginateRequestDto
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortBy? sortBy { get; set; }
        public bool? IsActive { get; set; } = null;
        public TemplateType? TemplateType { get; set; }
    }
}
