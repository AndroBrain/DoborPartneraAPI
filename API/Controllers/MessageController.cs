using API.DataAccess.Repositories;
using API.Dtos.Messge;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IAuthService _authService;

        public MessageController(IMessageRepository messageRepository, IAuthService authService)
        {
            _messageRepository = messageRepository;
            _authService = authService;
        }

        [HttpGet, Route("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var userId = await _authService.GetUserId(HttpContext);
            if (userId is null)
            {
                return Unauthorized();
            }
            var conversations = await _messageRepository.GetConversations(userId.Value);

            return Ok(
                conversations.ConvertAll(conversation => new ProfileWithMessagesDto
                {
                    Id = conversation.UserId,
                    Name = conversation.Name,
                    Avatar = conversation.Avatar,
                    messages = conversation.Messages,
                }
                )
            );
        }

        [HttpPost, Route("messages")]
        public async Task<IActionResult> GetMessages([FromBody] GetMessagesRequestDto dto)
        {
            var userId = await _authService.GetUserId(HttpContext);
            if (userId is null)
            {
                return Unauthorized();
            }
            var messages = await _messageRepository.GetMessages(userId.Value, dto.Id, dto.LastMessageTimestamp);
            return Ok(new GetMessagesResponseDto
            {
                Messages = messages.ConvertAll(message => new MessageDto
                {
                    Id = message.Id,
                    FromUser = message.FromUser,
                    ToUser = message.ToUser,
                    MessageText = message.MessageText,
                    SentTimestamp = message.SentTimestamp,
                }),
                CanLoadMore = messages.Count >= 10
            });
        }
    }
}
