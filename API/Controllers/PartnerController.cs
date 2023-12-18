using API.DataAccess.Repositories;
using API.Dtos.Partner;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace API.Controllers
{
    [Authorize]
    [Route("api/partner")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthService _authService;
        private readonly IPartnerRepository _partnerRepository;
        public PartnerController(IAccountRepository accountRepository, IAuthService authService, IPartnerRepository partnerRepository)
        {
            _accountRepository = accountRepository;
            _authService = authService;
            _partnerRepository = partnerRepository;
        }

        [HttpPost, Route("decline")]
        public async Task<IActionResult> Decline([FromBody] DeclineRequestDto decline)
        {
            var userId = await _authService.GetUserId(HttpContext);
            if (userId is null)
            {
                return Unauthorized();
            }
            await _partnerRepository.DeclineUser(userId.Value, decline.Id);

            return Ok();
        }

        [HttpGet, Route("matches")]
        public async Task<IActionResult> GetMatches()
        {
            var userId = await _authService.GetUserId(HttpContext);
            if (userId is null)
            {
                return Unauthorized();
            }
            var userBaseInfo = await _accountRepository.GetAccountBaseInfo(userId.Value);
            if (userBaseInfo is null || userBaseInfo.Gender is null)
            {
                return Unauthorized();
            }
            var matches = new List<MatchDto>();
            foreach (Match match in (await _partnerRepository.GetMatches(userId.Value, userBaseInfo.Gender, userBaseInfo.Birthdate)))
            {
                var images = await _accountRepository.GetImages(match.UserId);
                var interests = await _accountRepository.GetInterests(match.UserId);

                matches.Add(
                    new MatchDto
                    {
                        UserId = match.UserId,
                        Name = match.Name,
                        Birthdate = match.Birthdate,
                        Description = match.Description,
                        Avatar = match.Avatar,
                        Images = images,
                        Interests = interests,
                    });
            }
            if (!matches.Any())
            {
                return NotFound();
            }
            return Ok(matches);
        }

        [HttpPost, Route("match")]
        public async Task<IActionResult> GetMatch([FromBody] GetMatchRequestDto matchDto)
        {
            var userId = await _authService.GetUserId(HttpContext);
            if (userId is null)
            {
                return Unauthorized();
            }
            Match? match = await _partnerRepository.GetMatch(matchDto.Id);
            if (match is null)
            {
                return NotFound();
            }

            var images = await _accountRepository.GetImages(match.UserId);
            var interests = await _accountRepository.GetInterests(match.UserId);
            return Ok(new MatchDto
            {
                UserId = match.UserId,
                Name = match.Name,
                Birthdate = match.Birthdate,
                Description = match.Description,
                Avatar = match.Avatar,
                Images = images,
                Interests = interests,
            });
        }
    }
}
