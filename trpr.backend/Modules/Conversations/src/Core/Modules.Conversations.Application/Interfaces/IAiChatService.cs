using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Conversations.Application.Dtos.Requests;

namespace Modules.Conversations.Application.Interfaces
{
    public interface IAiChatService
    {
        public Task<ICollection<KeyValuePair<string, object?>>> SendMessageAsync(
            Guid userId,
            SendAiPromptRequestDto request,
            CancellationToken cancellationToken = default);
    }
}