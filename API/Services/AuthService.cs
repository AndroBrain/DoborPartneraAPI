using API.DataAccess.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public interface IAuthService
    {
        string GenerateToken(string email);
        Task<int?> GetUserId(HttpContext context);
    }
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string GenerateToken(string email)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("$ecr3tKeeyYT11nd3r@pp"));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:8081",
                audience: "http://localhost:8081",
                claims: new List<Claim> { new Claim(ClaimTypes.Email, email) },
                expires: DateTime.Now.AddMinutes(24 * 60),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<int?> GetUserId(HttpContext context)
        {
            return await _userRepository.GetUserId(context.User.FindFirstValue(ClaimTypes.Email));
        }
    }
}
