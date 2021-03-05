using Microsoft.AspNetCore.Hosting;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services
{
    public class TimeInOutService : ITimeInOutService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly IHostingEnvironment _environment;
        private readonly ISystemLogService _systemLogService;

        public TimeInOutService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment, ISystemLogService systemLogService)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _environment = environment;
            _systemLogService = systemLogService;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;

        public async Task<TimeInOutViewModel> GetInoutRealtimeAsync(string language)
        {
            var data = new TimeInOutViewModel();
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
                        time_type = "1",
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/TimeInOut/GetInoutRealtime",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter time_typeToday = new SqlParameter("time_type", "1");
                    SqlParameter time_typeYesterday = new SqlParameter("time_type", "2");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spDataToday = context.SpMbEmpInoutRealtime.FromSqlRaw("sp_mb_emp_inout_realtime @emp_id, @time_type", emp_id, time_typeToday).ToList();
                    jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = userId,
                        time_type = "2",
                        lang = language
                    });
                    systemLog = new SystemLog()
                    {
                        module = "api/TimeInOut/GetInoutRealtime",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    var spDataYesterday = context.SpMbEmpInoutRealtime.FromSqlRaw("sp_mb_emp_inout_realtime @emp_id, @time_type", emp_id, time_typeYesterday).ToList();
                    data.content = new List<TimeInOutContentViewModel>();
                    TimeInOutContentViewModel today = new TimeInOutContentViewModel();
                    today.title = "วันนี้";
                    today.childs = new List<TimeInOutContentchildsViewModel>();
                    foreach (var item in spDataToday)
                    {
                        TimeInOutContentchildsViewModel timeIn = new TimeInOutContentchildsViewModel();
                        timeIn.title = item.title;
                        timeIn.detail = item.detail;
                        today.childs.Add(timeIn);
                    }
                    data.content.Add(today);
                    TimeInOutContentViewModel yesterday = new TimeInOutContentViewModel();
                    yesterday.title = "วันที่ผ่านมา";
                    yesterday.childs = new List<TimeInOutContentchildsViewModel>();
                    foreach (var item in spDataYesterday)
                    {
                        TimeInOutContentchildsViewModel timeIn = new TimeInOutContentchildsViewModel();
                        timeIn.title = item.title;
                        timeIn.detail = item.detail;
                        yesterday.childs.Add(timeIn);
                    }
                    data.content.Add(yesterday);

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

        public async Task<InoutEmpRealtimeViewModel> GetInoutEmpRealtimeAsync(string language)
        {
            var data = new InoutEmpRealtimeViewModel();
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
                        module = "api/TimeInOut/GetInoutEmpRealtime",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbEmpUnder.FromSqlRaw("sp_mb_emp_under @emp_id, @lang", emp_id, lang).ToList();
                    data.emps = new List<InoutEmpRealtimeEmpViewModel>();

                    foreach (var item in spData)
                    {
                        InoutEmpRealtimeEmpViewModel dataItem = new InoutEmpRealtimeEmpViewModel();
                        dataItem.name = item.name;
                        dataItem.emp_id = item.emp_id;
                        data.emps.Add(dataItem);
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


        public async Task<InoutEmpRealtimeSearchViewModel> GetInoutEmpRealtimeSearchAsync(string language)
        {
            var data = new InoutEmpRealtimeSearchViewModel();
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
                        time_type = "1",
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/TimeInOut/GetInoutEmpRealtimeSearch",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("v_userid", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");
                    SqlParameter time_typeToday = new SqlParameter("time_type", "1");
                    SqlParameter time_typeYesterDay = new SqlParameter("time_type", "2");
                    var spDataToday = context.SpMbDepartInoutRealtime.FromSqlRaw("sp_mb_depart_inout_realtime @v_userid, @time_type", emp_id, time_typeToday).ToList();
                    jsonData = JsonConvert.SerializeObject(new
                    {
                        v_userid = userId,
                        time_type = "2",
                        lang = language
                    });
                    systemLog = new SystemLog()
                    {
                        module = "api/TimeInOut/GetInoutEmpRealtimeSearch",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    var spDataYesterDay = context.SpMbDepartInoutRealtime.FromSqlRaw("sp_mb_depart_inout_realtime @v_userid, @time_type", emp_id, time_typeYesterDay).ToList();
                    data.content = new List<InoutEmpRealtimeEmpContentViewModel>();
                    InoutEmpRealtimeEmpContentViewModel contentViewModel = new InoutEmpRealtimeEmpContentViewModel();
                    contentViewModel.title = "วันนี้";
                    contentViewModel.childs = new List<InoutEmpRealtimeEmpChildsViewModel>();
                    foreach (var item in spDataToday)
                    {
                        InoutEmpRealtimeEmpChildsViewModel childsViewModel = new InoutEmpRealtimeEmpChildsViewModel();
                        childsViewModel.name = item.name;
                        childsViewModel.detail = item.detail;
                        childsViewModel.time = item.time;
                        contentViewModel.childs.Add(childsViewModel);
                    }
                    data.content.Add(contentViewModel);
                    contentViewModel = new InoutEmpRealtimeEmpContentViewModel();
                    contentViewModel.title = "วันที่ผ่านมา";
                    contentViewModel.childs = new List<InoutEmpRealtimeEmpChildsViewModel>();
                    foreach (var item in spDataYesterDay)
                    {
                        InoutEmpRealtimeEmpChildsViewModel childsViewModel = new InoutEmpRealtimeEmpChildsViewModel();
                        childsViewModel.name = item.name;
                        childsViewModel.detail = item.detail;
                        childsViewModel.time = item.time;
                        contentViewModel.childs.Add(childsViewModel);
                    }
                    data.content.Add(contentViewModel);

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

        //public async Task<ReturnMsgViewModel> CheckinOutdoorAsync(CheckinOutdoorViewModel dataCheckin)
        //{
        //    ReturnMsgViewModel data = new ReturnMsgViewModel();
        //    data.message = new messageModel();
        //    try
        //    {
        //        var dt = DateTime.Now;
        //        var userId = JwtHelper.GetUserIdFromToken(HttpContext);
        //        if (String.IsNullOrEmpty(userId))
        //        {
        //            throw new Exception("Unauthorized Access");
        //        }
        //        using (var context = new StandardcanContext())
        //        {
        //            var jsonData = JsonConvert.SerializeObject(new
        //            {
        //                TarDate = dt.ToString("dd/MM/yyyy"),
        //                TarTime = dt.ToString("dd/MM/yyyy HH:mm:ss"),
        //                TarType = 2,
        //                EmpId = userId,
        //                MLat = dataCheckin.lat,
        //                MLong = dataCheckin.lng,
        //                Remark = dataCheckin.remark,
        //                Img = dataCheckin.img
        //            });
        //            SystemLog systemLog = new SystemLog()
        //            {
        //                module = "api/TimeInOut/CheckinOutdoor",
        //                data_log = jsonData
        //            };
        //            await _systemLogService.InsertSystemLogAsync(systemLog);


        //            TimeAttRealtime timeAttRealtime = new TimeAttRealtime();
        //            timeAttRealtime.TarDate = dt;
        //            timeAttRealtime.TarTime = dt;
        //            timeAttRealtime.TarType = 2;
        //            timeAttRealtime.EmpId = Convert.ToInt32(userId);
        //            timeAttRealtime.MLat = dataCheckin.lat;
        //            timeAttRealtime.MLong = dataCheckin.lng;
        //            timeAttRealtime.Remark = dataCheckin.remark;
        //            context.TimeAttRealtime.Add(timeAttRealtime);
        //            await context.SaveChangesAsync();
        //            if (timeAttRealtime.TarId != 0)
        //            {
        //                if (dataCheckin.img != null)
        //                {
        //                    foreach (var item in dataCheckin.img)
        //                    {
        //                        var intIdt = context.TimeAttImage.DefaultIfEmpty().Max(r => r == null ? 1 : r.TariId);
        //                        TimeAttImage timeImg = new TimeAttImage();
        //                        timeImg.TarId = timeAttRealtime.TarId;

        //                        string[] img = item.base64.Split(',');
        //                        var imgBase64 = img.Count() > 1 ? img[1] : img[0];
        //                        byte[] imgbyte = Convert.FromBase64String(imgBase64);
        //                        var uniqueFileName = "TimeAtt_" + timeAttRealtime.TarId + "_" + intIdt + ".JPG";
        //                        string filePath = Path.Combine(_environment.ContentRootPath + "\\images\\TimeAttReal\\", uniqueFileName);
        //                        var uploads = Path.Combine(_environment.ContentRootPath + "\\images\\TimeAttReal");
        //                        if (!Directory.Exists(uploads))
        //                        {
        //                            Directory.CreateDirectory(uploads);
        //                        }

        //                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        //                        {
        //                            using (BinaryWriter bw = new BinaryWriter(fs))
        //                            {
        //                                bw.Write(imgbyte);
        //                                fs.Flush(true);
        //                            }
        //                        }

        //                        timeImg.TariImage = uniqueFileName;
        //                        timeImg.TariUpdateDate = dt;
        //                        context.TimeAttImage.Add(timeImg);
        //                        await context.SaveChangesAsync();
        //                    }
        //                }
        //            }
        //            data.message.status = "1";
        //            data.message.msg = "Success";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data.message.status = "2";
        //        data.message.msg = ex.Message;
        //    }
        //    return data;
        //}

        public async Task<ReturnMsgViewModel> CheckinOutdoorAsync(CheckinOutdoorViewModel dataCheckin)
        {
            ReturnMsgViewModel data = new ReturnMsgViewModel();
            data.message = new messageModel();
            try
            {
                var dt = DateTime.Now;
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                string stime = dt.ToString("dd/MM/yyyy HH:mm:ss");
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        TarDate = dt.ToString("dd/MM/yyyy"),
                        TarTime = dt.ToString("dd/MM/yyyy HH:mm:ss"),
                        TarType = 2,
                        EmpId = userId,
                        MLat = dataCheckin.lat,
                        MLong = dataCheckin.lng,
                        Remark = dataCheckin.remark,
                        Img = dataCheckin.img
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/TimeInOut/CheckinOutdoor",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);


                    TimeAttRealtime timeAttRealtime = new TimeAttRealtime();
                    timeAttRealtime.TarDate = dt;
                    timeAttRealtime.TarTime = dt;
                    timeAttRealtime.TarType = 2;
                    timeAttRealtime.EmpId = Convert.ToInt32(userId);
                    timeAttRealtime.MLat = dataCheckin.lat;
                    timeAttRealtime.MLong = dataCheckin.lng;
                    timeAttRealtime.Remark = dataCheckin.remark;
                    context.TimeAttRealtime.Add(timeAttRealtime);
                    await context.SaveChangesAsync();
                    if (timeAttRealtime.TarId != 0)
                    {
                        if (dataCheckin.img != null)
                        {
                            foreach (var item in dataCheckin.img)
                            {
                                var intIdt = context.TimeAttImage.DefaultIfEmpty().Max(r => r == null ? 1 : r.TariId);
                                TimeAttImage timeImg = new TimeAttImage();
                                timeImg.TarId = timeAttRealtime.TarId;

                                string[] img = item.base64.Split(',');
                                var imgBase64 = img.Count() > 1 ? img[1] : img[0];
                                byte[] imgbyte = Convert.FromBase64String(imgBase64);
                                var uniqueFileName = "TimeAtt_" + timeAttRealtime.TarId + "_" + intIdt + ".JPG";
                                // string filePath = Path.Combine(_environment.ContentRootPath + "\\images\\TimeAttReal\\", uniqueFileName);
                                //var uploads = Path.Combine(_environment.ContentRootPath + "\\images\\TimeAttReal");

                                string root = @"D:\SmartCard\API";

                                string filePath = Path.Combine(root + "\\images\\TimeAttReal\\", uniqueFileName);
                                var uploads = Path.Combine(root + "\\images\\TimeAttReal");

                                if (!Directory.Exists(uploads))
                                {
                                    Directory.CreateDirectory(uploads);
                                }

                                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                                {
                                    using (BinaryWriter bw = new BinaryWriter(fs))
                                    {
                                        bw.Write(imgbyte);
                                        fs.Flush(true);
                                    }
                                }

                                timeImg.TariImage = uniqueFileName;
                                timeImg.TariUpdateDate = dt;
                                context.TimeAttImage.Add(timeImg);
                                await context.SaveChangesAsync();
                            }
                        }
                    }
                    data.message.status = "1";
                    data.message.msg = "ลงเวลาสำเร็จ : " + stime;
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<ReturnMsgViewModel> CheckInTimeAsync(CheckInTimeViewModel dataCheckin)
        {
            ReturnMsgViewModel data = new ReturnMsgViewModel();
            data.message = new messageModel();
            try
            {
                var dt = DateTime.Now;
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
                        qrcode = dataCheckin.qrcode,
                        lang = dataCheckin.language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/TimeInOut/CheckInTime",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter qrcode = new SqlParameter("qrcode", dataCheckin.qrcode ?? "");
                    SqlParameter lang = new SqlParameter("lang", dataCheckin.language ?? "");
                    //SqlParameter newPass = new SqlParameter("new_pass", newPassEncrypt ?? "");
                    var spData = context.SpMbMeetingCheckin.FromSqlRaw("sp_mb_meeting_checkin @emp_id, @qrcode, @lang", emp_id, qrcode, lang).ToList();
                    foreach(var item in spData)
                    {
                        data.message.status = item.status;
                        data.message.msg = item.msg;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<SummaryTimeViewModel> GetSummaryTimeAsync(string language, string type, string year, string month, DateTime? start, DateTime? stop)
        {
            var data = new SummaryTimeViewModel();
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
                        userId = userId,
                        lang = language,
                        type = type,
                        year = year,
                        month = month,
                        start = start,
                        stop = stop
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/TimeInOut/GetSummaryTime",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    var date_from = start != null ? start.Value.ToString("dd/MM/yyyy") : "";
                    var date_to = stop != null ? stop.Value.ToString("dd/MM/yyyy") : "";

                    SqlParameter emp_id = new SqlParameter("v_emp_id", userId ?? "");
                    SqlParameter v_year = new SqlParameter("v_year", year ?? "");
                    SqlParameter v_month = new SqlParameter("v_month", month ?? "");
                    SqlParameter v_start_date = new SqlParameter("v_start_date", date_from);
                    SqlParameter v_stop_date = new SqlParameter("v_stop_date", date_to);
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbEmpWorkSummary.FromSqlRaw("sp_mb_emp_work_summary @v_emp_id, @v_year, @v_month, @v_start_date, @v_stop_date", emp_id, v_year, v_month, v_start_date, v_stop_date).ToList();

                    data.head = new SummaryTimeHeadViewModel();
                    data.head.line1 = "ปี: " + (year ?? "-");
                    data.head.line2 = "เดือน " + (month ?? "-");
                    data.head.line3 = "วันที่ " + date_from + "-" + date_to;

                    data.budget = new SummaryTimeBudgetViewModel();
                    data.budget.All = spData.Count().ToString();
                    data.budget.Default = spData.Where(a => a.s_status == "ปกติ").ToList().Count().ToString();
                    data.budget.Late = spData.Where(a => a.s_status == "สาย").ToList().Count().ToString();
                    data.budget.Leave = spData.Where(a => a.s_status == "ขาด").ToList().Count().ToString();
                    data.budget.La = spData.Where(a => a.s_status == "ลา").ToList().Count().ToString();

                    data.list = new List<SummaryTimeListViewModel>();
                    foreach(var item in spData)
                    {
                        data.list.Add(new SummaryTimeListViewModel
                        {
                            line1 = "วันที่ " + (item.s_date ?? "-"),
                            line2 = item.s_reason,
                            sts_color = item.s_color,
                            sts_text = item.s_status
                        });
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


        public async Task<SummaryTimeFilterViewModel> GetSummaryTimeFilterAsync(string language)
        {
            var data = new SummaryTimeFilterViewModel();
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
                        userId = userId,
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/TimeInOut/GetSummaryTimeFilter",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    data.years = new List<SummaryTimeFilterYearViewModel>();
                    data.years.Add(new SummaryTimeFilterYearViewModel
                    {
                        id = DateTime.Now.ToString("yyyy"),
                        name = DateTime.Now.ToString("yyyy")
                    });
                    data.years.Add(new SummaryTimeFilterYearViewModel
                    {
                        id = DateTime.Now.AddYears(-1).ToString("yyyy"),
                        name = DateTime.Now.AddYears(-1).ToString("yyyy")
                    });

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
