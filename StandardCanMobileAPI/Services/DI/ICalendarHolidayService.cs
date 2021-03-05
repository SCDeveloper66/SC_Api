using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface ICalendarHolidayService
    {
        Task<CalendarHolidayViewModel> GetCalendarHolidayAsync(string language);
        Task<CalendarScheduleViewModel> GetCalendarScheduleAsync(string language);
    }
}
