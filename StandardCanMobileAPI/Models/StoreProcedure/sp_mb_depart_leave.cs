using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.StoreProcedure
{
    public partial class sp_mb_depart_leave
    {
        public string title_group { get; set; }
        public string date_for_sort { get; set; }
        public string title { get; set; }
        public string detail { get; set; }
        public string reason { get; set; }
        public string appr_status { get; set; }
        public string color { get; set; }
    }
}
