using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Match
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("birthdate")]
        public DateTime Birthdate { get; set; }
        [Column("description")]
        public string Description { get; set; }
        
        [Column("avatar")]
        public string Avatar { get; set; }
    }
}
