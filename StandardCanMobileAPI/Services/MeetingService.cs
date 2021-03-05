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
    public class MeetingService : IMeetingService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;

        public MeetingService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _systemLogService = systemLogService;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;

        public async Task<MeetingViewModel> GetMeetingAsync(string language)
        {
            var data = new MeetingViewModel();
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
                        type = "1",
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Meeting/GetMeeting",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter typeToday = new SqlParameter("type", "1");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");
                    SqlParameter typeYes = new SqlParameter("type", "2");

                    var spDataToday = context.SpMbMeeting.FromSqlRaw("sp_mb_meeting @emp_id, @type, @lang", emp_id, typeToday, lang).ToList();
                    jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = userId,
                        lang = language,
                        type = "2",
                    });
                    systemLog = new SystemLog()
                    {
                        module = "api/Meeting/GetMeeting",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    var spDataYes = context.SpMbMeeting.FromSqlRaw("sp_mb_meeting @emp_id, @type, @lang", emp_id, typeYes, lang).ToList();

                    data.content = new List<MeetingContentViewModel>();
                    MeetingContentViewModel viewModel = new MeetingContentViewModel();
                    viewModel.title = "รายการนัดหมายปัจจุบัน";
                    viewModel.chlids = new List<MeetingChlidsViewModel>();

                    foreach (var item in spDataToday)
                    {
                        MeetingChlidsViewModel meetingChlids = new MeetingChlidsViewModel();
                        meetingChlids.title = item.title;
                        meetingChlids.line1 = item.line1;
                        meetingChlids.line2 = item.line2;
                        meetingChlids.line3 = item.line3;
                        meetingChlids.icon = item.icon;
                        viewModel.chlids.Add(meetingChlids);
                    }
                    data.content.Add(viewModel);

                    viewModel = new MeetingContentViewModel();
                    viewModel.title = "รายการนัดหมายที่ผ่านมา";
                    viewModel.chlids = new List<MeetingChlidsViewModel>();

                    foreach (var item in spDataYes)
                    {
                        MeetingChlidsViewModel meetingChlids = new MeetingChlidsViewModel();
                        meetingChlids.title = item.title;
                        meetingChlids.line1 = item.line1;
                        meetingChlids.line2 = item.line2;
                        meetingChlids.line3 = item.line3;
                        meetingChlids.icon = item.icon;
                        viewModel.chlids.Add(meetingChlids);
                    }
                    data.content.Add(viewModel);

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
