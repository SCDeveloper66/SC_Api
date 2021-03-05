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
    public class NotiService : INotiService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;

        public NotiService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _systemLogService = systemLogService;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;

        public async Task<NotiViewModel> GetNotiListAsync(string last_id, string language)
        {
            var data = new NotiViewModel();
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
                        last_id = last_id,
                        emp_id = userId,
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Noti/GetNotiList",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter noti_id = new SqlParameter("last_id", last_id ?? "");
                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbNotiList.FromSqlRaw("sp_mb_noti_list @emp_id, @last_id, @lang", emp_id, noti_id, lang).ToList();
                    data.content = new List<NoticontentViewModel>();
                    foreach (var item in spData)
                    {
                        NoticontentViewModel news = new NoticontentViewModel();
                        news.id = item.id;
                        news.type = item.type;
                        news.url_img = item.url_img;
                        news.title = item.title;
                        news.detail = item.detail;
                        news.time = item.time;
                        data.content.Add(news);
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
