namespace API.Utils.limiter
{
    public class LoginLimit
    {
        public int Attempts { get; set; }
        public DateTime? LastDateTime { get; set; }
    }
}
