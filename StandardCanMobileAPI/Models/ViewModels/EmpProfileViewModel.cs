using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class EmpProfileViewModel
    {
        public messageModel message { get; set; }
        public string url_img { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string id { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string line { get; set; }
        public string address { get; set; }
        public string dist { get; set; }
        public string prov { get; set; }
        public string country { get; set; }
        public string address_code { get; set; }
        public string outdoor_sts { get; set; }
    }

    public class EmpProfileDataViewModel
    {
        public string token_login { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string line { get; set; }
        public string language { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string token_login { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
        public string language { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string emp_code { get; set; }
        public string new_pass { get; set; }
        public string confirm_pass { get; set; }
        public string email { get; set; }
        public string language { get; set; }
    }

    public class UserNotiDetail
    {
        public messageModel message { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string remark { get; set; }
        public List<UserNotiDetailImg> img { get; set; }
    }

    public class UserNotiDetailImg
    {
        public string url { get; set; }
    }


}

