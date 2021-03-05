using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class holidayModel
    {
        public string method { get; set; }
        public string year { get; set; }
        public string user_id { get; set; }
    }

    public class holidayMasterModel
    {
        public List<dropdown> year { get; set; } = new List<dropdown>(); 
    }

}