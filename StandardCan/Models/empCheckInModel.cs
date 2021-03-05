using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class empCheckInModel
    {
        public string method { get; set; }      
        public string id { get; set; }
        public string user_id { get; set; }  
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public string emp_code_from { get; set; }
        public string emp_code_to { get; set; }
        public string depart_from { get; set; }
        public string depart_to { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public List<empCheckInListModel> data { get; set; } = new List<empCheckInListModel>();       
    }

    public class empCheckInListModel
    {
        public string id { get; set; }
        public string emp_code { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
    }
    
 
}