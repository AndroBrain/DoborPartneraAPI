namespace API.Dtos.Account
{
    public class AccountInfoDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public List<string> Images { get; set; }
        public List<string> Interests { get; set; }
    }
}
