using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class CompanyViewModel
    {
        public messageModel message { get; set; }
        public List<CompanyContentViewModel> content { get; set; }

    }
    public class CompanyContentViewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string detail_line1 { get; set; }
        public string detail_line2 { get; set; }
        public string detail_line3 { get; set; }
        public string detail_line4 { get; set; }
        public string link_file { get; set; }
    }

    public class AboutCompanyViewModel
    {
        public messageModel message { get; set; }
        public List<AboutCompanyContentViewModel> content { get; set; }

    }

    public class AboutCompanyContentViewModel
    {
        public string name { get; set; }
        public string tel { get; set; }
    }

}
