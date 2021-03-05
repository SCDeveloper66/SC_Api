using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class benefitsModel
    {
        public string method { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string detail { get; set; }
        public string doc { get; set; }
        public string url { get; set; }
        public string user_id { get; set; }
    }

    public class benefitsDataResultModel
    {
        public messageModel message { get; set; }
        public List<benefitsModel> benefitData { get; set; }
    }
}