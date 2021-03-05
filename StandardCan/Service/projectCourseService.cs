using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using StandardCan.jwt;
using StandardCan.Models;
using StandardCan.Models.ViewModels;

namespace StandardCan.Service
{
    public class projectCourseService
    {

        public projectCourseDetailModel detail2(projectCourseModel value)
        {
            projectCourseDetailModel result = new projectCourseDetailModel();

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

                    result.data = context.sp_project_course_detail_v4(value.id).ToList();

                    string sql = "";
                    sql = "select		convert(nvarchar(5), MPJ_ID) code ";
                    sql += " , MPJ_NAME [text] ";
                    sql += " from MAS_PROJECT ";
                    sql += " where MPJ_STATUS = 1 ";
                    sql += " order by MPJ_NAME ";
                    result.project = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select		convert(nvarchar(5), MEP_ID) code ";
                    sql += " , MEP_NAME [text] ";
                    sql += " from MAS_EXPERT ";
                    sql += " where MEP_STATUS = 1 ";
                    sql += " order by MEP_NAME ";
                    result.expert = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select		convert(nvarchar(5), MTD_ID) code ";
                    sql += " , MTD_NAME [text] ";
                    sql += " from MAS_TRAIN_DESTINATION ";
                    sql += " where MTD_STATUS = 1 ";
                    sql += " order by MTD_NAME ";
                    result.location = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select		convert(nvarchar(5), MFL_ID) code ";
                    sql += " , MFL_NAME [text] ";
                    sql += " from MAS_FORMULA ";
                    sql += " where MFL_STATUS = 1 ";
                    sql += " order by MFL_NAME ";
                    result.formula = context.Database.SqlQuery<dropdown>(sql).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public IEnumerable<sp_project_course_detail_v3_Result> detail(projectCourseModel value)
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
                IEnumerable<sp_project_course_detail_v3_Result> result = context.sp_project_course_detail_v3(value.id).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public projectCourseMasterModel master(projectCourseModel value)
        {
            projectCourseMasterModel result = new projectCourseMasterModel();

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
                    string sql = "select convert(nvarchar(4), MPJ_YEAR) code, convert(nvarchar(4), MPJ_YEAR) [text] ";
                    sql += " from MAS_PROJECT ";
                    sql += " where MPJ_YEAR is not null ";
                    sql += " group by    MPJ_YEAR ";
                    result.year = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select		convert(nvarchar(5), MPJ_ID) code ";
                    sql += " , MPJ_NAME [text] ";
                    sql += " from MAS_PROJECT ";
                    sql += " where MPJ_STATUS = 1 ";
                    sql += " order by MPJ_NAME ";
                    result.project = context.Database.SqlQuery<dropdown>(sql).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }


        public IEnumerable<sp_project_course_search_v2_Result> search(projectCourseModel value)
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
                IEnumerable<sp_project_course_search_v2_Result> result = context.sp_project_course_search_v2(value.year_from, value.year_to, value.project_from, value.project_to, value.course_name, value.status_id).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public messageModel save(projectCourseModel value)
        {
            messageModel result = new messageModel();

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

                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));

                using (var context = new StandardCanEntities())
                {
                    var _Gapi = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "API_PATH");
                    var _Gpath = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "FILE_PATH");
                    JsonPathModel jsonPath = new JsonPathModel();
                    if (_Gpath != null)
                    {
                        jsonPath = (JsonPathModel)Newtonsoft.Json.JsonConvert.DeserializeObject(_Gpath.GGC_VAL, typeof(JsonPathModel));
                    }

                    if (!String.IsNullOrEmpty(value.file_base64))
                    {
                        string[] doc = value.file_base64.Split(',');
                        var imgBase64 = doc.Count() > 1 ? doc[1] : doc[0];
                        byte[] imgbyte = Convert.FromBase64String(imgBase64);
                        var guId = Guid.NewGuid().ToString();
                        var fileName = guId + ".PDF";
                        string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.projectCourse),
                                                                   Path.GetFileName(fileName));
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.projectCourse));
                        foreach (FileInfo files in di.GetFiles())
                        {
                            if (files.Name == value.file_name)
                            {
                                files.Delete();
                            }
                        }
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            stream.Write(imgbyte, 0, imgbyte.Length);
                        }
                        value.file_name = fileName;
                    }
                    else if(String.IsNullOrEmpty(value.file_url))
                    {
                        string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.projectCourse));
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.projectCourse));
                        foreach (FileInfo files in di.GetFiles())
                        {
                            if (files.Name == value.file_name)
                            {
                                files.Delete();
                            }
                        }
                        value.file_name = null;
                    }

                    int ret = context.sp_project_course_save_v2(value.id, value.project_id, value.course_id, value.course_name, value.formula_id, value.location_id, value.expert_id, value.file_name, value.file_base64, value.remark, value.score1, value.score2, value.score3, value.score4, value.score5, userId, value.status_id, myOutputParamInt);

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

        public messageModel insert(projectFormularModel value)
        {
            messageModel result = new messageModel();

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

                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));
                using (var context = new StandardCanEntities())
                {
                    int ret = context.sp_formular_insert(value.fml_name, value.fml_type, value.fml_input_type, value.user_id, myOutputParamInt);
                    int countRange = value.formularRange.Count();
                    for (int i = 0; i < countRange; i++)
                    {
                        int ret2 = context.sp_formularrange_insert(ret.ToString(), value.formularRange[i].fml_range_no, value.formularRange[i].fml_range_score, value.formularRange[i].fml_range_display, value.user_id, myOutputParamInt);
                    }

                    int countvalue = value.formularValue.Count();
                    for (int i = 0; i < countvalue; i++)
                    {
                        int ret2 = context.sp_formularvalue_insert(ret.ToString(), value.formularValue[i].fmlv_orderby, value.formularValue[i].fmlv_value, value.formularValue[i].fmlv_text, value.user_id, myOutputParamInt);
                    }
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

        //public messageModel update(formularModel value)
        //{
        //    messageModel result = new messageModel();

        //    try
        //    {
        //        using (var context = new StandardCanEntities())
        //        {
        //            int ret = context.sp_car_update(value.id, value.car_type, value.name, value.detail, value.user_id);
        //        }

        //        result.status = "S";
        //        result.message = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        result.status = "E";
        //        result.message = ex.Message.ToString();
        //    }

        //    return result;
        //}

        public messageModel delete(projectFormularModel value)
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

                    //   int ret = context.sp_form(value.id, value.user_id);
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