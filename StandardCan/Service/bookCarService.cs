using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using StandardCan.jwt;
using StandardCan.Models;

namespace StandardCan.Service
{
    public class bookCarService
    {

        public bookCarDetailModel detail(bookCarModel value)
        {
            bookCarDetailModel result = new bookCarDetailModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    var userGroup = JwtHelper.GetUserGroupFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    if (userGroup == "1" || userGroup == "2" || userGroup == "4")
                    {
                        result.button.reject = false;
                        result.button.approve = false;
                        result.button.cancel = true;
                        result.button.save_draft = true;
                        result.button.save_send = true;
                    }
                    else if (userGroup == "3")
                    {
                        result.button.reject = true;
                        result.button.approve = true;
                        result.button.cancel = true;
                        result.button.save_revise = true;
                        result.button.save_draft = true;
                        result.button.save_send = false;
                    }

                    //if (userGroup == "1")
                    //{
                    //    result.button.reject = false;
                    //    result.button.approve = false;
                    //    result.button.cancel = true;
                    //    result.button.save_draft = true;
                    //    result.button.save_send = true;
                    //}
                    //else if (userGroup == "2")
                    //{
                    //    result.button.reject = true;
                    //    result.button.approve = true;
                    //    result.button.cancel = false;
                    //    result.button.save_draft = false;
                    //    result.button.save_send = false;
                    //}


                    string sql = "";

                    sql = "select		convert(nvarchar(5), MCT_ID) code ";
                    sql += " , MCT_NAME [text] ";
                    sql += " from MAS_CAR_TYPE ";
                    sql += " where MCT_STATUS = 1 ";
                    sql += " order by MCT_NAME ";
                    result.car_type = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select		convert(nvarchar(5), MCA_ID) code ";
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

                    sql = "select		convert(nvarchar(5), MDT_ID) code ";
                    sql += " , MDT_NAME [text] ";
                    sql += " from MAS_DESTINATION ";
                    sql += " where MDT_STATUS = 1 ";
                    sql += " order by    MDT_NAME ";
                    result.car_dest = context.Database.SqlQuery<dropdown>(sql).ToList();

                    if (string.IsNullOrEmpty(value.id))
                    {
                        result.button.reject = false;
                        result.button.cancel = false;
                        result.button.save_revise = false;
                    }
                    else
                    {

                        result.emp_list = context.sp_bookcar_emp(value.id).ToList();

                        var h = context.BOOK_CAR.Where(p => p.BC_ID.ToString().Equals(value.id)).FirstOrDefault();
                        if (h != null)
                        {
                            result.id = h.BC_ID.ToString();
                            result.topic = h.bc_topic ?? "";
                            result.start_date = h.bc_start_date == null ? "" : Convert.ToDateTime(h.bc_start_date).ToString("dd/MM/yyyy");
                            result.start_time = h.bc_start_time ?? "";
                            result.stop_date = h.bc_stop_date == null ? "" : Convert.ToDateTime(h.bc_stop_date).ToString("dd/MM/yyyy");
                            result.stop_time = h.bc_stop_time ?? "";
                            result.person_total = h.bc_person_total == null ? "" : h.bc_person_total.ToString();
                            result.car_type_id = h.MCT_ID == null ? "" : h.MCT_ID.ToString();
                            result.car_id = h.MCA_ID == null ? "" : h.MCA_ID.ToString();
                            result.reason_id = h.MCR_ID == null ? "" : h.MCR_ID.ToString();
                            result.dest_id = h.MDT_ID == null ? "" : h.MDT_ID.ToString();
                            result.remark = h.bc_remark ?? "";

                            if (h.bc_request_by != null)
                            {
                                result.bc_request = h.bc_request_by.ToString();
                                var emp_request = context.EMP_PROFILE.SingleOrDefault(a => a.emp_code == h.bc_request_by.ToString());
                                if (emp_request != null)
                                {
                                    result.bc_request_name = emp_request.emp_fname + " " + emp_request.emp_lname;
                                }
                            }

                            if (h.bc_status == 1)
                            {
                                result.read_only = true;
                                result.button.reject = false;
                                result.button.approve = false;
                                result.button.save_revise = false;
                                result.button.cancel = false;
                                result.button.save_draft = false;
                                result.button.save_send = false;

                                if (userGroup == "3")
                                {
                                    result.button.reject = true;
                                    result.button.approve = true;
                                    result.button.save_draft = true;
                                    result.button.cancel = false;
                                    result.button.save_revise = false;
                                    result.button.save_send = false;
                                    result.read_only = false;
                                }

                                if (h.bc_create_by.ToString() == userId)
                                {
                                    result.read_only = false;
                                    result.button.save_draft = true;
                                    if (userGroup != "3")
                                    {
                                        result.button.cancel = true;
                                        result.button.save_send = true;
                                    }
                                }
                            }
                            else if (h.bc_status == 2)
                            {
                                result.read_only = true;

                                result.button.cancel = false;
                                result.button.save_draft = false;
                                result.button.save_send = false;
                                if (userGroup == "3")
                                {
                                    result.button.save_revise = true;
                                    result.read_only = false;
                                }
                                else
                                {
                                    result.button.save_revise = false;
                                }
                                if (h.bc_create_by.ToString() == userId)
                                {
                                    result.button.cancel = true;
                                }
                               
                            }
                            else if (h.bc_status == 0 || h.bc_status == 3 || h.bc_status == 4 || h.bc_status == 5)
                            {
                                result.button.reject = false;
                                result.button.approve = false;
                                result.button.cancel = false;
                                result.button.save_draft = false;
                                result.button.save_send = false;
                                if (userGroup == "3")
                                {
                                    result.button.save_revise = true;
                                }
                                else
                                {
                                    result.button.save_revise = false;
                                }
                                //if (h.bc_status == 3)
                                //{
                                //    result.button.gen_qrCode = true;
                                //}
                                result.read_only = true;
                            }
                            else if (h.bc_status == 6)
                            {
                                var emp_sh_request = context.EMP_PROFILE.SingleOrDefault(a => a.EMP_ID == h.bc_appr_by);
                                if (emp_sh_request != null)
                                {
                                    if (h.bc_appr_by.ToString() == userId)
                                    {
                                        result.button.reject = true;
                                        result.button.approve = true;
                                    }
                                    else
                                    {
                                        result.button.reject = false;
                                        result.button.approve = false;
                                    }
                                }
                                else
                                {
                                    result.button.reject = false;
                                    result.button.approve = false;
                                }
                                result.button.cancel = false;
                                result.button.save_revise = false;
                                result.button.save_draft = false;
                                result.button.save_send = false;
                                result.read_only = true;
                            }
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public bookRoomMasterModel master(bookRoomModel value)
        {
            try
            {
                bookRoomMasterModel result = new bookRoomMasterModel();

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

                    string sql = "select		convert(nvarchar(5), mrm_id) code ";
                    sql += " , mrm_name [text] ";
                    sql += " from MAS_ROOM ";
                    sql += " order by mrm_name ";
                    result.room = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = " select convert(nvarchar(5), MAS_ID) code, mas_name [text] ";
                    sql += " from MAS_APPROVE_STATUS ";
                    sql += " where MAS_ID != 0 ";
                    sql += " order by mas_orderby ";
                    result.status = context.Database.SqlQuery<dropdown>(sql).ToList();

                }

                return result;
            }
            catch (Exception ex)
            {
                //result.status = "E";
                //result.message = ex.Message.ToString();
                throw new Exception(ex.Message);
            }

        }

        public IEnumerable<sp_bookcar_search_v3_Result> search(bookCarModel value)
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
                IEnumerable<sp_bookcar_search_v3_Result> result = context.sp_bookcar_search_v3(value.car_type_from, value.car_type_to, value.car_license_from, value.car_license_to, value.date_from, value.date_to, value.car_reason_from, value.car_reason_to, value.status_from, value.status_to, userId).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public messageModel save_status(bookCarDetailModel value)
        {
            messageModel result = new messageModel();

            try
            {

                //System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));
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

                    string save_type = "";
                    if (value.method.Equals("save_cancel"))
                        save_type = "0";
                    else if (value.method.Equals("save_approve"))
                        save_type = "3";
                    else if (value.method.Equals("save_reject"))
                        save_type = "4";
                    else if (value.method.Equals("save_approve_admin"))
                        save_type = "6";
                    else if (value.method.Equals("save_approve_sh"))
                        save_type = "7";

                    if (save_type == "3" || save_type == "6" || save_type == "7")
                    {
                        var allCarList = context.BOOK_CAR.ToList();
                        var bookingDeatil = allCarList.SingleOrDefault(a => a.BC_ID.ToString() == value.id);
                        if (bookingDeatil != null)
                        {
                            System.Data.Entity.Core.Objects.ObjectParameter myOutputParam_sts = new System.Data.Entity.Core.Objects.ObjectParameter("r_sts", typeof(String));
                            System.Data.Entity.Core.Objects.ObjectParameter myOutputParam_msg = new System.Data.Entity.Core.Objects.ObjectParameter("r_msg", typeof(String));

                            var date_from = DateTime.ParseExact(bookingDeatil.bc_start_date.Value.ToString("dd/MM/yyyy") + " " + bookingDeatil.bc_start_time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            var date_to = DateTime.ParseExact(bookingDeatil.bc_stop_date.Value.ToString("dd/MM/yyyy") + " " + bookingDeatil.bc_stop_time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                            string s_date_from = date_from.ToString("yyyy-MM-dd") + " " + date_from.ToString("HH:mm:ss", new CultureInfo("th-TH"));
                            string s_date_to = date_to.ToString("yyyy-MM-dd") + " " + date_to.ToString("HH:mm:ss", new CultureInfo("th-TH"));
                            context.sp_bookcar_availability(s_date_from, s_date_to, bookingDeatil.MCA_ID.ToString(), value.id, myOutputParam_sts, myOutputParam_msg);
                            if (myOutputParam_sts.Value != null)
                            {
                                if (myOutputParam_sts.Value.ToString() == "E")
                                {
                                    throw new Exception(myOutputParam_msg.Value.ToString());
                                }
                                else
                                {
                                    int ret = context.sp_bookcar_save_status(value.id, save_type, value.remark, userId);
                                    result.status = "S";
                                    result.message = "";
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Data not Found");
                        }
                    }
                    else
                    {
                        int ret = context.sp_bookcar_save_status(value.id, save_type, value.remark, userId);
                        result.status = "S";
                        result.message = "";
                    }
                }
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel save_draft(bookCarDetailModel value)
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

                    string save_type = "1"; // save_draft
                    if (value.method.Equals("save_send"))
                    {
                        save_type = "2";
                        if (!String.IsNullOrEmpty(value.start_date))
                        {
                            var startDate = DateTime.ParseExact(value.start_date + " " + value.stop_time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            var bfDateNow = DateTime.Now.AddDays(2);
                            if (bfDateNow >= startDate)
                            {
                                throw new Exception("Please book at least 2 days");
                            }
                        }
                    }
                    else if (value.method.Equals("save_update"))
                    {
                        save_type = "";
                        if (!String.IsNullOrEmpty(value.start_date))
                        {
                            var startDate = DateTime.ParseExact(value.start_date + " " + value.stop_time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            var bfDateNow = DateTime.Now.AddDays(2);
                            if (bfDateNow >= startDate)
                            {
                                throw new Exception("Please book at least 2 days");
                            }
                        }
                    }

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);

                    context.interface_log.Add(new interface_log
                    {
                        ID = 1,
                        data_log = json,
                        module = "book_car_save_draft",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    int ret = context.sp_bookcar_save(value.id, value.topic, value.start_date, value.start_time, value.stop_date, value.stop_time, value.person_total, value.car_type_id, value.car_id, value.reason_id, value.dest_id, value.remark, save_type, userId, value.bc_request, myOutputParamInt);

                    if (myOutputParamInt.Value != null)
                    {
                        int r_id = Convert.ToInt32(myOutputParamInt.Value);
                        value.id = r_id.ToString();
                    }


                    context.sp_bookcar_delete_emp(value.id);
                    if (value.emp_list != null)
                    {
                        foreach (var item in value.emp_list)
                        {
                            context.sp_bookcar_insert_emp(value.id, item.emp_code);
                        }
                    }

                }
                result.status = "S";
                result.message = "";
                result.value = value.id;

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