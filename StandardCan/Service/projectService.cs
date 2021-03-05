using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StandardCan.jwt;
using StandardCan.Models;

namespace StandardCan.Service
{
    public class projectService
    {

        public IEnumerable<sp_project_detail_Result> detail(projectModel value)
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
                IEnumerable<sp_project_detail_Result> result = context.sp_project_detail(value.prj_id).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<sp_project_search_v2_Result> search(projectModel value)
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
                IEnumerable<sp_project_search_v2_Result> result = context.sp_project_search_v2(value.year_from, value.year_to, value.prj_from, value.prj_to, value.prj_name, value.status_id).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public projectMasterModel master(projectModel value)
        {
            projectMasterModel result = new projectMasterModel();

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

        public messageModel insert(projectModel value)
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
                    int ret = context.sp_project_insert_v3(value.prj_name, value.start_date, value.stop_date, value.prj_detail, value.prj_status, userId, myOutputParamInt);
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

        public messageModel update(projectModel value)
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

                    int ret = context.sp_project_update_v2(value.prj_id, value.prj_name, value.start_date, value.stop_date, value.prj_detail, value.prj_status, userId);
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

        public messageModel delete(carModel value)
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
                    int ret = context.sp_car_delete(value.id, userId);
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