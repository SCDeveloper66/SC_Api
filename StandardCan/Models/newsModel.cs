using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class newsModel
    {
        public string method { get; set; }
        public string id { get; set; }
        public string newsTypeId { get; set; }
        public string newsTypeName { get; set; }
        public string topic { get; set; }
        public string detail { get; set; }
        public string img { get; set; }
        public string url { get; set; }
        public string urlVdo { get; set; }
        public string user_id { get; set; }
    }

    public class newsDataResultModel
    {
        public messageModel message { get; set; }
        public List<newsModel> newsData { get; set; }
    }
}