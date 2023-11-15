using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Conversation
    {

        [Column("user_id")]
        public int UserId { get; set; }
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Column("avatar")]
        public string Avatar { get; set; } = string.Empty;
    }
}
