using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using StandardCan.jwt;
using StandardCan.Models; 

namespace StandardCan.Service
{
    public class empCheckInService
    {
        public timeAttRealtimeMasterModel master(empCheckInModel value)
        {
            timeAttRealtimeMasterModel result = new timeAttRealtimeMasterModel();

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

                    string sql = "select		convert(nvarchar(5), MD_ID) code ";
                    sql += " , md_name [text] ";
                    sql += " from MAS_DEPARTMENT ";
                    sql += " where md_status = 1 ";
                    sql += " order by md_name ";
                    result.department = context.Database.SqlQuery<dropdown>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public IEnumerable<sp_emp_temporary_search_v2_Result> search_Temporary(empCheckInModel value)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);
                StandardCanEntities context = new StandardCanEntities();
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
                    ID = 1,
                    data_log = json,
                    module = "search_Temporary",
                    update_date = DateTime.Now
                });

                context.SaveChanges();

                IEnumerable<sp_emp_temporary_search_v2_Result> result = context.sp_emp_temporary_search_v2(userId, value.start_date, value.stop_date, value.emp_code_from, value.emp_code_to, value.depart_from, value.depart_to, value.fname, value.lname).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public IEnumerable<sp_emp_regular_search_v2_Result> search_Regular(empCheckInModel value)
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
                IEnumerable<sp_emp_regular_search_v2_Result> result = context.sp_emp_regular_search_v2(value.user_id).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }


        public IEnumerable<sp_car_search_v2_Result> search(carModel value)
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
                IEnumerable<sp_car_search_v2_Result> result = context.sp_car_search_v2(value.car_type_from, value.car_type_to, value.car_from, value.car_to).AsEnumerable();
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

                    //int ret = context.sp_car_insert(value.car_type, value.name, value.detail, value.user_id, myOutputParamInt);
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

        public messageModel save_Regular(empCheckInModel value)
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
                        ID = 1,
                        data_log = json,
                        module = "save_Regular",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    int ret = context.sp_emp_regular_delete(userId);

                    if (value.data != null)
                    {
                        foreach(var item in value.data)
                        {
                            context.sp_emp_regular_insert(userId, item.emp_code);
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

        public messageModel save_Temporary(empCheckInModel value)
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

                    if (value.data != null)
                    {
                        foreach (var item in value.data)
                        {
                            context.sp_emp_temporary_save(userId, item.id, item.emp_code, item.start_date, item.stop_date);
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

        public messageModel update(carModel value)
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

                    //int ret = context.sp_car_update(value.id, value.car_type, value.name, value.detail, value.user_id);
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

        public messageModel delete_temporary(empCheckInModel value)
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

                    int ret = context.sp_emp_temporary_delete(value.id, userId);
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