using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Trips.Domain.ValueObjects
{
    public enum TripPublishMode
    {
        DirectPublish,
        Bidding
    }
    public enum TripStatus
    {
        UnderReview = 1,
        Published = 2,
        Bidding = 3,
        Completed = 4,
        Started = 5,
        Finished = 6,
        Rejected = 7,
        Canceled = 8,
    }
    public enum TripVisibility
    {
        Private = 0,
        Public = 1,
    }
}
