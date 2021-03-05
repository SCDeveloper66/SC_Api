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
    public class benefitsService
    {
        public benefitsDataResultModel search(benefitsModel value)
        {
            benefitsDataResultModel result = new benefitsDataResultModel();
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
                        module = "Benefits_search",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var data = context.MAS_BENEFITS.Where(a => a.mb_title.Contains(value.title)).ToList();
                    result.benefitData = new List<benefitsModel>();
                    foreach (var item in data)
                    {
                        benefitsModel news = new benefitsModel();
                        news.id = item.MB_ID.ToString();
                        news.title = item.mb_title;
                        news.detail = item.mb_detail;
                        news.url = !String.IsNullOrEmpty(item.mb_link_url) ? _Gapi.GGC_VAL + jsonPath.benefits + item.mb_link_url : null;
                        result.benefitData.Add(news);
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

        public messageModel insert(benefitsModel value)
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
                        module = "Benefits_insert",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var dt = DateTime.Now;
                    MAS_BENEFITS benefits = new MAS_BENEFITS();
                    benefits.mb_title = value.title;
                    benefits.mb_detail = value.detail;

                    if (value.doc != null)
                    {
                        string[] doc = value.doc.Split(',');
                        var docBase64 = doc.Count() > 1 ? doc[1] : doc[0];
                        byte[] imgbyte = Convert.FromBase64String(docBase64);
                        var guId = Guid.NewGuid().ToString();
                        var fileName = guId + ".PDF";
                        string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.benefits),
                                                                   Path.GetFileName(fileName));
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            stream.Write(imgbyte, 0, imgbyte.Length);
                        }
                        benefits.mb_link_url = fileName;
                    }
                    context.MAS_BENEFITS.Add(benefits);
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


        public messageModel update(benefitsModel value)
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
                        module = "Benefits_update",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var dt = DateTime.Now;
                    var benefits = context.MAS_BENEFITS.SingleOrDefault(a => a.MB_ID.ToString() == value.id);
                    if (benefits != null)
                    {
                        benefits.mb_title = value.title;
                        benefits.mb_detail = value.detail;
                        if (value.doc != null)
                        {
                            string[] doc = value.doc.Split(',');
                            var imgBase64 = doc.Count() > 1 ? doc[1] : doc[0];
                            byte[] imgbyte = Convert.FromBase64String(imgBase64);
                            var guId = Guid.NewGuid().ToString();
                            var fileName = guId + ".PDF";
                            string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.benefits),
                                                                       Path.GetFileName(fileName));
                            if (!Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(path));
                            }
                            DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.benefits));
                            foreach (FileInfo files in di.GetFiles())
                            {
                                if (files.Name == benefits.mb_link_url)
                                {
                                    files.Delete();
                                }
                            }
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                stream.Write(imgbyte, 0, imgbyte.Length);
                            }
                            benefits.mb_link_url = fileName;
                        }
                        else if (String.IsNullOrEmpty(value.url))
                        {
                            string path = Path.Combine(HostingEnvironment.MapPath("~" + jsonPath.benefits));
                            if (!Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(path));
                            }
                            DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.benefits));
                            foreach (FileInfo files in di.GetFiles())
                            {
                                if (files.Name == benefits.mb_link_url)
                                {
                                    files.Delete();
                                }
                            }
                            benefits.mb_link_url = null;
                        }
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

        public messageModel delete(benefitsModel value)
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
                        module = "Benefits_delete",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var benefits = context.MAS_BENEFITS.SingleOrDefault(a => a.MB_ID.ToString() == value.id);
                    if (benefits != null)
                    {
                        DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~" + jsonPath.benefits));
                        foreach (FileInfo files in di.GetFiles())
                        {
                            if (files.Name == benefits.mb_link_url)
                            {
                                files.Delete();
                            }
                        }
                        context.MAS_BENEFITS.Remove(benefits);
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