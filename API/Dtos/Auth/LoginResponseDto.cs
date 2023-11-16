namespace API.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public bool IsProfileFilled { get; set; }
    }
}
