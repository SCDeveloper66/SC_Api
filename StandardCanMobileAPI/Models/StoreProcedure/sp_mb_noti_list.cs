using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.StoreProcedure
{
    public partial class sp_mb_noti_list
    {
        public string id { get; set; }
        public string type { get; set; }
        public string url_img { get; set; }
        public string title { get; set; }
        public string detail { get; set; }
        public string time { get; set; }
    }
}
