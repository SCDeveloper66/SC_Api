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
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }


        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(MeetingViewModel))]
        public async Task<MeetingViewModel> GetMeeting(string language)
        {
            var data = await _meetingService.GetMeetingAsync(language);
            return data;
        }


    }
}
