using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class CalendarHolidayViewModel
    {
        public messageModel message { get; set; }
        public List<CalendarHoliday> calendarholiday_list { get; set; }
    }

    public class CalendarScheduleViewModel
    {
        public messageModel message { get; set; }
        public List<CalendarSchedule> calendarschedule_list { get; set; }
    }

    public class CalendarHoliday
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string title { get; set; }
        public string remark { get; set; }
    }

    public class CalendarSchedule
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string title { get; set; }
        public string remark { get; set; }
    }

}
