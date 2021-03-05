using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;

namespace StandardCanMobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(NewsViewModel))]
        public async Task<NewsViewModel> GetInfomation(string language)
        {
            var data = await _newsService.GetInfomationAsync();
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(ImageSliderViewModel))]
        public async Task<ImageSliderViewModel> GetImageSlide(string language)
        {
            var data = await _newsService.GetImageSlideAsync();
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(NewsListViewModel))]
        public async Task<NewsListViewModel> GetNewsList(string type, string last_id, string language)
        {
            var data = await _newsService.GetNewsListAsync(type, last_id, language);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(NewsListViewModel))]
        public async Task<NewsListViewModel> GetNewsDetail(string id, string language)
        {
            var data = await _newsService.GetNewsDetailAsync(id, language);
            return data;
        }

    }
}
