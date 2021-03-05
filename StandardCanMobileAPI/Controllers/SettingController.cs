using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;

namespace StandardCanMobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingController : BaseController
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;

        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(NotiSettingDataViewModel))]
        public async Task<NotiSettingDataViewModel> GetNotiSetting(string language)
        {
            var data = await _settingService.GetNotiSettingAsync(language);
            return data;
        }

        [Route("[action]")]
        [HttpPost]
        [Produces("application/json", Type = typeof(ReturnMsgViewModel))]
        public async Task<IActionResult> NotiSetting(NotiSettingViewModel setting)
        {
            var data = await _settingService.NotiSettingAsync(setting);
            return Ok(data);
        }

    }
}
