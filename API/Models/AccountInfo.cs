using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AccountInfo
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Column("surname")]
        public string Surname { get; set; } = string.Empty;
        [Column("description")]
        public string Description { get; set; } = string.Empty; 
        [Column("avatar")]
        public string Avatar { get; set; } = string.Empty;

    }
}
