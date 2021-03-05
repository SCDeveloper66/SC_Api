using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StandardCanMobileAPI.Helper;
using StandardCanMobileAPI.Models;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;

namespace StandardCanMobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(EmpProfileViewModel))]
        public async Task<EmpProfileViewModel> GetProfile(string language)
        {
            var data = await _userService.GetProfileAsync(language);
            return data;
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(ReturnMsgViewModel))]
        public async Task<IActionResult> UpdateUser(EmpProfileDataViewModel user)
        {
            var data = await _userService.UpdateUserAsync(user);
            return Ok(data);
        }

        [Route("[action]")]
        [HttpPost]
        [Produces("application/json", Type = typeof(ReturnMsgViewModel))]
        public async Task<IActionResult> UserChangePassword(ChangePasswordViewModel user)
        {
            var data = await _userService.UserChangePasswordAsync(user);
            return Ok(data);
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(UserNotiDetail))]
        public async Task<UserNotiDetail> GetNotiDetail(string noti_id, string language)
        {
            var data = await _userService.GetNotiDetailAsync(noti_id, language);
            return data;
        }

    }
}
