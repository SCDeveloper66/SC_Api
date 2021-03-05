using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class projectModel
    {
        public string method { get; set; }
        public string year_from { get; set; }
        public string year_to { get; set; }
        public string prj_from { get; set; }
        public string prj_to { get; set; }
        public string prj_name { get; set; }

        public string start_date { get; set; }
        public string stop_date { get; set; }
        public string prj_detail { get; set; }
        public string prj_status { get; set; }

        public string prj_id { get; set; }
        public string user_id { get; set; }
        public string status_id { get; set; }

    }
    public class projectMasterModel
    {
        public List<dropdown> year { get; set; } = new List<dropdown>();
        public List<dropdown> project { get; set; } = new List<dropdown>(); 
    }


}