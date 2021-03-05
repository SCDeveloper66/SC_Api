using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class carModel
    {
        public string method { get; set; }

        public string id { get; set; }

        public string car_type { get; set; }

        public string name { get; set; }

        public string detail { get; set; }
        public string color { get; set; }

        public string user_id { get; set; }
        public string carTypes { get; set; }


        public string car_type_from { get; set; }

        public string car_type_to { get; set; }

        public string car_from { get; set; }

        public string car_to { get; set; }
    }

    public class carMasterModel
    {
        public List<dropdown> car_type { get; set; } = new List<dropdown>();
        public List<dropdown> car_license { get; set; } = new List<dropdown>();
        public List<dropdown> car_reason { get; set; } = new List<dropdown>();
        public List<dropdown> car_status { get; set; } = new List<dropdown>();
    }
 
}