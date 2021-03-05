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
    public class TraningService : ITraningService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;

        public TraningService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _systemLogService = systemLogService;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;


        public async Task<TraningViewModel> GetTraningConditionAsync(string language)
        {
            var data = new TraningViewModel();
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
                        v_year = "",
                        v_prj_id = "",
                        v_lot_id = "",
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Traning/GetTraningCondition",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");
                    SqlParameter v_year = new SqlParameter("v_year", "");
                    SqlParameter v_prj_id = new SqlParameter("v_prj_id", "");
                    SqlParameter v_lot_id = new SqlParameter("v_lot_id", "");

                    var spDataYear = context.SpMbTrainingYear.FromSqlRaw("sp_mb_training_year @emp_id, @lang", emp_id, lang).ToList();
                    var spDataProject = context.SpMbTrainingProject.FromSqlRaw("sp_mb_training_project @emp_id, @v_year, @v_prj_id, @lang", emp_id, v_year, v_prj_id, lang).ToList();
                    var spDataLot = context.SpMbTrainingLot.FromSqlRaw("sp_mb_training_lot @emp_id, @v_prj_id, @v_lot_id, @lang", emp_id, v_prj_id, v_lot_id, lang).ToList();

                    data.year = new List<TraningYearViewModel>();
                    data.project = new List<TraningProjectViewModel>();
                    data.lot = new List<TraningLotViewModel>();
                    foreach(var item in spDataYear)
                    {
                        TraningYearViewModel traning = new TraningYearViewModel();
                        traning.year_id = item.year_id;
                        traning.year_text = item.year_id;
                        data.year.Add(traning);
                    }
                    foreach (var item in spDataProject)
                    {
                        TraningProjectViewModel traning = new TraningProjectViewModel();
                        traning.id = item.id;
                        traning.name = item.name;
                        traning.year_id = item.year_id;
                        data.project.Add(traning);
                    }
                    foreach (var item in spDataLot)
                    {
                        TraningLotViewModel traning = new TraningLotViewModel();
                        traning.id = item.id;
                        traning.name = item.name;
                        traning.prj_id = item.prj_id;
                        data.lot.Add(traning);
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


        public async Task<TraningDetailViewModel> GetTraningDetailAsync(string year, string project_id, string lot_id, string language)
        {
            var data = new TraningDetailViewModel();
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
                        v_year = "",
                        v_prj_id = project_id,
                        v_lot_id = lot_id,
                        v_prj_lot = lot_id,
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Traning/GetTraningDetail",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");
                    SqlParameter v_year = new SqlParameter("v_year", year ?? "");
                    SqlParameter v_prj_id = new SqlParameter("v_prj_id", project_id ?? "");
                    SqlParameter v_prj_lot = new SqlParameter("v_prj_lot", lot_id ?? "");
                    SqlParameter v_lot_id = new SqlParameter("v_lot_id", lot_id ?? "");

                    var r_year = new SqlParameter
                    {
                        ParameterName = "r_year",
                        DbType = System.Data.DbType.String,
                        Size = Int32.MaxValue,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    var r_prj_id = new SqlParameter
                    {
                        ParameterName = "r_prj_id",
                        DbType = System.Data.DbType.Int64,
                        Size = Int32.MaxValue,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    var r_prj_name = new SqlParameter
                    {
                        ParameterName = "r_prj_name",
                        DbType = System.Data.DbType.String,
                        Size = Int32.MaxValue,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    var r_lot_id = new SqlParameter
                    {
                        ParameterName = "r_lot_id",
                        DbType = System.Data.DbType.Int64,
                        Size = Int32.MaxValue,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    var r_lot_name = new SqlParameter
                    {
                        ParameterName = "r_lot_name",
                        DbType = System.Data.DbType.String,
                        Size = Int32.MaxValue,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    var result = context.Database.ExecuteSqlCommand("sp_mb_training_line @emp_id, @v_year, @v_prj_id, @v_lot_id, @lang, @r_year OUT, @r_prj_id OUT, @r_prj_name OUT, @r_lot_id OUT, @r_lot_name OUT", emp_id, v_year, v_prj_id, v_lot_id, lang, r_year, r_prj_id, r_prj_name, r_lot_id, r_lot_name);

                    var spDataDetail = context.SpMbTrainingDetail.FromSqlRaw("sp_mb_training_detail @emp_id, @v_year, @v_prj_id, @v_prj_lot, @lang", emp_id, v_year, v_prj_id, v_prj_lot, lang).ToList();
                    var spDataProject = context.SpMbTrainingProject.FromSqlRaw("sp_mb_training_project @emp_id, @v_year, @v_prj_id, @lang", emp_id, v_year, v_prj_id, lang).ToList();

                    data.line1 = "ปีทีเริ่มโครงการ " + r_year.Value.ToString();
                    data.line2 = r_prj_name.Value.ToString();
                    data.line3 = r_lot_name.Value.ToString();

                    data.list = new List<TraninglistViewModel>();
                    foreach (var item in spDataDetail)
                    {
                        TraninglistViewModel traning = new TraninglistViewModel();
                        traning.name = item.name;
                        traning.detail = item.detail;
                        traning.no = item.no;
                        traning.icon_name = item.icon_name;
                        traning.icon_color = item.icon_color;
                        data.list.Add(traning);
                    }
                    data.budget = new List<TraningBudgetViewModel>();
                    TraningBudgetViewModel traningBudget = new TraningBudgetViewModel();
                    traningBudget.all = spDataDetail.Count().ToString();
                    traningBudget.pass = spDataDetail.Where(a => a.status.Equals("1")).Count().ToString();
                    traningBudget.reject = spDataDetail.Where(a => a.status.Equals("2")).Count().ToString();
                    traningBudget.wait = spDataDetail.Where(a => a.status.Equals("3")).Count().ToString();
                    data.budget.Add(traningBudget);
                   
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
