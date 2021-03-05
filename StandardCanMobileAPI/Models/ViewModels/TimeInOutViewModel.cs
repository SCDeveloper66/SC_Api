using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class TimeInOutViewModel
    {
        public messageModel message { get; set; }
        public List<TimeInOutContentViewModel> content { get; set; }
    }

    public class TimeInOutContentViewModel
    {
        public string title { get; set; }
        public List<TimeInOutContentchildsViewModel> childs { get; set; }
    }

    public class TimeInOutContentchildsViewModel
    {
        public string title { get; set; }
        public string detail { get; set; }
    }

    public class InoutEmpRealtimeViewModel
    {
        public messageModel message { get; set; }
        public List<InoutEmpRealtimeEmpViewModel> emps { get; set; }
    }

    public class InoutEmpRealtimeEmpViewModel
    {
        public string name { get; set; }
        public string emp_id { get; set; }
    }

    public class InoutEmpRealtimeSearchViewModel
    {
        public messageModel message { get; set; }
        public List<InoutEmpRealtimeEmpContentViewModel> content { get; set; }
    }

    public class InoutEmpRealtimeEmpContentViewModel
    {
        public string title { get; set; }
        public List<InoutEmpRealtimeEmpChildsViewModel> childs { get; set; }
    }
    public class InoutEmpRealtimeEmpChildsViewModel
    {
        public string name { get; set; }
        public string detail { get; set; }
        public string time { get; set; }
    }

    public class CheckinOutdoorViewModel
    {
        public string lat { get; set; }
        public string lng { get; set; }
        public string remark { get; set; }
        public List<CheckinOutdoorImgViewModel> img { get; set; }
        public string language { get; set; }
    }

    public class CheckinOutdoorImgViewModel
    {
        public string base64 { get; set; }
    }

    public class CheckInTimeViewModel
    {
        public string language { get; set; }
        public string qrcode { get; set; }
    }

    public class SummaryTimeViewModel
    {
        public messageModel message { get; set; }
        public SummaryTimeHeadViewModel head { get; set; }
        public SummaryTimeBudgetViewModel budget { get; set; }
        public List<SummaryTimeListViewModel> list { get; set; }
    }

    public class SummaryTimeHeadViewModel
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
    }

    public class SummaryTimeBudgetViewModel
    {
        public string All { get; set; }
        public string Default { get; set; }
        public string Late { get; set; }
        public string Leave { get; set; }
        public string La { get; set; }
    }

    public class SummaryTimeListViewModel
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string sts_color { get; set; }
        public string sts_text { get; set; }
    }

    public class SummaryTimeFilterViewModel
    {
        public messageModel message { get; set; }
        public List<SummaryTimeFilterYearViewModel> years { get; set; }
    }

    public class SummaryTimeFilterYearViewModel
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
