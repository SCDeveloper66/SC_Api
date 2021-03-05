using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class roomModel
    {
        public string method { get; set; }

        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string color { get; set; }

        public string user_id { get; set; }
    }

    public class roomMasterModel
    {
        public List<dropdown> color { get; set; } = new List<dropdown>();
    }
}