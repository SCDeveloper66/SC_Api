using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StandardCanMobileAPI.Helper;
using StandardCanMobileAPI.Models;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services
{
    public class NewsService : INewsService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;

        public NewsService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _systemLogService = systemLogService;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;

        public async Task<NewsViewModel> GetInfomationAsync()
        {
            var data = new NewsViewModel();
            data.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = userId
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/News/GetInfomation",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    var spData = context.SpMbProfileDefault.FromSqlRaw("sp_mb_profile_default @emp_id", emp_id).ToList();
                    foreach (var item in spData)
                    {
                        data.notification_count = item.notification_count;
                        data.sc_creadit = item.sc_creadit;
                        data.profile_img = item.profile_img;
                        data.profile_name = item.profile_name;
                        data.profile_depart = item.profile_depart;
                    }
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<ImageSliderViewModel> GetImageSlideAsync()
        {
            var data = new ImageSliderViewModel();
            data.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = userId
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/News/GetImageSlide",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    var param = new SqlParameter("emp_id", userId ?? "");
                    var spData = context.SpMbImageSlide.FromSqlRaw("sp_mb_image_slide @emp_id", param).ToList();
                    data.img_slider = new List<NewsImageViewModel>();
                    foreach (var item in spData)
                    {
                        NewsImageViewModel newsImage = new NewsImageViewModel();
                        newsImage.url_img = item.url_img;
                        newsImage.url_link = item.url_link;
                        data.img_slider.Add(newsImage);
                    }
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<NewsListViewModel> GetNewsListAsync(string type, string last_id, string language)
        {
            var data = new NewsListViewModel();
            data.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        new_type = type,
                        last_id = last_id,
                        emp_id = userId,
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/News/GetNewsList",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter new_type = new SqlParameter("new_type", type ?? "");
                    SqlParameter lastId = new SqlParameter("last_id", last_id ?? "");
                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbNewsList.FromSqlRaw("sp_mb_news_list @new_type, @last_id, @emp_id, @lang", new_type, lastId, emp_id, lang).ToList();
                    data.content = new List<NewscontentViewModel>();
                    foreach (var item in spData)
                    {
                        NewscontentViewModel news = new NewscontentViewModel();
                        news.id = item.id;
                        news.url_img = item.url_img;
                        news.title = item.title;
                        news.sub_detail = item.sub_detail;
                        news.time = item.time;
                        news.link_youtube = item.link_youtube;
                        data.content.Add(news);
                    }
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<NewsListViewModel> GetNewsDetailAsync(string id, string language)
        {
            var data = new NewsListViewModel();
            data.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        id = id,
                        emp_id = userId,
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/News/GetNewsDetail",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter new_id = new SqlParameter("id", id ?? "");
                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");

                    var spData = context.SpMbNewsDetail.FromSqlRaw("sp_mb_news_detail @id, @emp_id, @lang", new_id, emp_id, lang).ToList();
                    data.content = new List<NewscontentViewModel>();
                    foreach (var item in spData)
                    {
                        NewscontentViewModel news = new NewscontentViewModel();
                        news.id = item.id;
                        news.url_img = item.url_img;
                        news.title = item.title;
                        news.sub_detail = item.sub_detail;
                        news.time = item.time;
                        news.link_youtube = item.link_youtube;
                        data.content.Add(news);
                    }
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }
    }
}
