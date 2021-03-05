using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StandardCanMobileAPI.Helper;
using StandardCanMobileAPI.Models;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services
{
    public class CalendarHolidayService : ICalendarHolidayService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;

        public CalendarHolidayService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _systemLogService = systemLogService;
            _configuration = configuration;
            _accessor = httpContextAccessor;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;
        public async Task<CalendarHolidayViewModel> GetCalendarHolidayAsync(string language)
        {
            var data = new CalendarHolidayViewModel();
            data.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = userId,
                        lang = language,
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/CalendarHoliday/GetCalendarHoliday",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbGetHoliday.FromSqlRaw("sp_mb_get_holiday").ToList();
                    data.calendarholiday_list = new List<CalendarHoliday>();
                    foreach (var item in spData)
                    {
                        CalendarHoliday calendarHoliday = new CalendarHoliday();
                        calendarHoliday.startTime = item.startTime;
                        calendarHoliday.endTime = item.endTime;
                        calendarHoliday.title = item.title;
                        calendarHoliday.remark = item.remark;
                        data.calendarholiday_list.Add(calendarHoliday);
                    }
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<CalendarScheduleViewModel> GetCalendarScheduleAsync(string language)
        {
            var data = new CalendarScheduleViewModel();
            data.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = userId,
                        lang = language,
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/CalendarHoliday/GetCalendarSchedule",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbGetSchedule.FromSqlRaw("sp_mb_get_schedule @emp_id", emp_id).ToList();
                    //context.SpMbEmpOTHoursResult.FromSqlRaw("sp_mb_emp_ot_hours @emp_id, @s_start_date, @s_stop_date, @lang", emp_id, s_start_date, s_stop_date, lang).ToList();
                    data.calendarschedule_list = new List<CalendarSchedule>();
                    foreach (var item in spData)
                    {
                        CalendarSchedule calendarHoliday = new CalendarSchedule();
                        calendarHoliday.startTime = item.startTime;
                        calendarHoliday.endTime = item.endTime;
                        calendarHoliday.title = item.title;
                        calendarHoliday.remark = item.remark;
                        data.calendarschedule_list.Add(calendarHoliday);
                    }
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }
    }
}
