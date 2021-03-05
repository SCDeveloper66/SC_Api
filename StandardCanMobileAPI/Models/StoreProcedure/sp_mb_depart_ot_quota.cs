using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.StoreProcedure
{
    public partial class sp_mb_depart_ot_quota
    {
        public string title_group { get; set; }
        public string date_for_sort { get; set; }
        public string title { get; set; }
        public Nullable<int> value { get; set; }
        public string value_color { get; set; }
    }
}
