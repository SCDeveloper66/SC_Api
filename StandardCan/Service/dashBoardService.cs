using StandardCan.jwt;
using StandardCan.Models;
using StandardCan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace StandardCan.Service
{
    public class dashBoardService
    {
        public dashBoardModel search(dashBoardModel value)
        {
            dashBoardModel result = new dashBoardModel();
            result.message = new messageModel();
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
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "dashBoard",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var data = context.IMAGE_SLIDE.Where(a => a.is_status == 1).ToList().OrderBy(a => a.is_order_by).ToList();
                    result.imgList = new List<dashBoardImg>();
                    foreach (var item in data)
                    {
                        dashBoardImg news = new dashBoardImg();
                        string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.imageSlide),
                                                                       Path.GetFileName(item.is_url_image));
                        byte[] bytes = File.ReadAllBytes(path);
                        news.base64 = Convert.ToBase64String(bytes);
                        result.imgList.Add(news);
                    }

                    result.message.status = "S";
                    result.message.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.message.status = "E";
                result.message.message = ex.Message.ToString();
            }
            return result;
        }

        public List<dashBoardCalendarViewModel> searchCalendar(dashBoardModel value)
        {
            List<dashBoardCalendarViewModel> result = new List<dashBoardCalendarViewModel>();
            try
            {
                using (StandardCanEntities context = new StandardCanEntities())
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
                        data_log = json,
                        module = "dashBoard",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();


                    var queryRoom = (from a in context.BOOK_ROOM
                                     join b in context.MAS_ROOM on a.MRM_ID equals b.MRM_ID
                                     join c in context.MAS_APPROVE_STATUS on a.br_status equals c.MAS_ID
                                     join d in context.EMP_PROFILE on a.br_create_by equals d.EMP_ID
                                     where a.br_create_by.ToString() == userId && c.mas_name == "Approved"
                                     select new
                                     {
                                         Type = "1",
                                         Title = b.MRM_CODE,
                                         Start = a.br_date + " " + a.br_start_time,
                                         End = a.br_date + " " + a.br_stop_time,
                                         Id = a.BR_ID,
                                         Color = b.MRM_COLOR,
                                         StsText = c.mas_name,
                                         Create_by = a.br_create_by
                                     }).ToList();
                    foreach (var item in queryRoom)
                    {
                        dashBoardCalendarViewModel viewModel = new dashBoardCalendarViewModel();
                        viewModel.id = item.Id.ToString();
                        viewModel.type = item.Type;
                        viewModel.title = item.Title;
                        viewModel.start = item.Start;
                        viewModel.end = item.End;
                        viewModel.color = item.Color;
                        //if(item.Create_by.ToString() == userId)
                        //{
                        //    result.Add(viewModel);
                        //}
                        result.Add(viewModel);
                    }

                    var queryCar = (from a in context.BOOK_CAR
                                    join b in context.MAS_CAR_TYPE on a.MCT_ID equals b.MCT_ID
                                    join c in context.MAS_CAR on a.MCA_ID equals c.MCA_ID
                                    join d in context.MAS_CAR_REASON on a.MCR_ID equals d.MCR_ID
                                    join e in context.EMP_PROFILE on a.bc_create_by equals e.EMP_ID
                                    join f in context.MAS_APPROVE_STATUS on a.bc_status equals f.MAS_ID
                                    join g in context.MAS_DESTINATION on a.MDT_ID equals g.MDT_ID
                                    where a.bc_create_by.ToString() == userId && f.mas_name == "Approved"
                                    select new
                                    {
                                        Type = "2",
                                        Title = d.MCR_NAME + " " + c.MCA_NAME,
                                        Start = a.bc_start_date + " " + a.bc_start_time,
                                        End = a.bc_stop_date + " " + a.bc_stop_time,
                                        Id = a.BC_ID,
                                        Color = c.MCA_COLOR,
                                        StsText = f.mas_name,
                                        Create_by = a.bc_create_by
                                    }).ToList();
                    foreach (var item in queryCar)
                    {
                        dashBoardCalendarViewModel viewModel = new dashBoardCalendarViewModel();
                        viewModel.id = item.Id.ToString();
                        viewModel.type = item.Type;
                        viewModel.title = item.Title;
                        viewModel.start = item.Start;
                        viewModel.end = item.End;
                        viewModel.color = item.Color;
                        //if (item.Create_by.ToString() == userId)
                        //{
                        //    result.Add(viewModel);
                        //}
                        result.Add(viewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public List<sp_calendar_dashboard_Result> searchCalendar_v2(dashBoardModel value)
        {
            try
            {
                using (StandardCanEntities context = new StandardCanEntities())
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
                        data_log = json,
                        module = "dashBoard",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    List<sp_calendar_dashboard_Result> result = context.sp_calendar_dashboard(userId).ToList();
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<dashBoardCalendarViewModel> searchCalendarStore(dashBoardModel value)
        {
            List<dashBoardCalendarViewModel> result = new List<dashBoardCalendarViewModel>();
            try
            {
                using (StandardCanEntities context = new StandardCanEntities())
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
                        data_log = json,
                        module = "dashBoard",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var resultStore = "";

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public IEnumerable<sp_worklist_Result> searchWorkList(dashBoardModel value)
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
                IEnumerable<sp_worklist_Result> result = context.sp_worklist(userId, value.draft, value.pending, value.watiDP, value.approve, value.cancel).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public messageModel workListApproveSubmit(dashBoardModel value)
        {
            messageModel result = new messageModel();

            try
            {
                result.status = "S";
                result.message = "";

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

                    System.Data.Entity.Core.Objects.ObjectParameter myOutputParam_sts = new System.Data.Entity.Core.Objects.ObjectParameter("r_sts", typeof(String));
                    System.Data.Entity.Core.Objects.ObjectParameter myOutputParam_msg = new System.Data.Entity.Core.Objects.ObjectParameter("r_msg", typeof(String));


                    System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));
                    foreach (var item in value.select_room_list)
                    {
                        context.sp_bookroom_availability_in(item, myOutputParam_sts, myOutputParam_msg);
                        if (myOutputParam_sts.Value != null)
                        {
                            if (myOutputParam_sts.Value.ToString() == "E")
                            {
                                result.status = "E";
                                result.message += (myOutputParam_msg.Value == null ? "" : myOutputParam_msg.Value.ToString());
                            }
                            else
                                context.sp_worklist_submit(userId, "1", "1", item, null);

                        } 
                    }
                    foreach (var item in value.select_car_list)
                    {
                        context.sp_bookcar_availability_in(item, myOutputParam_sts, myOutputParam_msg);
                        if (myOutputParam_sts.Value != null)
                        {
                            if (myOutputParam_sts.Value.ToString() == "E")
                            {
                                result.status = "E";
                                result.message += (myOutputParam_msg.Value == null ? "" : myOutputParam_msg.Value.ToString());
                            }
                            else
                                context.sp_worklist_submit(userId, "1", "2", item, null);

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

        public messageModel workListCancelsSubmit(dashBoardModel value)
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

                    foreach (var item in value.select_room_list)
                    {
                        int ret = context.sp_worklist_submit(userId, "0", "1", item, value.remark);
                    }
                    foreach (var item in value.select_car_list)
                    {
                        int ret = context.sp_worklist_submit(userId, "0", "2", item, value.remark);
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

        //public List<dashBoardWorkListViewModel> searchWorkList(dashBoardModel value)
        //{
        //    List<dashBoardWorkListViewModel> result = new List<dashBoardWorkListViewModel>();
        //    try
        //    {
        //        using (StandardCanEntities context = new StandardCanEntities())
        //        {
        //            if (String.IsNullOrEmpty(value.user_id))
        //            {
        //                throw new Exception("Unauthorized Access");
        //            }
        //            var userId = JwtHelper.GetUserIdFromToken(value.user_id);
        //            if (String.IsNullOrEmpty(userId))
        //            {
        //                throw new Exception("Unauthorized Access");
        //            }
        //            JavaScriptSerializer js = new JavaScriptSerializer();
        //            string json = js.Serialize(value);
        //            context.interface_log.Add(new interface_log
        //            {
        //                data_log = json,
        //                module = "dashBoard",
        //                update_date = DateTime.Now
        //            });
        //            context.SaveChanges();
        //            SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
        //            var spData = context.sp_worklist.FromSqlRaw("sp_worklist @emp_id", emp_id).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return result;
        //}
    }
}