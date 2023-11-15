using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class UserUpdateInfo
    {
        [Column("avatar")]
        public string Avatar { get; set; }
        [Column("description")]
        public string Description { get; set; }
        public string[] Images { get; set; }
        public string[] Interests { get; set; }
    }
}
