using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class messageModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public string value { get; set; }
    }

    public class messageAuthenticationModel
    {
        public messageModel message { get; set; }
        public string token_login { get; set; }
        public string permission { get; set; }
    }
}