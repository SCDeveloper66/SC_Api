using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;

namespace StandardCanMobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;

        }

        [Route("[action]")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> UserLogin([FromBody] UserViewModel user)
        {
            var jwt = await _userService.UserLogin(user.UserName, user.Password, user.token_noticiation);
            return Ok(jwt);
        }

        [Route("[action]")]
        [HttpPost]
        [Produces("application/json", Type = typeof(ReturnMsgViewModel))]
        public async Task<IActionResult> UserResetPassword(ResetPasswordViewModel user)
        {
            var data = await _userService.UserResetPasswordAsync(user);
            return Ok(data);
        }

    }
}
