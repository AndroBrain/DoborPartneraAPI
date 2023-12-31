﻿using API.DataAccess.Repositories;
using API.Dtos.Account;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthService _authService;
        public AccountController(IAccountRepository accountRepository, IAuthService authService)
        {
            _accountRepository = accountRepository;
            _authService = authService;
        }


        [HttpPost, Route("info")]
        public async Task<IActionResult> SetInfo([FromBody] SetAccountInfoRequestDto info)
        {
            var userId = await _authService.GetUserId(HttpContext);

            if (userId is not null)
            {
                var userUpdateInfo = new UserUpdateInfo
                {
                    Avatar = info.Avatar,
                    Description = info.Description,
                    Images = info.Images,
                    Interests = info.Interests,
                };
                await _accountRepository.UpdateAccount(userId.Value, userUpdateInfo);
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet, Route("info")]
        public async Task<IActionResult> GetInfo()
        {
            var userId = await _authService.GetUserId(HttpContext);

            if (userId is null)
            {
                return Unauthorized();
            }

            var user = await _accountRepository.GetAccount(userId.Value);
            if (user is null)
            {
                return NotFound();
            }
            var images = await _accountRepository.GetImages(userId.Value);
            var interests = await _accountRepository.GetInterests(userId.Value);
            return Ok(new AccountInfoDto
            {
                Name = user.Name,
                Surname = user.Surname,
                Description = user.Description,
                Avatar = user.Avatar,
                Images = images,
                Interests = interests,
            });
        }

        [HttpPost, Route("test")]
        public async Task<IActionResult> SetTest([FromBody] SetTestRequestDto test)
        {
            var userId = await _authService.GetUserId(HttpContext);
            if (userId is null)
            {
                return Unauthorized();
            }
            await _accountRepository.SetTest(
                new Test
                {
                    UserId = userId.Value,
                    Eyes = test.Eyes,
                    Hair = test.Hair,
                    Tattoo = test.Tattoo,
                    Sport = test.Sport,
                    Education = test.Education,
                    Recreation = test.Recreation,
                    Family = test.Family,
                    Charity = test.Charity,
                    People = test.People,
                    Wedding = test.Wedding,
                    Belief = test.Belief,
                    Money = test.Money,
                    Religious = test.Religious,
                    Mind = test.Mind,
                    Humour = test.Humour,
                }
            );
            return Ok();
        }
    }
}
