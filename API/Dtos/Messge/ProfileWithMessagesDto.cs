using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Dtos.Messge
{
    public class ProfileWithMessagesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public List<Message> messages { get; set; } = new List<Message>();
    }
}
