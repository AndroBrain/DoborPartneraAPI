using System.ComponentModel.DataAnnotations.Schema;

namespace API.Dtos.Partner
{
    public class MatchDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public List<string> Interests { get; set; }
        public List<string> Images { get; set; }
    }
}
