using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;

namespace StandardCanMobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarHolidayController : BaseController
    {

        private readonly ICalendarHolidayService _calendarHolidayService;

        public CalendarHolidayController(ICalendarHolidayService calendarHolidayService)
        {
            _calendarHolidayService = calendarHolidayService;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(CalendarHolidayViewModel))]
        public async Task<CalendarHolidayViewModel> GetCalendarHoliday(string language)
        {
            var data = await _calendarHolidayService.GetCalendarHolidayAsync(language);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(CalendarScheduleViewModel))]
        public async Task<CalendarScheduleViewModel> GetCalendarSchedule(string language)
        {
            var data = await _calendarHolidayService.GetCalendarScheduleAsync(language);
            return data;
        }

    }
}
