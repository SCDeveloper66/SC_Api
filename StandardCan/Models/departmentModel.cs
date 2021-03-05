using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class departmentModel
    {
        public string method { get; set; }

        public string id { get; set; }

        public string name { get; set; }

        public string tel { get; set; }
        public Boolean dis_status { get; set; }
        public Boolean status { get; set; }
        public string user_id { get; set; }
    }
}