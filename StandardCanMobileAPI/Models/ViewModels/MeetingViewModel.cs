using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class MeetingViewModel
    {
        public messageModel message { get; set; }
        public List<MeetingContentViewModel> content { get; set; }
    }

    public class MeetingContentViewModel
    {
        public string title { get; set; }
        public List<MeetingChlidsViewModel> chlids { get; set; }
    }


    public class MeetingChlidsViewModel
    {
        public string title { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        public string icon { get; set; }
    }

}
