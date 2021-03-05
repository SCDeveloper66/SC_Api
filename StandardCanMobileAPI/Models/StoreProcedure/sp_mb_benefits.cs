using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.StoreProcedure
{
    public partial class sp_mb_benefits
    {
        public string id { get; set; }
        public string title { get; set; }
        public string detail_line1 { get; set; }
        public string detail_line2 { get; set; }
        public string detail_line3 { get; set; }
        public string detail_line4 { get; set; }
        public string link_file { get; set; }
    }
}
