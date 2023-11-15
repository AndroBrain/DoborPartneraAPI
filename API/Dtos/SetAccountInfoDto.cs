using System.ComponentModel.DataAnnotations.Schema;

namespace API.Dtos
{
    public class SetAccountInfoDto
    {
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string[] Images { get; set; }
        public string[] Interests { get; set; }
    }
}
