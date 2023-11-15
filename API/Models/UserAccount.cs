using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace API.Models
{
    public class UserAccount
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("password_hash")]
        public byte[] PasswordHash { get; set; } = new byte[0];

        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; } = new byte[0];
    }
}
