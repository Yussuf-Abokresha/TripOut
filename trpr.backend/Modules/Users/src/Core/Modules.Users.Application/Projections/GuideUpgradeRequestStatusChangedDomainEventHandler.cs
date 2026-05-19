using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application.DomainEvents;
using Modules.Notifications.Contracts.Contracts;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Projections
{
    public class GuideUpgradeRequestStatusChangedDomainEventHandler(INotifiyContract notifyContract) : IDomainEventHandler<GuideUpgradeRequestStatusChangedDomainEvent>
    {
        public async Task HandleAsync(GuideUpgradeRequestStatusChangedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await notifyContract.NotifyAsync(new Notifications.Contracts.Dtos.SystemNotifyRequestDto(
                NotifyEmail: true,
                NotifyPhone: true,
                NotifySystem: true,
                TemplateType: TemplateTypeSwitch(domainEvent.NewStatus),
                ToUserIds: [domainEvent.UserId],
                ToEmails: [],
                ToPhoneNumbers: [],
                KeyValuePairs: new Dictionary<string, string>
                {
                    { "rejectionReason" , domainEvent.RejectionReason ?? string.Empty }
                },
                LangCode: "en"
            ), cancellationToken);
        }
        private string TemplateTypeSwitch(ApproveStatus status)
        {
            return status switch
            {
                ApproveStatus.Approved => "ApprovalMessage",
                ApproveStatus.Rejected => "RejectionMessage",
                _ => throw new NotSupportedException($"Unsupported approve status: {status}")
            };
        }
    }
}
