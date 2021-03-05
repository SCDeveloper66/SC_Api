using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class imageSlideModel
    {
        public string method { get; set; }
        public string id { get; set; }
        public string img { get; set; }
        public string url { get; set; }
        public string order { get; set; }
        public string base64 { get; set; }
        public string user_id { get; set; }
    }

    public class imageSlideDataResultModel
    {
        public messageModel message { get; set; }
        public List<imageSlideModel> imageSlideData { get; set; }
    }
}