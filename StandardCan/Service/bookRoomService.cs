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
    public class bookRoomService
    {

        public bookRoomDetailModel detail(bookRoomModel value)
        {
            bookRoomDetailModel result = new bookRoomDetailModel();

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

                    string sql = "select		convert(nvarchar(5), mrm_id) code ";
                    sql += " , mrm_code [text] ";
                    sql += " from MAS_ROOM ";
                    sql += " where MRM_STATUS=1";
                    sql += " order by mrm_name ";
                    result.room_list = context.Database.SqlQuery<dropdown>(sql).ToList();

                    if (string.IsNullOrEmpty(value.id))
                    {
                        sql = "select   convert(nvarchar(3), MHW_ID) id ";
                        sql += "        , MHW_NAME text ";
                        sql += ", convert(bit, 0) opt ";
                        sql += " from   MAS_HARDWARE ";
                        sql += " where  MHW_STATUS = 1";
                        result.device_list = context.Database.SqlQuery<checkbox_list>(sql).ToList();

                        result.button.reject = false;
                        result.button.cancel = false;
                        result.button.save_revise = false;
                    }
                    else
                    {
                        sql = "select   convert(nvarchar(3), a.MHW_ID) id ";
                        sql += "        , MHW_NAME text ";
                        sql += ", convert(bit, case when b.MHW_ID is null then 0 else 1 end ) opt ";
                        sql += " from   MAS_HARDWARE a left join BOOK_ROOM_DEVICE b on a.MHW_ID=b.MHW_ID and b.BR_ID= " + value.id;
                        sql += " where  MHW_STATUS = 1";
                        result.device_list = context.Database.SqlQuery<checkbox_list>(sql).ToList();

                        result.emp_list = context.sp_bookroom_emp(value.id).ToList();

                        var h = context.BOOK_ROOM.Where(p => p.BR_ID.ToString().Equals(value.id)).FirstOrDefault();
                        if (h != null)
                        {
                            var mas_room = context.MAS_ROOM.SingleOrDefault(a =>a.MRM_ID == h.MRM_ID);
                            result.id = h.BR_ID.ToString();
                            result.room_name = mas_room != null ? mas_room.MRM_NAME : "";
                            result.topic = h.br_topic ?? "";
                            result.date = h.br_date == null ? "" : Convert.ToDateTime(h.br_date).ToString("dd/MM/yyyy");
                            result.start_time = h.br_start_time ?? "";
                            result.stop_time = h.br_stop_time ?? "";
                            result.person_total = h.br_person_total == null ? "" : h.br_person_total.ToString();
                            result.room_id = h.MRM_ID == null ? "" : h.MRM_ID.ToString();
                            result.remark = h.br_remark ?? "";
                            result.br_status = h.br_status.ToString();

                            if(h.br_request_by != null)
                            {
                                result.br_request = h.br_request_by.ToString();
                                var emp_request = context.EMP_PROFILE.SingleOrDefault(a => a.emp_code == h.br_request_by.ToString());
                                if (emp_request != null)
                                {
                                    result.br_request_name = emp_request.emp_fname + " " + emp_request.emp_lname;
                                }
                            }

                            if (h.br_status == 1)
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

                                if (h.br_create_by.ToString() == userId)
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
                            else if (h.br_status == 2)
                            {
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
                                if (h.br_create_by.ToString() == userId)
                                {
                                    result.button.cancel = true;
                                }
                                result.read_only = true;
                            }
                            else if (h.br_status == 0 || h.br_status == 3 || h.br_status == 4 || h.br_status == 5)
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
                                result.read_only = true;
                                if(h.br_status == 3)
                                {
                                    result.button.gen_qrCode = true;
                                }
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
            bookRoomMasterModel result = new bookRoomMasterModel();

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

                    string sql = "select		convert(nvarchar(5), MRM_ID) code ";
                    sql += " , MRM_CODE [text] ";
                    sql += " from MAS_ROOM ";
                    sql += " order by MRM_CODE ";
                    result.room = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = " select convert(nvarchar(5), MAS_ID) code, mas_name [text] ";
                    sql += " from MAS_APPROVE_STATUS ";
                    sql += " where MAS_ID != 0 ";
                    sql += " order by mas_orderby ";
                    result.status = context.Database.SqlQuery<dropdown>(sql).ToList();

                }

                //if (myOutputParamInt.Value != null)
                //{
                //    int r_id = Convert.ToInt32(myOutputParamInt.Value);
                //    result.status = "S";
                //    result.message = "";
                //    result.value = r_id.ToString();
                //}
                //else
                //{
                //    result.status = "E";
                //    result.message = "";
                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public IEnumerable<sp_bookroom_search_v3_Result> search(bookRoomModel value)
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
                IEnumerable<sp_bookroom_search_v3_Result> result = context.sp_bookroom_search_v3(value.room_from, value.room_to, value.date_from, value.date_to, value.status_from, value.status_to, userId).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<sp_bookroom_search_v3_Result> search_all_calendar(bookRoomModel value)
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
                IEnumerable<sp_bookroom_search_v3_Result> result = context.sp_bookroom_search_v3(value.room_from, value.room_to, value.date_from, value.date_to, value.status_from, value.status_to, "all").AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public messageModel save_status(bookRoomDetailModel value)
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

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);

                    context.interface_log.Add(new interface_log
                    {
                        ID = 1,
                        data_log = json,
                        module = "book_room_save",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    string save_type = "";
                    if (value.method.Equals("save_cancel"))
                    {
                        save_type = "0";
                    }
                    else if (value.method.Equals("save_approve"))
                    {
                        save_type = "3";
                    }
                    else if (value.method.Equals("save_reject"))
                    {
                        save_type = "4";
                    }
                    else if (value.method.Equals("save_revise"))
                    {
                        ///TODO Draft
                        save_type = "1";
                    }

                    if (save_type == "3")
                    {
                        var allRoomList = context.BOOK_ROOM.ToList();
                        var bookingDeatil = allRoomList.SingleOrDefault(a =>a.BR_ID.ToString() == value.id);
                        if(bookingDeatil != null)
                        {
                            var dupTime = 0;
                            //var chkSameDate = allRoomList.Where(a => a.br_date == bookingDeatil.br_date && a.MRM_ID == bookingDeatil.MRM_ID && a.br_status == 3).ToList();
                            //var start_time = DateTime.ParseExact(bookingDeatil.br_start_time, "HH:mm", CultureInfo.InvariantCulture);
                            //var stop_time = DateTime.ParseExact(bookingDeatil.br_stop_time, "HH:mm", CultureInfo.InvariantCulture);
                            //foreach (var item in chkSameDate)
                            //{
                            //    var _itemStart_time = DateTime.ParseExact(item.br_start_time, "HH:mm", CultureInfo.InvariantCulture);
                            //    var _itemStop_time = DateTime.ParseExact(item.br_stop_time, "HH:mm", CultureInfo.InvariantCulture);
                                
                            //    if(stop_time > _itemStart_time)
                            //    {
                            //        dupTime++;
                            //    }
                            //}
                            if(dupTime == 0)
                            {
                                int ret = context.sp_bookroom_save_status(value.id, save_type, value.remark, userId);
                                result.status = "S";
                                result.message = "";
                            }
                            else
                            {
                                throw new Exception("ห้องนี้ในช่วงเวลานี้ได้ถูกจองแล้ว");
                            }
                        }
                        else
                        {
                            throw new Exception("Data not Found");
                        }
                    }
                    else
                    {
                        int ret = context.sp_bookroom_save_status(value.id, save_type, value.remark, userId);
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

        public messageModel save_draft(bookRoomDetailModel value)
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

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);

                    context.interface_log.Add(new interface_log
                    {
                        ID = 1,
                        data_log = json,
                        module = "book_room_save_draft",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    string save_type = "1"; // save_draft
                    if (value.method.Equals("save_send"))
                    {
                        save_type = "2";
                    }
                    //else if (value.method.Equals("save_revise"))
                    //{
                    //    save_type = "1";
                    //}

                    int ret = context.sp_bookroom_save(value.id, value.topic, value.date, value.start_time, value.stop_time
                        , value.room_id, value.person_total, value.remark, save_type, userId, myOutputParamInt, value.br_request);

                    if (myOutputParamInt.Value != null)
                    {
                        int r_id = Convert.ToInt32(myOutputParamInt.Value);
                        result.status = "S";
                        result.message = "";
                        result.value = r_id.ToString();
                        value.id = String.IsNullOrEmpty(value.id) ? r_id.ToString() : value.id;
                    }
                    else
                    {
                        result.status = "E";
                        result.message = "";
                    }

                    context.sp_bookroom_delete_device(value.id);

                    if (value.device_list != null)
                    {
                        foreach (var item in value.device_list)
                        {
                            if (item.opt)
                                context.sp_bookroom_insert_device(value.id, item.id);
                        }
                    }

                    context.sp_bookroom_delete_emp(value.id);
                    if (value.emp_list != null)
                    {
                        foreach (var item in value.emp_list)
                        {
                            context.sp_bookroom_insert_emp(value.id, item.emp_code);
                        }
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