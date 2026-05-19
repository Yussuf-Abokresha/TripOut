using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain;

namespace Common.Application.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
            where T : IIntegrationEvent;
    }
}