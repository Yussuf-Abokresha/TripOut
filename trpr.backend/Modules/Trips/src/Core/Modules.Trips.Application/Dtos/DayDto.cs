using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Trips.Application.Dtos
{
    public class DayDto
    {
        public double Duration { get; set; }
        public ICollection<int> PlacesIds { get; set; } = [];
    }
}
