using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class hardwareModel
    {
        public string method { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
    }

    public class hardwareListModel
    {
        public messageModel message { get; set; } = new messageModel();
        public List<hardwareList> list { get; set; } = new List<hardwareList>();
    }

    public class hardwareList
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}