using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AccountBaseInfo
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("surname")]
        public string Surname { get; set; } = string.Empty;

        [Column("gender")]
        public string Gender { get; set; } = string.Empty;

        [Column("birthdate")]
        public DateTime Birthdate { get; set; }
    }
}
