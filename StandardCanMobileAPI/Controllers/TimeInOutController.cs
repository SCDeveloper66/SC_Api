using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class TimeInOutController : BaseController
    {
        private readonly ITimeInOutService _timeInOutService;

        public TimeInOutController(ITimeInOutService timeInOutService)
        {
            _timeInOutService = timeInOutService;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(TimeInOutViewModel))]
        public async Task<TimeInOutViewModel> GetInoutRealtime(string language)
        {
            var data = await _timeInOutService.GetInoutRealtimeAsync(language);
            return data;
        }


        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(InoutEmpRealtimeViewModel))]
        public async Task<InoutEmpRealtimeViewModel> GetInoutEmpRealtime(string language)
        {
            var data = await _timeInOutService.GetInoutEmpRealtimeAsync(language);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(InoutEmpRealtimeSearchViewModel))]
        public async Task<InoutEmpRealtimeSearchViewModel> GetInoutEmpRealtimeSearch(string language)
        {
            var data = await _timeInOutService.GetInoutEmpRealtimeSearchAsync(language);
            return data;
        }

        [Route("[action]")]
        [HttpPost]
        [Produces("application/json", Type = typeof(ReturnMsgViewModel))]
        public async Task<IActionResult> CheckinOutdoor(CheckinOutdoorViewModel dataCheckin)
        {
            var data = await _timeInOutService.CheckinOutdoorAsync(dataCheckin);
            return Ok(data);
        }

        [Route("[action]")]
        [HttpPost]
        [Produces("application/json", Type = typeof(ReturnMsgViewModel))]
        public async Task<IActionResult> CheckInTime(CheckInTimeViewModel dataCheckin)
        {
            var data = await _timeInOutService.CheckInTimeAsync(dataCheckin);
            return Ok(data);
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(SummaryTimeViewModel))]
        public async Task<SummaryTimeViewModel> GetSummaryTime(string language, string type, string year, string month, string start, string stop)
        {
            DateTime? startDate = null;
            DateTime? stopDate = null;
            if (!String.IsNullOrEmpty(start))
            {
                var tempSDateM = start.Split('-');
                if (tempSDateM.Length > 3)
                {
                    startDate = DateTime.ParseExact(tempSDateM[0] + "-" + tempSDateM[1] + "-" + tempSDateM[2], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                var tempSDateP = start.Split(' ');
                if (tempSDateP.Length > 1)
                {
                    startDate = DateTime.ParseExact(tempSDateP[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
            }
            if (!String.IsNullOrEmpty(stop))
            {
                var tempSDateM = stop.Split('-');
                if (tempSDateM.Length > 3)
                {
                    stopDate = DateTime.ParseExact(tempSDateM[0] + "-" + tempSDateM[1] + "-" + tempSDateM[2], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                var tempSDateP = stop.Split(' ');
                if (tempSDateP.Length > 1)
                {
                    stopDate = DateTime.ParseExact(tempSDateP[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
            }

            var data = await _timeInOutService.GetSummaryTimeAsync(language, type, year, month, startDate, stopDate);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(SummaryTimeFilterViewModel))]
        public async Task<SummaryTimeFilterViewModel> GetSummaryTimeFilter(string language)
        {
            var data = await _timeInOutService.GetSummaryTimeFilterAsync(language);
            return data;
        }

    }
}
