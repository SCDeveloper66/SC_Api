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
    public class PayrollService : IPayrollService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;
        public PayrollService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _systemLogService = systemLogService;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;

        public async Task<PayrollViewModel> GetPayrollAsync(string language)
        {
            var data = new PayrollViewModel();
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
                        module = "api/Payroll/GetPayroll",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbSalary.FromSqlRaw("sp_mb_salary @emp_id, @lang", emp_id, lang).ToList();
                    data.content = new List<PayrollContentViewModel>();
                    foreach (var item in spData)
                    {
                        PayrollContentViewModel payroll = new PayrollContentViewModel();
                        payroll.title1 = item.title1;
                        payroll.title2 = item.title2;
                        payroll.detail = item.detail;
                        payroll.link_frame = item.link_frame;
                        data.content.Add(payroll);
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
