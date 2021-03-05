using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class NotiViewModel
    {
        public messageModel message { get; set; }
        public List<NoticontentViewModel> content { get; set; }
    }

    public class NoticontentViewModel
    {
        public string id { get; set; }
        public string type { get; set; }
        public string url_img { get; set; }
        public string title { get; set; }
        public string detail { get; set; }
        public string time { get; set; }
    }

    public class NotiSettingViewModel
    {
        public string notification { get; set; }
        public string language { get; set; }
    }

    public class NotiSettingDataViewModel
    {
        public messageModel message { get; set; }
        public string notification { get; set; }
    }

}
