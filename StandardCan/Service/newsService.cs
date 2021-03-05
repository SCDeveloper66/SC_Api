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
    public class newsService
    {
        public newsDataResultModel search(newsModel value)
        {
            newsDataResultModel result = new newsDataResultModel();
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
                        module = "News_search",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var data = context.NEWS.Where(a => a.nw_topic.Contains(value.topic)).ToList();
                    result.newsData = new List<newsModel>();
                    foreach (var item in data)
                    {
                        newsModel news = new newsModel();
                        news.id = item.NW_ID.ToString();
                        news.newsTypeId = item.nw_type.ToString();
                        if (news.newsTypeId == "1")
                        {
                            news.newsTypeName = "ข่าวสาร";
                        }
                        else if (news.newsTypeId == "2")
                        {
                            news.newsTypeName = "กิจกรรม";
                        }
                        else if (news.newsTypeId == "3")
                        {
                            news.newsTypeName = "vdo";
                        }
                        news.topic = item.nw_topic;
                        news.detail = item.nw_detail;
                        news.url = !String.IsNullOrEmpty(item.nw_image) ? _Gapi.GGC_VAL + jsonPath.news + item.nw_image : null;
                        news.urlVdo = item.nw_vdo;
                        result.newsData.Add(news);
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

        public messageModel insert(newsModel value)
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
                        module = "News_insert",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var dt = DateTime.Now;
                    NEWS news = new NEWS();
                    news.nw_type = value.newsTypeId != null ? Convert.ToInt32(value.newsTypeId) : (int?)null;
                    news.nw_topic = value.topic;
                    news.nw_detail = value.detail;
                    news.nw_status = 1;

                    if (news.nw_type == 1 || news.nw_type == 2)
                    {
                        if (value.img != null)
                        {
                            string[] img = value.img.Split(',');
                            var imgBase64 = img.Count() > 1 ? img[1] : img[0];
                            byte[] imgbyte = Convert.FromBase64String(imgBase64);
                            var guId = Guid.NewGuid().ToString();
                            var fileName = guId + ".JPG";
                            string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.news),
                                                                       Path.GetFileName(fileName));
                            if (!Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(path));
                            }
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                stream.Write(imgbyte, 0, imgbyte.Length);
                            }
                            news.nw_image = fileName;
                        }
                    }
                    else if (news.nw_type == 3)
                    {
                        news.nw_vdo = value.urlVdo;
                    }

                    news.nw_date = dt;
                    news.nw_update_by = Convert.ToInt32(userId);
                    news.nw_update_date = dt;
                    context.NEWS.Add(news);
                    context.SaveChanges();

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

        public messageModel update(newsModel value)
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
                        module = "News_update",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var dt = DateTime.Now;
                    var news = context.NEWS.SingleOrDefault(a => a.NW_ID.ToString() == value.id);
                    if (news != null)
                    {
                        news.nw_type = value.newsTypeId != null ? Convert.ToInt32(value.newsTypeId) : (int?)null;
                        news.nw_topic = value.topic;
                        news.nw_detail = value.detail;
                        news.nw_status = 1;

                        //if (news.nw_type == 1 || news.nw_type == 2 || news.nw_type == 3)
                        //{
                        if (value.img != null)
                        {
                            string[] img = value.img.Split(',');
                            var imgBase64 = img.Count() > 1 ? img[1] : img[0];
                            byte[] imgbyte = Convert.FromBase64String(imgBase64);
                            var guId = Guid.NewGuid().ToString();
                            var fileName = guId + ".JPG";
                            string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.news),
                                                                       Path.GetFileName(fileName));
                            if (!Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(path));
                            }
                            DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.news));
                            foreach (FileInfo files in di.GetFiles())
                            {
                                if (files.Name == news.nw_image)
                                {
                                    files.Delete();
                                }
                            }
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                stream.Write(imgbyte, 0, imgbyte.Length);
                            }
                            news.nw_image = fileName;
                        }
                        else if (String.IsNullOrEmpty(value.url))
                        {
                            string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.news));
                            if (!Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(path));
                            }
                            DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.news));
                            foreach (FileInfo files in di.GetFiles())
                            {
                                if (files.Name == news.nw_image)
                                {
                                    files.Delete();
                                }
                            }
                            news.nw_image = null;
                        }
                        //}
                        //else if (news.nw_type == 3)
                        //{
                        //    news.nw_vdo = String.IsNullOrEmpty(value.urlVdo) ? null : value.urlVdo;
                        //    DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.news));
                        //    foreach (FileInfo files in di.GetFiles())
                        //    {
                        //        if (files.Name == news.nw_image)
                        //        {
                        //            files.Delete();
                        //        }
                        //    }
                        //    news.nw_image = null;
                        //}

                        news.nw_vdo = String.IsNullOrEmpty(value.urlVdo) ? null : value.urlVdo;
                        news.nw_update_date = dt;
                        news.nw_update_by = Convert.ToInt32(userId);
                        context.SaveChanges();

                        result.status = "S";
                        result.message = "Success";
                    }
                    else
                    {
                        result.status = "E";
                        result.message = "Error";
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

        public messageModel delete(newsModel value)
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
                        module = "News_delete",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var news = context.NEWS.SingleOrDefault(a => a.NW_ID.ToString() == value.id);
                    if (news != null)
                    {
                        DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.news));
                        foreach (FileInfo files in di.GetFiles())
                        {
                            if (files.Name == news.nw_image)
                            {
                                files.Delete();
                            }
                        }
                        context.NEWS.Remove(news);
                        context.SaveChanges();

                        result.status = "S";
                        result.message = "Success";
                    }
                    else
                    {
                        result.status = "E";
                        result.message = "Error";
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