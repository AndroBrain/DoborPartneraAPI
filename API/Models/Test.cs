using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Test
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("eyes")]
        public int Eyes { get; set; }
        [Column("hair")]
        public int Hair { get; set; }
        [Column("tattoo")]
        public int Tattoo { get; set; }
        [Column("sport")]
        public int Sport { get; set; }
        [Column("education")]
        public int Education { get; set; }
        [Column("recreation")]
        public int Recreation { get; set; }
        [Column("family")]
        public int Family { get; set; }
        [Column("charity")]
        public int Charity { get; set; }
        [Column("people")]
        public int People { get; set; }
        [Column("wedding")]
        public int Wedding { get; set; }
        [Column("belief")]
        public int Belief { get; set; }
        [Column("money")]
        public int Money { get; set; }
        [Column("religious")]
        public int Religious { get; set; }
        [Column("mind")]
        public int Mind { get; set; }
        [Column("humour")]
        public int Humour { get; set; }
    }
}
