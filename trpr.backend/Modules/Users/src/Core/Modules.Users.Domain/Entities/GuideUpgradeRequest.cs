using System.Data;
using Modules.Users.Domain.Abstractions;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Domain.Entities
{
    public class GuideUpgradeRequest : Entity
    {
        public Guid Id { get; set; }
        public Guid userId { get; set; }
        public ApproveStatus Status { get; set; } = ApproveStatus.Pending;
        public string? RejectionReason { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public Guid? adminId { get; set; }
        public DateTime? ReviewedAtUTC { get; set; }
        //TODO: add guide languages
        public virtual ICollection<Document> Documents { get; set; } = [];
        public virtual User user { get; set; } = null!;
        public static GuideUpgradeRequest Create(Guid userid, Dictionary<string, DocumentType> documents)
        {
            var upgradeRequest = new GuideUpgradeRequest
            {
                Id = Guid.NewGuid(),
                userId = userid,
                Status = ApproveStatus.Pending,
            };
            ICollection<Document> docs = [];
            foreach (var document in documents)
            {
                var docm = new Document
                {
                    Id = Guid.NewGuid(),
                    GuideRequestId = upgradeRequest.Id,
                    Type = document.Value,
                    FileUrl = document.Key,
                };
                docs.Add(docm);
            }
            upgradeRequest.Documents = docs;
            return upgradeRequest;
        }
        public void UpdateStatus(Guid AdminId, Guid userId,ApproveStatus status, string? reason = null)
        {
            Status = status;
            RejectionReason = reason;
            adminId = AdminId;
            ReviewedAtUTC = DateTime.UtcNow;
            this.RaiseDomainEvent(new GuideUpgradeRequestStatusChangedDomainEvent(AdminId, userId, status, reason));
        }
    }


}