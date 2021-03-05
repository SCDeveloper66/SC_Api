using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.StoreProcedure
{
    public partial class sp_mb_news_detail
    {
        public string id { get; set; }
        public string url_img { get; set; }
        public string title { get; set; }
        public string sub_detail { get; set; }
        public string time { get; set; }
        public string link_youtube { get; set; }
    }
}
