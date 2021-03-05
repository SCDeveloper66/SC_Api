using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class JsonWebToken
    {
        public messageModel message { get; set; }
        public string Token_login { get; set; }
        public string Permission { get; set; }
        public string Pass_isdefault { get; set; }
    }

    public class messageModel
    {
        public string status { get; set; }
        public string msg { get; set; }
    }
}
