using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using Common.Application.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Domain.Entities;
using Modules.Conversations.Infrastructure.Mappers;
using Newtonsoft.Json;

namespace Modules.Conversations.Infrastructure.Services
{
    public class AiChatService(IChatClient chatClient, RepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : IAiChatService
    {
        private readonly ChatMessage systemPrompt = new ChatMessage(ChatRole.System, """
            You are a helpful AI assistant. 
            - Always be concise and polite in your responses. 
            """);
        static string GetWeather(string city)
        {
            return $"The weather in {city} is sunny.";
        }
        static ICollection<string> GetEvents(string city)
        {
            return
            [
                $"Tech Conference 2026 in {city}",
                $"Music Festival in {city} Downtown",
                $"Startup Networking Meetup in {city}",
                $"Food Expo at {city} International Center",
                $"AI & Cloud Summit in {city}"
            ];
        }

        public async Task<ICollection<KeyValuePair<string, object?>>> SendMessageAsync(Guid userId, SendAiPromptRequestDto request, CancellationToken cancellationToken = default)
        {

            AiConversation conversation =
                request.ConversationId == null ? AiConversation.Create(userId) : await repositoryFactory
                .Repository<AiConversation>()
                .GetQueryable()
                .Where(x => x.Id == request.ConversationId && x.UserId == userId)
                .Select(x => new AiConversation
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Messages = x.Messages
                        .Where(m => m.ParentMessageId == null)
                        .OrderBy(m => m.Id)
                        .Take(5)
                        .Select(m => new AiMessage
                        {
                            Id = m.Id,
                            ParentMessageId = m.ParentMessageId,
                            Contnet = m.Contnet,
                            SubMessages = m.SubMessages
                                .OrderBy(sm => sm.CreatedOnUtc)
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException("Conversation.NotFound");

            ICollection<ChatMessage> chatHistory = conversation
                .Messages
                .SelectMany(
                    m => new[] { m }.Concat(m.SubMessages)
                ).ToList().ToChatMessage();

            var userPrompt = new ChatMessage(ChatRole.User, request.Prompt);

            var messagesToBeSent =
                new List<ChatMessage> { systemPrompt }
                .Concat(chatHistory)
                .Concat([userPrompt]);

            ChatResponse response = await chatClient.GetResponseAsync(
                messages: messagesToBeSent,
                options: GenerateChatOptions(),
                cancellationToken: cancellationToken);

            var aiMessage = userPrompt.ToAiMessage(conversation, response.Messages);
            repositoryFactory.Repository<AiConversation>().Attach(conversation);
            repositoryFactory.Repository<AiMessage>().Add(aiMessage);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return [.. messagesToBeSent.Concat(response.Messages)
                .Select(m => new KeyValuePair<string, object?>(
                    m.Role.ToString(), m))];

        }
        private static ChatOptions GenerateChatOptions()
        {
            return new ChatOptions
            {
                ModelId = "gemini-3-flash-preview",
                Temperature = 0.7f,
                TopP = 0.9f,
                Tools = [
                    AIFunctionFactory.Create(
                        name : "get_weather",
                        description : "Get the current weather for a specific city by name.",
                        method : GetWeather
                    ),
                    AIFunctionFactory.Create(
                        name : "get_events",
                        description : "Get a list of upcoming events in a specific city.",
                        method : GetEvents
                    )],
                ToolMode = ChatToolMode.Auto,
            };
        }
    }
}