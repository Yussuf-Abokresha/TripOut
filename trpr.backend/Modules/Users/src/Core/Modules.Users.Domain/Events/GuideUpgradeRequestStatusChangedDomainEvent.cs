using Common.Domain;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Domain.Events
{
    public class GuideUpgradeRequestStatusChangedDomainEvent : DomainEvent
    {
        public Guid GuideUpgradeRequestId { get; }
        public Guid UserId { get; }
        public ApproveStatus NewStatus { get; }
        public string? RejectionReason { get; }
        public GuideUpgradeRequestStatusChangedDomainEvent(Guid guideUpgradeRequestId, Guid userId, ApproveStatus newStatus, string? rejectionReason = null)
        {
            GuideUpgradeRequestId = guideUpgradeRequestId;
            UserId = userId;
            NewStatus = newStatus;
            RejectionReason = rejectionReason;
        }
    }
}