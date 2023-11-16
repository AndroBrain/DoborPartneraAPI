namespace API.Dtos.Auth
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
