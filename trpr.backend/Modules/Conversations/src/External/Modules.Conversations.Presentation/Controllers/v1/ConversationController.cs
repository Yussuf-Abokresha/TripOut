using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Application.Services;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/conversation")]
    public class ConversationController(IAiChatService aiChatService, ChatService chatService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [Authorize]
        [HttpPost("ai")]
        public async Task<IActionResult> GetPrompt(SendAiPromptRequestDto request)
        {
            var result = await aiChatService.SendMessageAsync(UserId, request);
            return Ok(result);
        }
        [HttpPost("direct")]
        public async Task<IActionResult> SendDirectMessage(SendMessageRequestDto request)
        {
            await chatService.StartDirectMessage(UserId, request);
            return Ok();
        }
        [HttpPost("conversation")]
        public async Task<IActionResult> SendMessageToConversation(SendMessageRequestDto request)
        {
            await chatService.SendMessage(UserId, request);
            return Ok();
        }

        [HttpGet("{Id}/messages")]
        public async Task<ActionResult<ICollection<Message>>> GetMessages([FromRoute] Guid Id, [FromQuery] Guid? LastMessageId, [FromQuery] DateTime? lastSentAt, [FromQuery] bool older = true)
        {
            var messages = await chatService.GetMessagesAsync(Id, LastMessageId, lastSentAt, older);
            return Ok(messages);
        }

        [HttpPost("relay")]
        public async Task<ActionResult<ICollection<Message>>> GetRelayMessage([FromBody] GetRelayMessageRequestDto request)
        {
            var messages = await chatService.GetRelayMessagesAsync(request.LastConversationsMessages);
            return Ok(messages);
        }

        [HttpGet("")]
        public async Task<ActionResult<ICollection<Conversation>>> GetUserConversations()
        {
            var conversations = await chatService.GetUserConversationsAsync(UserId);
            return Ok(conversations);
        }

    }
}