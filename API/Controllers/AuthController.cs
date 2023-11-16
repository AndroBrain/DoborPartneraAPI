using API.DataAccess.Repositories;
using API.Dtos.Auth;
using API.Models;
using API.Services;
using API.Utils.limiter;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthService _tokenGenerator;
        private readonly ILoginLimiter _loginLimiter;
        public AuthController(IAccountRepository accountRepository, IAuthService tokenGenerator, ILoginLimiter loginLimiter)
        {
            _accountRepository = accountRepository;
            _tokenGenerator = tokenGenerator;
            _loginLimiter = loginLimiter;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto credentials)
        {
            await Task.Delay(2000);
            if (!_loginLimiter.CanLogin(credentials.Email))
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, "Too Many Request try again in 10 minutes");
            }
            _loginLimiter.AddAttempt(credentials.Email);


            var user = await _accountRepository.GetUser(credentials.Email);

            if (user is null) return BadRequest("Invalid username or password.");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(credentials.Password));

            for (int i = 0; i < computedHash.Length; ++i)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid username or password.");
            }
            var account = await _accountRepository.GetAccount(user.Id);
            _loginLimiter.ResetAttempts(credentials.Email);
            return Ok(new LoginResponseDto
            {
                Token = _tokenGenerator.GenerateToken(credentials.Email),
                IsProfileFilled = account is not null && account.Avatar.Length != 0,
            });
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto credentials)
        {
            if (await _accountRepository.GetUser(credentials.Email) is not null) return BadRequest("Email already taken.");

            using var hmac = new HMACSHA512();

            var newAccount = new UserAccount
            {
                Email = credentials.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(credentials.Password)),
                PasswordSalt = hmac.Key
            };

            var newAccountBaseInfo = new AccountBaseInfo
            {
                Name = credentials.Name,
                Surname = credentials.Surname,
                Gender = credentials.Gender,
                Birthdate = credentials.Birthdate,
            };

            await _accountRepository.AddNewAccount(newAccount, newAccountBaseInfo);

            return Ok(new RegisterResponseDto
            {
                Token = _tokenGenerator.GenerateToken(newAccount.Email),
            });
        }
    }
}
