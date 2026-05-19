using Modules.Trips.Domain.Abstractions;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Domain.Entities
{
    public class TripState : Entity
    {
        public string? RejectionReason { get; set; }
        public TripPublishMode PublishMode { get; set; } = TripPublishMode.DirectPublish;
        public TripStatus Status { get; set; } = TripStatus.UnderReview;

        public void Approve()
        {
            if (Status != TripStatus.UnderReview)
                throw new InvalidOperationException("Only trips under review can be approved.");
            if (PublishMode == TripPublishMode.DirectPublish)
                Status = TripStatus.Published;
            else
                Status = TripStatus.Bidding;
        }
        public void Reject(string reason)
        {
            if (Status != TripStatus.UnderReview)
                throw new InvalidOperationException("Only trips under review can be rejected.");
            Status = TripStatus.Rejected;
            RejectionReason = reason;
        }
        public void Complete()
        {
            if (Status != TripStatus.Bidding && Status != TripStatus.Started)
                throw new InvalidOperationException("Only Bidding or Started trips can be completed.");
            Status = TripStatus.Completed;
        }
        public void Start()
        {
            if (Status != TripStatus.Published)
                throw new InvalidOperationException("Only published trips can be started.");
            Status = TripStatus.Started;
        }
        public void Cancel()
        {
            if (Status == TripStatus.Canceled)
                throw new InvalidOperationException("Trip is already canceled.");
            Status = TripStatus.Canceled;
        }

    }
}
