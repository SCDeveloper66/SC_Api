using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class book_room_configModel
    {
        public string method { get; set; }

        public string id { get; set; }

        public string timeconfig { get; set; }
        public string user_id { get; set; }
    }

    public class book_room_configDetailModel
    {
        public messageModel result { get; set; }
        public string id { get; set; }
        public string value { get; set; }
    }
}