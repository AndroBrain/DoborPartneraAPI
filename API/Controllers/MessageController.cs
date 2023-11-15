using API.DataAccess.Repositories;
using API.Dtos;
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
                conversations.ConvertAll(conversation => new ConversationDto
                {
                    Id = conversation.UserId,
                    Name = conversation.Name,
                    Avatar = conversation.Avatar,
                }
                )
            );
        }
    }
}
