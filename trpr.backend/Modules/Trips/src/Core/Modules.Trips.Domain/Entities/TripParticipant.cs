using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Domain.Entities
{
    public class TripParticipant : Entity
    {
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
        public virtual Trip Trip { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}
