using API.DataAccess.Repositories;
using API.Models;
using API.Services;
using API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public ChatHub(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository= messageRepository;
            _userRepository= userRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userEmail = Context.User.FindFirstValue(ClaimTypes.Email);
            var id = await _userRepository.GetUserId(userEmail);

            await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userEmail = Context.User.FindFirstValue(ClaimTypes.Email);
            var id = await _userRepository.GetUserId(userEmail);

            await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(int receiverId, string messageText)
        {
            var senderEmail = Context.User.FindFirstValue(ClaimTypes.Email);
            var senderId = await _userRepository.GetUserId(senderEmail);
            var timestamp = DateTimeOffset.Now.ToUnixTimeMicroseconds();
            var message = new Message
            {
                FromUser = senderId.Value,
                ToUser = receiverId,
                MessageText = messageText,
                SentTimestamp = timestamp
            };
            await _messageRepository.AddMessage(message);

            await Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage", message.Id,receiverId, senderId, message.MessageText, timestamp);
            await Clients.Group(senderId.Value.ToString()).SendAsync("ReceiveMessage",message.Id, receiverId, senderId, message.MessageText, timestamp);
        }
    }
}
