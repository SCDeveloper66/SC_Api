using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class TraningViewModel
    {
        public messageModel message { get; set; }
        public List<TraningYearViewModel> year { get; set; }
        public List<TraningProjectViewModel> project { get; set; }
        public List<TraningLotViewModel> lot { get; set; }
    }

    public class TraningYearViewModel
    {
        public string year_id { get; set; }
        public string year_text { get; set; }
    }

    public class TraningProjectViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string year_id { get; set; }
    }

    public class TraningLotViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string prj_id { get; set; }
    }

    public class TraningDetailViewModel
    {
        public messageModel message { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        public List<TraningBudgetViewModel> budget { get; set; }
        public List<TraninglistViewModel> list { get; set; }
    }

    public class TraningBudgetViewModel
    {
        public string all { get; set; }
        public string pass { get; set; }
        public string reject { get; set; }
        public string wait { get; set; }
    }

    public class TraninglistViewModel
    {
        public string name { get; set; }
        public string detail { get; set; }
        public string no { get; set; }
        public string icon_name { get; set; }
        public string icon_color { get; set; }
    }

}
