using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;
using QRCoder;
using StandardCan.jwt;
using StandardCan.Models;
using StandardCan.Models.ViewModels;

namespace StandardCan.Service
{
    public class timeAttRealService
    {

        public IEnumerable<sp_timeatt_realtime_search_Result> search(timeAttRealModel value)
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
                IEnumerable<sp_timeatt_realtime_search_Result> result = context.sp_timeatt_realtime_search
                    (value.start_date, value.stop_date, value.emp_code_from, value.emp_code_to, value.depart_from, value.depart_to, value.fname, value.lname, value.node_from, value.node_to, userId).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public timeAttRealtimeMasterModel master(timeAttRealModel value)
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

                    sql = "select		convert(nvarchar(5), MN_ID) code ";
                    sql += " , mn_name [text] ";
                    sql += " from MAS_NODE ";
                    sql += " where mn_status = 1 ";
                    sql += " order by mn_name ";
                    result.node = context.Database.SqlQuery<dropdown>(sql).ToList();

                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public timeAttRealtimeDetailModel detail(timeAttRealModel value)
        {
            timeAttRealtimeDetailModel result = new timeAttRealtimeDetailModel();
            try
            {
                using (var context = new StandardCanEntities())
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
                    result.imgList = new List<string>();
                    var attRealtime = context.TIME_ATT_REALTIME.SingleOrDefault(a =>a.TAR_ID.ToString() == value.id);
                    if (attRealtime != null)
                    {
                        var attRealtimeImg = context.TIME_ATT_IMAGE.Where(a => a.TAR_ID == attRealtime.TAR_ID).ToList();
                        if (!Directory.Exists(Path.GetDirectoryName(HostingEnvironment.MapPath("~" + jsonPath.timeAttReal))))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(HostingEnvironment.MapPath("~" + jsonPath.timeAttReal)));
                        }
                        DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.timeAttReal));

                        foreach (var item in attRealtimeImg)
                        {
                            foreach (FileInfo files in di.GetFiles())
                            {
                                if (files.Name == item.tari_image)
                                {
                                    string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.timeAttReal),
                                                                       Path.GetFileName(item.tari_image));
                                    
                                    byte[] bytes = File.ReadAllBytes(path);
                                    var base64 = Convert.ToBase64String(bytes);
                                    result.imgList.Add(base64);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Data not found");
                    }
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

                    //int ret = context.sp_car_update(value.id, value.car_type, value.name, value.detail, userId);
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

