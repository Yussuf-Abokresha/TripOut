using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Domain.Entities
{
    public class User : Entity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public virtual ICollection<Trip> CreatedTrips { get; set; } = [];
        public virtual ICollection<TripParticipant> JoinedTrips { get; set; } = [];
    }
}
