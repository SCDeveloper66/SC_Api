using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StandardCan.jwt;
using StandardCan.Models;

namespace StandardCan.Service
{
    public class reportSettingService
    {

      

        public IEnumerable<sp_report_setting_search_Result> search(reportSettingModel value)
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
                IEnumerable<sp_report_setting_search_Result> result = context.sp_report_setting_search(value.report, userId).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public carMasterModel master(carModel value)
        {
            carMasterModel result = new carMasterModel();

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

                    string sql = "select		convert(nvarchar(5), MCT_ID) code ";
                    sql += " , MCT_NAME [text] ";
                    sql += " from MAS_CAR_TYPE ";
                    sql += " where MCT_STATUS = 1 ";
                    sql += " order by MCT_NAME ";
                    result.car_type = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select		MCA_NAME code ";
                    sql += " , MCA_NAME [text] ";
                    sql += " from MAS_CAR ";
                    sql += " where MCA_STATUS = 1 ";
                    sql += " order by MCA_NAME ";
                    result.car_license = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select		convert(nvarchar(5), MCR_ID) code ";
                    sql += " , MCR_NAME[text] ";
                    sql += " from MAS_CAR_REASON ";
                    sql += " where MCR_STATUS = 1 ";
                    sql += " order by    MCR_NAME ";
                    result.car_reason = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql += " select convert(nvarchar(5), MAS_ID) code, mas_name [text] ";
                    sql += " from MAS_APPROVE_STATUS ";
                    sql += " where MAS_ID != 0 ";
                    sql += " order by mas_orderby ";
                    result.car_status = context.Database.SqlQuery<dropdown>(sql).ToList();

                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public messageModel insert(carModel value)
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

                    //int ret = context.sp_car_insert(value.car_type, value.name, value.detail, userId, myOutputParamInt);
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

        public messageModel update(reportSettingModel value)
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

                    int ret = context.sp_report_setting_update(value.report, value.name, userId);
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