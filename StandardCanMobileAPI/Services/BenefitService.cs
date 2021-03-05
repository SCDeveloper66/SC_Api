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
    public class BenefitService : IBenefitService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;

        public BenefitService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _systemLogService = systemLogService;
            _configuration = configuration;
            _accessor = httpContextAccessor;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;

        public async Task<BenefitViewModel> GetOTSummaryAsync(string language, DateTime? start_date, DateTime? stop_date)
        {
            var data = new BenefitViewModel();
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
                        s_start_date = start_date,
                        s_stop_date = stop_date
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Benefit/GetOTSummary",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter s_start_date = new SqlParameter("s_start_date", start_date != null ? start_date.Value.ToString("dd/MM/yyyy") : "");
                    SqlParameter s_stop_date = new SqlParameter("s_stop_date", stop_date != null ? stop_date.Value.ToString("dd/MM/yyyy") : "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData_hours = context.SpMbEmpOTHoursResult.FromSqlRaw("sp_mb_emp_ot_hours @emp_id, @s_start_date, @s_stop_date, @lang", emp_id, s_start_date, s_stop_date, lang).ToList();
                    var spData_quota = context.SpMbEmpOTQuotaResult.FromSqlRaw("sp_mb_emp_ot_quota @emp_id, @s_start_date, @s_stop_date, @lang", emp_id, s_start_date, s_stop_date, lang).ToList();

                    data.quota_list = new List<Benefitline>();
                    data.hours_list = new List<Benefitline>();


                    double sumH = 0;
                    foreach (var item in spData_quota)
                    {
                        Benefitline news = new Benefitline();
                        news.line1 = item.ot_text;
                        news.hours = item.ot_hours;
                        data.quota_list.Add(news);
                        sumH += item.total_minute;
                    }
                    var HH = sumH / 60;
                    var MM = sumH % 60;
                    var totalQ = HH.ToString().Split('.');
                    var totalQM = MM.ToString().Split('.');
                    data.quota = totalQ[0] + ":" + totalQM[0];

                    HH = 0;
                    MM = 0;
                    sumH = 0;
                    foreach (var item in spData_hours)
                    {
                        Benefitline news = new Benefitline();
                        news.line1 = item.ot_text;
                        news.hours = item.ot_hours;
                        data.hours_list.Add(news);
                        sumH += item.total_minute;
                    }
                    HH = sumH / 60;
                    MM = sumH % 60;
                    var totalH = HH.ToString().Split('.');
                    var totalHM = MM.ToString().Split('.');
                    data.hours = totalH[0] + ":" + totalHM[0];

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

        public async Task<BenefitLoad> GetBenefitsEmployeeAsync(string language)
        {
            var data = new BenefitLoad();
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
                        module = "api/Benefit/GetBenefitsEmployee",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbEmpBenefits.FromSqlRaw("sp_mb_emp_benefits @emp_id, @lang", emp_id, lang).ToList();
                    data.content = new List<BenefitContent>();
                    BenefitContent benefitContent = new BenefitContent();
                    benefitContent.title = "ข้อมูล ณ วันที่ " + DateTime.Now.ToString("dd/MM/yyyy");
                    benefitContent.chlids = new List<BenefitContentChlids>();
                    foreach (var item in spData.GroupBy(a => a.title))
                    {
                        foreach (var z in item)
                        {
                            BenefitContentChlids chlids = new BenefitContentChlids();
                            chlids.title = z.title;
                            chlids.sum = z.sum;
                            chlids.now = z.now;
                            chlids.used = z.used;
                            chlids.current = z.current;
                            benefitContent.chlids.Add(chlids);
                        }
                    }
                    data.content.Add(benefitContent);
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

        public async Task<BenefitDepartmentViewModel> GetOTDepartmentSummaryAsync(DateTime? start_date, DateTime? stop_date, string empId, string sts_id, string language)
        {
            var data = new BenefitDepartmentViewModel();
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
                        v_userid = userId,
                        v_emp_id = empId,
                        v_sts_id = sts_id,
                        v_start_date = start_date,
                        v_stop_date = stop_date,
                        lang = language,
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Benefit/GetOTDepartmentSummary",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter v_userid = new SqlParameter("v_userid", userId ?? "");
                    SqlParameter emp_id = new SqlParameter("v_emp_id", empId ?? "");
                    SqlParameter v_sts_id = new SqlParameter("v_sts_id", sts_id ?? "");
                    SqlParameter s_start_date = new SqlParameter("v_start_date", start_date != null ? start_date.Value.ToString("dd/MM/yyyy") : "");
                    SqlParameter s_stop_date = new SqlParameter("v_stop_date", stop_date != null ? stop_date.Value.ToString("dd/MM/yyyy") : "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData_hours = context.SpMbDepartOTHours.FromSqlRaw("sp_mb_depart_ot_hours @v_userid, @v_emp_id, @v_start_date, @v_stop_date", v_userid, emp_id, s_start_date, s_stop_date).ToList();
                    var spData_quota = context.SpMbDepartOTQuota.FromSqlRaw("sp_mb_depart_ot_quota @v_userid, @v_emp_id, @v_start_date, @v_stop_date", v_userid, emp_id, s_start_date, s_stop_date).ToList();
                    SqlParameter empIdEmp = new SqlParameter("emp_id", userId ?? "");
                    var spDataEmp = context.SpMbEmpUnder.FromSqlRaw("sp_mb_emp_under @emp_id, @lang", empIdEmp, lang).ToList();
                    var spDataLeave = context.SpMbMasterLeave.FromSqlRaw("sp_mb_master_leave @lang", lang).ToList();


                    data.head = new BenefitDepartmentHeadViewModel();
                    data.head.line1 = "วันที่ :";
                    data.head.line1 += start_date != null ? start_date.Value.ToString("dd/MM/yyyy") : "";
                    data.head.line1 += "-";
                    data.head.line1 += stop_date != null ? stop_date.Value.ToString("dd/MM/yyyy") : "";
                    data.head.line2 = "พนักงาน :";
                    if (String.IsNullOrEmpty(empId))
                    {
                        data.head.line2 += "ทั้งแผนก";
                    }
                    else if (empId == "-1")
                    {
                        data.head.line2 += "ทั้งแผนก";
                    }
                    else
                    {
                        data.head.line2 += spDataEmp != null ? spDataEmp.SingleOrDefault(a => a.emp_id == empId).name : "";
                    }
                    data.head.line3 += "สถานะ :";
                    if (String.IsNullOrEmpty(empId))
                    {
                        data.head.line3 += "ทั้งหมด";
                    }
                    else if (empId == "-1")
                    {
                        data.head.line3 += "ทั้งหมด";
                    }
                    else
                    {
                        data.head.line3 += spDataLeave != null ? spDataLeave.SingleOrDefault(a => a.sts_id == sts_id).name : "";
                    }

                    data.budget = new BenefitDepartmentBudgetViewModel();
                    data.budget.hours = spData_hours.Count().ToString();
                    data.budget.quota = spData_quota.Count().ToString();

                    data.quota_list = new List<BenefitDepartmentListViewModel>();
                    foreach (var item in spData_quota.GroupBy(a => a.title_group))
                    {
                        BenefitDepartmentListViewModel benefit = new BenefitDepartmentListViewModel();
                        benefit.title_group = item.Key;
                        benefit.childs = new List<BenefitDepartmentChildsViewModel>();
                        foreach (var z in item)
                        {
                            BenefitDepartmentChildsViewModel model = new BenefitDepartmentChildsViewModel();
                            model.title = z.title;
                            model.value = z.value != null ? z.value.ToString() : "";
                            model.value_color = z.value_color;
                            benefit.childs.Add(model);
                        }
                        data.quota_list.Add(benefit);
                    }
                    data.hours_list = new List<BenefitDepartmentListViewModel>();
                    foreach (var item in spData_hours.GroupBy(a => a.title_group))
                    {
                        BenefitDepartmentListViewModel benefit = new BenefitDepartmentListViewModel();
                        benefit.title_group = item.Key;
                        benefit.childs = new List<BenefitDepartmentChildsViewModel>();
                        foreach (var z in item)
                        {
                            BenefitDepartmentChildsViewModel model = new BenefitDepartmentChildsViewModel();
                            model.title = z.title;
                            model.value = z.value != null ? z.value.ToString() : "";
                            model.value_color = z.value_color;
                            benefit.childs.Add(model);
                        }
                        data.hours_list.Add(benefit);
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


        public async Task<BenefitMasterLoad> GetBenefitsDepartmentMasterAsync(string language)
        {
            var data = new BenefitMasterLoad();
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
                        module = "api/Benefit/GetBenefitsDepartmentMaster",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spDataEmp = context.SpMbEmpUnder.FromSqlRaw("sp_mb_emp_under @emp_id, @lang", emp_id, lang).ToList();
                    var spDataLeave = context.SpMbMasterLeave.FromSqlRaw("sp_mb_master_leave @lang", lang).ToList();
                    data.emps = new List<BenefitEmpsMasterLoad>();
                    data.status = new List<BenefitStatusMasterLoad>();
                    foreach (var item in spDataEmp)
                    {
                        BenefitEmpsMasterLoad masterLoad = new BenefitEmpsMasterLoad();
                        masterLoad.name = item.name;
                        masterLoad.emp_id = item.emp_id;
                        data.emps.Add(masterLoad);
                    }
                    foreach (var item in spDataLeave)
                    {
                        BenefitStatusMasterLoad masterLoad = new BenefitStatusMasterLoad();
                        masterLoad.name = item.name;
                        masterLoad.sts_id = item.sts_id;
                        data.status.Add(masterLoad);
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

        public async Task<BenefitLeave> GetBenefitsLeaveAsync(DateTime? start_date, DateTime? stop_date, string empId, string sts_id, string language)
        {
            var data = new BenefitLeave();
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
                        v_userid = userId,
                        v_emp_id = empId,
                        v_sts_id = sts_id,
                        v_start_date = start_date,
                        v_stop_date = stop_date,
                        lang = language,
                        emp_id = userId
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Benefit/GetBenefitsLeave",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter v_userid = new SqlParameter("v_userid", userId ?? "");
                    SqlParameter emp_id = new SqlParameter("v_emp_id", empId ?? "");
                    SqlParameter v_sts_id = new SqlParameter("v_sts_id", sts_id ?? "");
                    SqlParameter s_start_date = new SqlParameter("v_start_date", start_date != null ? start_date.Value.ToString("dd/MM/yyyy") : "");
                    SqlParameter s_stop_date = new SqlParameter("v_stop_date", stop_date != null ? stop_date.Value.ToString("dd/MM/yyyy") : "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");
                    var spData = context.SpMbDepartLeave.FromSqlRaw("sp_mb_depart_leave @v_userid, @v_emp_id, @v_start_date, @v_stop_date", v_userid, emp_id, s_start_date, s_stop_date).ToList();
                    SqlParameter empIdEmp = new SqlParameter("emp_id", userId ?? "");
                    var spDataEmp = context.SpMbEmpUnder.FromSqlRaw("sp_mb_emp_under @emp_id, @lang", empIdEmp, lang).ToList();
                    var spDataLeave = context.SpMbMasterLeave.FromSqlRaw("sp_mb_master_leave @lang", lang).ToList();

                    data.head = new BenefitDepartmentHeadViewModel();
                    data.head.line1 = "วันที่ :";
                    data.head.line1 += stop_date != null ? stop_date.Value.ToString("dd/MM/yyyy") : "";
                    data.head.line1 += "-";
                    data.head.line1 += stop_date != null ? stop_date.Value.ToString("dd/MM/yyyy") : "";
                    data.head.line2 = "พนักงาน :";
                    if (String.IsNullOrEmpty(empId))
                    {
                        data.head.line2 += "ทั้งแผนก";
                    }
                    else if (empId == "-1")
                    {
                        data.head.line2 += "ทั้งแผนก";
                    }
                    else
                    {
                        data.head.line2 += spDataEmp != null ? spDataEmp.SingleOrDefault(a => a.emp_id == empId).name : "";
                    }
                    data.head.line3 += "สถานะ :";
                    if (String.IsNullOrEmpty(empId))
                    {
                        data.head.line3 += "ทั้งหมด";
                    }
                    else if (empId == "-1")
                    {
                        data.head.line3 += "ทั้งหมด";
                    }
                    else
                    {
                        data.head.line3 += spDataLeave != null ? spDataLeave.SingleOrDefault(a => a.sts_id == sts_id).name : "";
                    }

                    data.list = new List<BenefitLeaveListViewModel>();
                    foreach (var item in spData.GroupBy(a => a.title_group))
                    {
                        BenefitLeaveListViewModel benefit = new BenefitLeaveListViewModel();
                        benefit.title_group = item.Key.ToString();
                        benefit.childs = new List<BenefitLeaveChildsViewModel>();
                        foreach (var z in item)
                        {
                            BenefitLeaveChildsViewModel model = new BenefitLeaveChildsViewModel();
                            model.title = z.title;
                            model.detail = z.detail;
                            model.appr_status = z.appr_status;
                            model.color = z.color;
                            benefit.childs.Add(model);
                        }
                        data.list.Add(benefit);
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
