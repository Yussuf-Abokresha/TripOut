using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Application.Dtos.Responses
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}