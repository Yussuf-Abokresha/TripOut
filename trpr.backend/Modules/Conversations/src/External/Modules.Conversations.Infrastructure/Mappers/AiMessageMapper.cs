using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.Serialization;
using Microsoft.Extensions.AI;
using Modules.Conversations.Domain.Entities;
using Newtonsoft.Json;

namespace Modules.Conversations.Infrastructure.Mappers
{
    public static class AiMessageMapper
    {
        public static AiMessage ToAiMessage(this ChatMessage userPrompt, AiConversation conversation, ICollection<ChatMessage>? responses = null)
        {
            responses ??= [];
            var userPromptContent = JsonConvert.SerializeObject(userPrompt, SerializerSettings.Instance)!;

            ICollection<string> responsesChatContent = [..
                responses
                .Select(x =>
                    JsonConvert.SerializeObject(x, SerializerSettings.Instance)!
                )];

            return AiMessage.Create(conversation, userPromptContent, responsesChatContent);
        }

        public static ICollection<ChatMessage> ToChatMessage(this ICollection<AiMessage> aiMessages)
        {
            var chatMessages = new List<ChatMessage>();
            foreach (var aiMessage in aiMessages)
                chatMessages.Add(aiMessage.ToChatMessage());
            return chatMessages;
        }

        public static ChatMessage ToChatMessage(this AiMessage aiMessage)
        {
            return JsonConvert.DeserializeObject<ChatMessage>(aiMessage.Contnet, SerializerSettings.Instance)!;
        }
    }
}