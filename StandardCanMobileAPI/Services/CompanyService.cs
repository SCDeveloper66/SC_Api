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
    public class CompanyService : ICompanyService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;

        public CompanyService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _systemLogService = systemLogService;
            _configuration = configuration;
            _accessor = httpContextAccessor;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;


        public async Task<CompanyViewModel> GetBenefitsCompanyAsync(string lastId, string language)
        {
            var data = new CompanyViewModel();
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
                        last_id = lastId,
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Company/GetBenefitsCompany",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter last_id = new SqlParameter("last_id", lastId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbBenefits.FromSqlRaw("sp_mb_benefits @emp_id, @lang", emp_id, lang).ToList();
                    data.content = new List<CompanyContentViewModel>();
                    foreach (var item in spData)
                    {
                        CompanyContentViewModel news = new CompanyContentViewModel();
                        news.id = item.id;
                        news.title = item.title;
                        news.detail_line1 = item.detail_line1;
                        news.detail_line2 = item.detail_line2;
                        news.detail_line3 = item.detail_line3;
                        news.detail_line4 = item.detail_line4;
                        news.link_file = item.link_file;
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

        public async Task<AboutCompanyViewModel> GetAboutCompanyAsync(string language)
        {
            var data = new AboutCompanyViewModel();
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
                        module = "api/Company/GetAboutCompany",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbDepartContact.FromSqlRaw("sp_mb_depart_contact @emp_id, @lang", emp_id, lang).ToList();
                    data.content = new List<AboutCompanyContentViewModel>();
                    foreach (var item in spData)
                    {
                        AboutCompanyContentViewModel news = new AboutCompanyContentViewModel();
                        news.name = item.name;
                        news.tel = item.tel;
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
