using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Web.Script.Serialization;

using StandardCan.Models;
using StandardCan.jwt;
using StandardCan.Models.ViewModels;
using System.Web.Hosting;

namespace StandardCan.Service
{
    public class employeeService
    {
        public messageModel getUploadFile(employeeModel value)
        {
            messageModel result = new messageModel();
            try
            {
                using (StandardCanEntities context = new StandardCanEntities())
                {
                    var _Gapi = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "API_PATH");
                    var _Gpath = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "FILE_PATH");
                    JsonPathModel jsonPath = new JsonPathModel();
                    if (_Gpath != null)
                    {
                        jsonPath = (JsonPathModel)Newtonsoft.Json.JsonConvert.DeserializeObject(_Gpath.GGC_VAL, typeof(JsonPathModel));
                    }
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var empDetail = context.EMP_PROFILE.SingleOrDefault(a => a.emp_code == value.emp_code);
                    if (empDetail != null)
                    {
                        string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.employee),
                                                                        Path.GetFileName(empDetail.emp_image));
                        byte[] bytes = File.ReadAllBytes(path);
                        result.value = Convert.ToBase64String(bytes);
                    }
                    else
                    {
                        throw new Exception("Data not found");
                    }
                }

                result.status = "S";
                result.message = "";
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }
            return result;
        }
        public messageModel uploadFile(employeeModel value)
        {
            messageModel result = new messageModel();
            try
            {
                using (var context = new StandardCanEntities())
                {
                    var dt = DateTime.Now;
                    var _Gapi = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "API_PATH");
                    var _Gpath = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "FILE_PATH");
                    JsonPathModel jsonPath = new JsonPathModel();
                    if (_Gpath != null)
                    {
                        jsonPath = (JsonPathModel)Newtonsoft.Json.JsonConvert.DeserializeObject(_Gpath.GGC_VAL, typeof(JsonPathModel));
                    }
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "Employee_uploadFile",
                        update_date = dt
                    });
                    context.SaveChanges();
                    if (value.img != null)
                    {
                        string[] img = value.img.Split(',');
                        var imgBase64 = img.Count() > 1 ? img[1] : img[0];
                        byte[] imgbyte = Convert.FromBase64String(imgBase64);
                        var guId = Guid.NewGuid().ToString();
                        var fileName = guId + ".JPG";
                        string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.employee),
                                                                   Path.GetFileName(fileName));
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            stream.Write(imgbyte, 0, imgbyte.Length);
                        }
                        var empDetail = context.EMP_PROFILE.SingleOrDefault(a => a.emp_code == value.emp_code);
                        if (empDetail != null)
                        {
                            empDetail.emp_image = fileName;
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Data not found");
                        }
                    }
                }

                result.status = "S";
                result.message = "";

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }
            return result;
        }

        public messageModel tab_behavior_update(employeeModel value)
        {
            messageModel result = new messageModel();

            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);

                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    context.interface_log.Add(new interface_log
                    {
                        module = "tab_behavior_update",
                        data_log = json,
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();
                    context.sp_emp_profile_tabbehavior_update(value.emp_code, value.id, value.date, value.detail, value.score, value.detail_id);
                }

                result.status = "S";
                result.message = "";

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel update_emp_score(employeeModel value)
        {
            messageModel result = new messageModel();

            try
            {
                string emp_name = "";
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);
                int ret = -1;
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    context.interface_log.Add(new interface_log
                    {
                        module = "update_emp_score",
                        data_log = json,
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var emp = context.EMP_PROFILE.Where(p => p.emp_card_no.Equals(value.emp_code)).FirstOrDefault();
                    if (emp == null)
                    {
                        result.status = "E";
                        result.message = "รหัสบัตรไม่ถูกต้อง !";
                    }
                    else
                    {
                        ret = context.sp_update_score_outdoor(value.emp_code, value.date, value.score, value.detail_id, userId);
                        emp_name = (emp.emp_fname ?? "") + " " + emp.emp_lname ?? "";
                    }


                }

                if (ret == -1)
                {
                    result.status = "E";
                    result.message = "รหัสบัตรไม่ถูกต้อง !";
                }
                else
                {
                    result.status = "S";
                    result.message = "บันทึกคะแนนของ " + emp_name + " สำเร็จ";
                }


            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel tab_behavior_delete(employeeModel value)
        {
            messageModel result = new messageModel();

            try
            {

                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);

                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    context.interface_log.Add(new interface_log
                    {
                        module = "tab_behavior_delete",
                        data_log = json,
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();
                    context.sp_emp_profile_tabbehavior_delete(value.emp_code, value.id);
                }

                result.status = "S";
                result.message = "";

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel tab_behavior_export(employeeModel value)
        {
            messageModel result = new messageModel();

            StandardCanEntities context = new StandardCanEntities();
            try
            {
                var _Gapi = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "API_PATH");
                var _Gpath = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "FILE_PATH");
                JsonPathModel jsonPath = new JsonPathModel();
                if (_Gpath != null)
                {
                    jsonPath = (JsonPathModel)Newtonsoft.Json.JsonConvert.DeserializeObject(_Gpath.GGC_VAL, typeof(JsonPathModel));
                }

                if (String.IsNullOrEmpty(value.user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }

                var dataList = context.sp_emp_profile_tabbehavior_search_v2(value.emp_code).ToList();

                if (dataList != null && dataList.Count() > 0)
                {
                    string file_name = string.Format("emp_behavior_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileExport", file_name);

                    string file_path_folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileExport");
                    bool exists = System.IO.Directory.Exists(file_path_folder);

                    if (!exists)
                    {
                        var fileCreate = System.IO.Directory.CreateDirectory(file_path_folder);
                    }

                    FileInfo excelFile = new FileInfo(file_path);
                    using (ExcelPackage excel = new ExcelPackage(excelFile))
                    {
                        string sheetName = "data";
                        int row = 1;
                        ExcelWorksheet wsTemplate = excel.Workbook.Worksheets.Add(sheetName);
                        wsTemplate.Cells[row, 1].Value = "ลำดับ";
                        wsTemplate.Cells[row, 2].Value = "ปี";
                        wsTemplate.Cells[row, 3].Value = "วันที่";
                        wsTemplate.Cells[row, 4].Value = "รายละเอียด";
                        wsTemplate.Cells[row, 5].Value = "คะแนน";

                        row++;
                        foreach (var item in dataList)
                        {
                            wsTemplate.Cells[row, 1].Value = item.no;
                            wsTemplate.Cells[row, 2].Value = item.year;
                            wsTemplate.Cells[row, 3].Value = item.date;
                            wsTemplate.Cells[row, 4].Value = item.detail;
                            wsTemplate.Cells[row, 5].Value = item.score;
                            row++;
                        }

                        wsTemplate.Cells.AutoFitColumns();
                        excel.Save();
                    }

                    result.status = "S";
                    result.message = "Success";
                    result.value = _Gapi.GGC_VAL + jsonPath.fileExport + file_name;

                }
                else
                {
                    result.status = "E";
                    result.message = "not found data!";
                }
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public IEnumerable<sp_emp_profile_tabbehavior_search_v3_Result> tab_behavior_search(employeeModel value)
        {
            try
            {
                StandardCanEntities context = new StandardCanEntities();
                IEnumerable<sp_emp_profile_tabbehavior_search_v3_Result> result = context.sp_emp_profile_tabbehavior_search_v3(value.emp_code).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public messageModel tab_work_update(employeeModel value)
        {
            messageModel result = new messageModel();

            try
            {

                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);

                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    context.interface_log.Add(new interface_log
                    {
                        module = "tab_behavior_update",
                        data_log = json,
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();
                    context.sp_emp_profile_tabwork_update(value.emp_code, value.id, value.date, value.no, value.grade);
                }

                result.status = "S";
                result.message = "";

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel tab_work_delete(employeeModel value)
        {
            messageModel result = new messageModel();

            try
            {

                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    context.sp_emp_profile_tabwork_delete(value.emp_code, value.id);
                }

                result.status = "S";
                result.message = "";

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public IEnumerable<sp_emp_profile_tabwork_search_Result> tab_work_search(employeeModel value)
        {
            try
            {
                StandardCanEntities context = new StandardCanEntities();
                IEnumerable<sp_emp_profile_tabwork_search_Result> result = context.sp_emp_profile_tabwork_search(value.emp_code).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public messageModel tab_card_update(employeeModel value)
        {
            messageModel result = new messageModel();

            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);

                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    context.interface_log.Add(new interface_log
                    {
                        module = "tab_behavior_update",
                        data_log = json,
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();
                    context.sp_emp_profile_tabcard_update(value.emp_code, value.id, value.date, value.card_no, value.status);
                }

                result.status = "S";
                result.message = "";

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public IEnumerable<sp_emp_profile_tabcard_search_Result> tab_card_search(employeeModel value)
        {
            try
            {
                StandardCanEntities context = new StandardCanEntities();
                IEnumerable<sp_emp_profile_tabcard_search_Result> result = context.sp_emp_profile_tabcard_search(value.emp_code).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public messageModel tab_note_update(employeeModel value)
        {
            messageModel result = new messageModel();

            try
            {

                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);

                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    context.interface_log.Add(new interface_log
                    {
                        module = "tab_behavior_update",
                        data_log = json,
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    context.sp_emp_profile_tabnote_update(value.emp_code, value.note);
                }

                result.status = "S";
                result.message = "";

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public empTabNoteModel tab_note_search(employeeModel value)
        {
            empTabNoteModel result = new empTabNoteModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    var note = context.sp_emp_profile_tabnote_search(value.emp_code).FirstOrDefault();
                    result.note = note ?? "";

                }
            }
            catch (Exception ex)
            {
                //result.status = "E";
                //result.message = ex.Message.ToString();
                throw new Exception(ex.Message);
            }

            return result;
        }


        public empTab3Model tab3_search(employeeModel value)
        {
            empTab3Model result = new empTab3Model();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    result.quota = context.sp_emp_profile_tab3_quota(value.emp_code).ToList();
                    result.data = context.sp_emp_profile_tab3(value.emp_code, value.start_date, value.stop_date).ToList();
                }
            }
            catch (Exception ex)
            {
                //result.status = "E";
                //result.message = ex.Message.ToString();
                throw new Exception(ex.Message);
            }

            return result;
        }

        public empTab2Model tab2_search(employeeModel value)
        {
            empTab2Model result = new empTab2Model();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    result.data = context.sp_emp_profile_tab2(value.emp_code, value.start_date, value.stop_date).ToList();

                    if (result.data != null)
                    {
                        result.status1 = result.data.Where(p => p.time_in_status.Equals("ปกติ")).Count().ToString();
                        result.status2 = result.data.Where(p => p.time_in_status.Equals("ขาดงาน")).Count().ToString();
                        result.status3 = result.data.Where(p => p.time_in_status.Equals("สาย")).Count().ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                //result.status = "E";
                //result.message = ex.Message.ToString();
                throw new Exception(ex.Message);
            }

            return result;
        }

        public BenefitViewModel tab_ot_search(employeeModel value)
        {
            var data = new BenefitViewModel();
            data.message = new messageModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    var spData_hours = context.sp_mb_emp_ot_hours(userId, value.start_date, value.stop_date, "").ToList();
                    var spData_quota = context.sp_mb_emp_ot_quota(userId, value.start_date, value.stop_date, "").ToList();

                    data.quota_list = new List<Benefitline>();
                    data.hours_list = new List<Benefitline>();

                    double sumH = 0;
                    foreach (var item in spData_quota)
                    {
                        Benefitline news = new Benefitline();
                        news.line1 = item.ot_text;
                        news.hours = item.ot_hours;
                        data.quota_list.Add(news);
                        sumH += item.total_minute == null ? 0 : Convert.ToInt32(item.total_minute);
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
                        sumH += item.total_minute == null ? 0 : Convert.ToInt32(item.total_minute);
                    }
                    HH = sumH / 60;
                    MM = sumH % 60;
                    var totalH = HH.ToString().Split('.');
                    var totalHM = MM.ToString().Split('.');
                    data.hours = totalH[0] + ":" + totalHM[0];

                    data.message.status = "1";
                    data.message.message = "Success";
                }
            }
            catch (Exception ex)
            {
                //result.status = "E";
                //result.message = ex.Message.ToString();
                throw new Exception(ex.Message);
            }

            return data;
        }

        public IEnumerable<sp_emp_profile_tab1_v2_Result> tab1_search(employeeModel value)
        {
            try
            {
                if (String.IsNullOrEmpty(value.user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                StandardCanEntities context = new StandardCanEntities();
                IEnumerable<sp_emp_profile_tab1_v2_Result> result = context.sp_emp_profile_tab1_v2(value.emp_code).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public empMasterModel master(employeeModel value)
        {
            empMasterModel result = new empMasterModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    result.sh = context.sp_sh_search().ToList();
                    result.depart = context.sp_depart_search().ToList();
                    result.emp_status = context.sp_emp_status_search().ToList();
                }
            }
            catch (Exception ex)
            {
                //result.status = "E";
                //result.message = ex.Message.ToString();
                throw new Exception(ex.Message);
            }

            return result;
        }

        public IEnumerable<sp_emp_search_byid_Result> search_byid(employeeModel value)
        {
            try
            {
                if (String.IsNullOrEmpty(value.user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                StandardCanEntities context = new StandardCanEntities();
                IEnumerable<sp_emp_search_byid_Result> result = context.sp_emp_search_byid(value.emp_code).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<sp_emp_search_by_popup_Result> search_by_popup(employeeModel value)
        {
            try
            {
                if (String.IsNullOrEmpty(value.user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }

                StandardCanEntities context = new StandardCanEntities();
                IEnumerable<sp_emp_search_by_popup_Result> result = context.sp_emp_search_by_popup(value.emp_code, value.emp_name, value.sh_id).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<sp_emp_profile_search_v2_Result> search(employeeModel value)
        {
            try
            {
                if (String.IsNullOrEmpty(value.user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }

                StandardCanEntities context = new StandardCanEntities();
                IEnumerable<sp_emp_profile_search_v2_Result> result = context.sp_emp_profile_search_v2(value.emp_code_start, value.emp_code_stop,
                    value.sh_start, value.sh_stop, value.depart_start, value.depart_stop, value.emp_status_start, value.emp_status_stop,
                    value.emp_fname, value.emp_lname, value.head_fname, value.head_lname).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public messageModel insert(roomModel value)
        {
            messageModel result = new messageModel();

            try
            {

                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    //int ret = context.sp_room_insert(value.code, value.name, userId, myOutputParamInt);
                }

                if (myOutputParamInt.Value != null)
                {
                    int r_id = Convert.ToInt32(myOutputParamInt.Value);
                    result.status = "S";
                    result.message = "";
                    result.value = r_id.ToString();
                }
                else
                {
                    result.status = "E";
                    result.message = "";
                }

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel update(roomModel value)
        {
            messageModel result = new messageModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    //int ret = context.sp_room_update(value.id, value.code, value.name, userId);
                }

                result.status = "S";
                result.message = "";
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel delete(roomModel value)
        {
            messageModel result = new messageModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    int ret = context.sp_room_delete(value.id, userId);
                }

                result.status = "S";
                result.message = "";
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        
    }
}