using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface INewsService
    {
        Task<NewsViewModel> GetInfomationAsync();
        Task<ImageSliderViewModel> GetImageSlideAsync();
        Task<NewsListViewModel> GetNewsListAsync(string type, string last_id, string language);
        Task<NewsListViewModel> GetNewsDetailAsync(string id, string language);
    }
}