        public FileContentResult exportFileJob(string fileName, string dataDate)
        {
            try
            {
                //if (String.IsNullOrEmpty(value.user_id))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                //if (String.IsNullOrEmpty(userId))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                using (var context = new StandardCanEntities())
                {
                    var _Gapi = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "API_PATH");
                    var _Gpath = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "FILE_PATH");
                    JsonPathModel jsonPath = new JsonPathModel();
                    if (_Gpath != null)
                    {
                        jsonPath = (JsonPathModel)Newtonsoft.Json.JsonConvert.DeserializeObject(_Gpath.GGC_VAL, typeof(JsonPathModel));
                    }

                    string out_put_dir = System.AppDomain.CurrentDomain.BaseDirectory + jsonPath.timeAttRealtimeJob;
                    Directory.CreateDirectory(out_put_dir);
                    string filePath = out_put_dir + fileName;
                    var microsoftDateFormatSettings = new JsonSerializerSettings
                    {
                        DateParseHandling = DateParseHandling.None,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        Formatting = Formatting.Indented,
                    };
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        var v_date = String.IsNullOrEmpty(dataDate) ? "" : DateTime.ParseExact(dataDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        IEnumerable<sp_report_timeatt_Result> dataList = context.sp_report_timeatt(v_date).ToList();
                        foreach (var item in dataList)
                        {
                            writer.WriteLine(item.emp_code + "  " + item.type1 + " " + item.tar_date + " " + item.tar_time + " " + item.type2);
                        }
                    }
                    var file = System.IO.Path.Combine(out_put_dir, fileName);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                    FileContentResult result = new FileContentResult(fileBytes, "application/octet-stream");
                    result.FileDownloadName = fileName;
                    
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public FileContentResult exportFileJobSCMM(string fileName, string dataDate)
        {
            try
            {
                //if (String.IsNullOrEmpty(value.user_id))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                //if (String.IsNullOrEmpty(userId))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                using (var context = new StandardCanEntities())
                {
                    var _Gapi = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "API_PATH");
                    var _Gpath = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "FILE_PATH");
                    JsonPathModel jsonPath = new JsonPathModel();
                    if (_Gpath != null)
                    {
                        jsonPath = (JsonPathModel)Newtonsoft.Json.JsonConvert.DeserializeObject(_Gpath.GGC_VAL, typeof(JsonPathModel));
                    }

                    string out_put_dir = System.AppDomain.CurrentDomain.BaseDirectory + jsonPath.timeAttRealtimeJobSCMM;
                    Directory.CreateDirectory(out_put_dir);
                    string filePath = out_put_dir + fileName;
                    var microsoftDateFormatSettings = new JsonSerializerSettings
                    {
                        DateParseHandling = DateParseHandling.None,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        Formatting = Formatting.Indented,
                    };
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        var v_date = String.IsNullOrEmpty(dataDate) ? "" : DateTime.ParseExact(dataDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        IEnumerable<sp_report_timeatt_v3_Result> dataList = context.sp_report_timeatt_v3(v_date).ToList();
                        foreach (var item in dataList)
                        {
                            writer.WriteLine(item.emp_code + "  " + item.type1 + " " + item.tar_date + " " + item.tar_time + " " + item.type2);
                        }
                    }
                    var file = System.IO.Path.Combine(out_put_dir, fileName);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                    FileContentResult result = new FileContentResult(fileBytes, "application/octet-stream");
                    result.FileDownloadName = fileName;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public FileContentResult exportFileWeb(timeAttRealModel value)
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
                using (var context = new StandardCanEntities())
                {
                    var _Gapi = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "API_PATH");
                    var _Gpath = context.MAS_GLOBAL_CONFIG.SingleOrDefault(x => x.GGC_KEY == "FILE_PATH");
                    JsonPathModel jsonPath = new JsonPathModel();
                    if (_Gpath != null)
                    {
                        jsonPath = (JsonPathModel)Newtonsoft.Json.JsonConvert.DeserializeObject(_Gpath.GGC_VAL, typeof(JsonPathModel));
                    }

                    string out_put_dir = System.AppDomain.CurrentDomain.BaseDirectory + jsonPath.timeAttRealtime;
                    Directory.CreateDirectory(out_put_dir);
                    string filePath = out_put_dir + value.fileName;
                    var microsoftDateFormatSettings = new JsonSerializerSettings
                    {
                        DateParseHandling = DateParseHandling.None,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        Formatting = Formatting.Indented,
                    };
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        var v_date_from = String.IsNullOrEmpty(value.start_date) ? "" : DateTime.ParseExact(value.start_date, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        var v_date_to = String.IsNullOrEmpty(value.stop_date) ? "" : DateTime.ParseExact(value.stop_date, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                        IEnumerable<sp_report_timeatt_v2_Result> dataList = context.sp_report_timeatt_v2(value.start_date, value.stop_date, value.emp_code_from, value.emp_code_to, value.depart_from, value.depart_to, value.fname, value.lname, value.node_from, value.node_to, userId).ToList();
                        foreach (var item in dataList)
                        {
                            writer.WriteLine(item.emp_code + "  " + item.type1 + " " + item.tar_date + " " + item.tar_time + " " + item.type2);
                        }
                    }

                    var file = System.IO.Path.Combine(out_put_dir, value.fileName);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                    FileContentResult result = new FileContentResult(fileBytes, "application/octet-stream");
                    result.FileDownloadName = value.fileName;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public FileContentResult exportFileQRCode(qrCodeViewModel value)
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
                using (var context = new StandardCanEntities())
                {
                    var fileName = "QRCode";
                    var qrCodeData = String.Format("{0},{1},{2},{3}", value.id, value.req_date, value.start_date, value.stop_date);
                    QRCodeGenerator _qrCode = new QRCodeGenerator();
                    QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(_qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    var microsoftDateFormatSettings = new JsonSerializerSettings
                    {
                        DateParseHandling = DateParseHandling.None,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        Formatting = Formatting.Indented,
                    };
                    Bitmap newBitmap;
                    PointF roomLocation = new PointF(70, 30);
                    PointF dateLocation = new PointF(70, 660);
                    PointF topicLocation = new PointF(70, 700);
                    using (Graphics graphics = Graphics.FromImage(qrCodeImage))
                    {
                        using (Font arialFont = new Font("Arial", 22, FontStyle.Bold))
                        {
                            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                            graphics.DrawString(value.room_name, arialFont, Brushes.Blue, roomLocation);
                            graphics.DrawString(value.req_date + " " + value.start_date + "-" + value.stop_date, arialFont, Brushes.Blue, dateLocation);
                            graphics.DrawString(value.topic, arialFont, Brushes.Blue, topicLocation);
                        }
                    }
                    newBitmap = new Bitmap(qrCodeImage);
                    byte[] fileBytes = BitmapToBytesCode(newBitmap);
                    FileContentResult result = new FileContentResult(fileBytes, "application/octet-stream");
                    result.FileDownloadName = fileName;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

    }
}