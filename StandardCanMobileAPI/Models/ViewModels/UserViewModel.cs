using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string token_noticiation { get; set; }
        public string language { get; set; }
    }
}
