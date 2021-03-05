using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class NewsViewModel
    {
        public messageModel message { get; set; }
        public string notification_count { get; set; }
        public string sc_creadit { get; set; }
        public string profile_img { get; set; }
        public string profile_name { get; set; }
        public string profile_depart { get; set; }

    }

    public class ImageSliderViewModel
    {
        public messageModel message { get; set; }
        public List<NewsImageViewModel> img_slider { get; set; }
    }

    public class NewsListViewModel
    {
        public messageModel message { get; set; }
        public List<NewscontentViewModel> content { get; set; }
    }

    public class NewsImageViewModel
    {
        public string url_img { get; set; }
        public string url_link { get; set; }
    }

    public class NewscontentViewModel
    {
        public string id { get; set; }
        public string url_img { get; set; }
        public string title { get; set; }
        public string sub_detail { get; set; }
        public string time { get; set; }
        public string link_youtube { get; set; }
    }


}
