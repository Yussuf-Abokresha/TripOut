using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Application.Dtos
{
    public class PaginationDto<T>
    {
        public ICollection<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public static PaginationDto<T> Create(int page, int pageSize, int totalItems, ICollection<T> items)
        {
            return new PaginationDto<T>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = items
            };
        }
    }
}