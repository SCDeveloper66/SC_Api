using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class PayrollViewModel
    {
        public messageModel message { get; set; }
        public List<PayrollContentViewModel> content { get; set; }

    }

    public class PayrollContentViewModel
    {
        public string url_icon { get; set; }
        public string title1 { get; set; }
        public string title2 { get; set; }
        public string detail { get; set; }
        public string link_frame { get; set; }
    }

}
