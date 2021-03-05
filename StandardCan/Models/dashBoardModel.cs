using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class dashBoardModel
    {
        public messageModel message { get; set; }
        public List<dashBoardImg> imgList { get; set; }
        public List<string> select_room_list { get; set; }
        public List<string> select_car_list { get; set; }
        public string user_id { get; set; }
        public string draft { get; set; }
        public string pending { get; set; }
        public string watiDP { get; set; }
        public string approve { get; set; }
        public string cancel { get; set; }
        public string method { get; set; }
        public string remark { get; set; }
    }

    public class dashBoardImg
    {
        public string base64 { get; set; }
    }

    public class dashBoardCalendarViewModel
    {
        public string type { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string id { get; set; }
        public string color { get; set; }
    }

    public class dashBoardWorkListViewModel
    {
        public string no { get; set; }
        public string booking_create { get; set; }
        public string booking_type { get; set; }
        public string booking_name { get; set; }
        public string create_by { get; set; }
        public string department { get; set; }
        public string booking_date { get; set; }
        public string booking_time { get; set; }
        public string booking_status { get; set; }
        public string approve_by { get; set; }
        public string is_approve { get; set; }
        public string is_cancel { get; set; }
        public string reason { get; set; }
    }

}