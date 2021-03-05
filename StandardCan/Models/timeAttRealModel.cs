using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class timeAttRealModel
    {
        public string method { get; set; }
        public string id { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public string emp_code_from { get; set; }
        public string emp_code_to { get; set; }
        public string depart_from { get; set; }
        public string depart_to { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string node_from { get; set; }
        public string node_to { get; set; }

        public string fileName { get; set; }


        public string car_type { get; set; }

        public string name { get; set; }

        public string detail { get; set; }

        public string user_id { get; set; }

     

        public string car_type_from { get; set; }

        public string car_type_to { get; set; }

        public string car_from { get; set; }

        public string car_to { get; set; }
    }

    public class timeAttRealtimeMasterModel
    {
        public List<dropdown> department { get; set; } = new List<dropdown>();
        public List<dropdown> node { get; set; } = new List<dropdown>(); 
    }

    public class timeAttRealtimeDetailModel
    {
        public string id { get; set; }
        public List<string> imgList { get; set; }
    }

    public class qrCodeViewModel
    {
        public string method { get; set; }
        public string id { get; set; }
        public string room_name { get; set; }
        public string topic { get; set; }
        public string req_date { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public string user_id { get; set; }
    }

}