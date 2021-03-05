using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models.ViewModels
{
    public class UserViewModel
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string token_notification { get; set; }
        public string language { get; set; }
    }
}