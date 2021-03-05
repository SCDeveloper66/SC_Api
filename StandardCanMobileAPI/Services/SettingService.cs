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
    public class SettingService : ISettingService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;

        public SettingService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _systemLogService = systemLogService;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;


        public async Task<NotiSettingDataViewModel> GetNotiSettingAsync(string language)
        {
            var data = new NotiSettingDataViewModel();
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
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Setting/GetNotiSetting",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");
                    var spData = context.SpMbGetSetting.FromSqlRaw("sp_mb_get_setting @emp_id", emp_id).ToList();
                    foreach (var item in spData)
                    {
                        data.notification = item.notification;
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

        public async Task<ReturnMsgViewModel> NotiSettingAsync(NotiSettingViewModel setting)
        {
            ReturnMsgViewModel data = new ReturnMsgViewModel();
            data.message = new messageModel();
            try
            {
                using (var context = new StandardcanContext())
                {
                    var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    try
                    {
                        var jsonData = JsonConvert.SerializeObject(new
                        {
                            emp_id = userId,
                            noti_status = setting.notification
                        });
                        SystemLog systemLog = new SystemLog()
                        {
                            module = "api/Setting/NotiSetting",
                            data_log = jsonData
                        };
                        await _systemLogService.InsertSystemLogAsync(systemLog);

                        SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                        SqlParameter noti_status = new SqlParameter("noti_status", setting.notification ?? "");
                        SqlParameter lang = new SqlParameter("lang", setting.language ?? "");
                        await context.Database.ExecuteSqlCommandAsync("sp_mb_update_setting @emp_id, @noti_status", emp_id, noti_status);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Noticiation is Error");
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
