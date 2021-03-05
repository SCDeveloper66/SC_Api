using StandardCan.jwt;
using StandardCan.Models;
using StandardCan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace StandardCan.Service
{
    public class imageSlideService
    {
        public imageSlideDataResultModel search(imageSlideModel value)
        {
            imageSlideDataResultModel result = new imageSlideDataResultModel();
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
                        module = "ImageSlide_search",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var data = context.IMAGE_SLIDE.Where(a => a.is_status == 1).ToList().OrderBy(a => a.is_order_by).ToList();
                    result.imageSlideData = new List<imageSlideModel>();
                    foreach (var item in data)
                    {
                        imageSlideModel news = new imageSlideModel();
                        news.id = item.IS_ID.ToString();
                        news.order = item.is_order_by.ToString();
                        news.img = !String.IsNullOrEmpty(item.is_url_image) ? _Gapi.GGC_VAL + jsonPath.imageSlide + item.is_url_image : null;
                        string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.imageSlide),
                                                                       Path.GetFileName(item.is_url_image));
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        if (File.Exists(path))
                        {
                            byte[] bytes = File.ReadAllBytes(path);
                            news.base64 = Convert.ToBase64String(bytes);
                            news.url = item.is_url_link;
                        }
                        else
                        {
                            news.base64 = "";
                            news.url = "";
                        }
                       
                        result.imageSlideData.Add(news);
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

        public messageModel insert(imageSlideModel value)
        {
            messageModel result = new messageModel();

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
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "ImageSlide_insert",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var dt = DateTime.Now;
                    IMAGE_SLIDE news = new IMAGE_SLIDE();
                    news.is_status = 1;
                    if (value.img != null)
                    {
                        string[] img = value.img.Split(',');
                        var imgBase64 = img.Count() > 1 ? img[1] : img[0];
                        byte[] imgbyte = Convert.FromBase64String(imgBase64);
                        var guId = Guid.NewGuid().ToString();
                        var fileName = guId + ".JPG";
                        string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.imageSlide),
                                                                   Path.GetFileName(fileName));
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            stream.Write(imgbyte, 0, imgbyte.Length);
                        }
                        news.is_url_image = fileName;
                        news.is_url_link = value.url;
                        news.is_update_date = dt;
                        news.is_update_by = Convert.ToInt32(userId);
                        var lastOrder = context.IMAGE_SLIDE.OrderByDescending(u => u.is_order_by).FirstOrDefault();
                        news.is_order_by = (short?)(lastOrder != null ? (Convert.ToInt32(lastOrder.is_order_by) + 1) : 0);
                        context.IMAGE_SLIDE.Add(news);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Image not Found");
                    }

                    result.status = "S";
                    result.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel update(imageSlideModel value)
        {
            messageModel result = new messageModel();

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
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "ImageSlide_update",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var dt = DateTime.Now;
                    var news = context.IMAGE_SLIDE.SingleOrDefault(a => a.IS_ID.ToString() == value.id);
                    if (news != null)
                    {
                        news.is_order_by = (short?)(value.order != null ? Convert.ToInt32(value.order) : 0);
                        news.is_status = 1;
                        if (value.img != null)
                        {
                            string[] img = value.img.Split(',');
                            var imgBase64 = img.Count() > 1 ? img[1] : img[0];
                            byte[] imgbyte = Convert.FromBase64String(imgBase64);
                            var guId = Guid.NewGuid().ToString();
                            var fileName = guId + ".JPG";
                            string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.imageSlide),
                                                                       Path.GetFileName(fileName));
                            if (!Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(path));
                            }
                            DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.imageSlide));
                            foreach (FileInfo files in di.GetFiles())
                            {
                                if (files.Name == news.is_url_image)
                                {
                                    files.Delete();
                                }
                            }
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                stream.Write(imgbyte, 0, imgbyte.Length);
                            }
                            news.is_url_image = fileName;
                        }
                        else
                        {
                            throw new Exception("Image not Found");
                        }
                        news.is_url_link = value.url;
                        news.is_update_date = dt;
                        news.is_update_by = Convert.ToInt32(userId);
                        context.SaveChanges();

                        result.status = "S";
                        result.message = "Success";
                    }
                    else
                    {
                        result.status = "E";
                        result.message = "Data not Found";
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

        public messageModel delete(imageSlideModel value)
        {
            messageModel result = new messageModel();

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
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "ImageSlide_delete",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var news = context.IMAGE_SLIDE.SingleOrDefault(a => a.IS_ID.ToString() == value.id);
                    if (news != null)
                    {
                        DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.imageSlide));
                        foreach (FileInfo files in di.GetFiles())
                        {
                            if (files.Name == news.is_url_image)
                            {
                                files.Delete();
                            }
                        }
                        context.IMAGE_SLIDE.Remove(news);
                        context.SaveChanges();

                        result.status = "S";
                        result.message = "Success";
                    }
                    else
                    {
                        result.status = "E";
                        result.message = "Data not Found";
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

    }
}