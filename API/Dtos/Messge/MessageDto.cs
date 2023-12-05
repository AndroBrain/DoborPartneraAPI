using System.ComponentModel.DataAnnotations.Schema;

namespace API.Dtos.Messge
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int FromUser { get; set; }
        public int ToUser { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public long SentTimestamp { get; set; }
    }
}
