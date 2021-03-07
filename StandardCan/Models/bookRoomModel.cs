using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class bookRoomModel
    {
        public string method { get; set; }

        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }

        public string room_from { get; set; }
        public string room_to { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }
        public string status_from { get; set; }
        public string status_to { get; set; }

        public string user_id { get; set; }
         
    }

    public class bookRoomDetailModel
    {
        public string method { get; set; }
        public bookRoomButtonModel button { get; set; } = new bookRoomButtonModel();
        public string id { get; set; }
        public string room_name { get; set; }
        public string topic { get; set; }
        public string date { get; set; }
        public string start_time { get; set; }
        public string stop_time { get; set; }
        public string person_total { get; set; }
        public string room_id { get; set; }
        public string br_status { get; set; }
        public string br_request { get; set; }
        public string br_request_name { get; set; }
        public List<dropdown> room_list { get; set; } = new List<dropdown>();
        public List<checkbox_list> device_list { get; set; } = new List<checkbox_list>();
        public string remark { get; set; }
        public List<sp_bookroom_emp_Result> emp_list { get; set; } = new List<sp_bookroom_emp_Result>();
        public string user_id { get; set; }
        public Boolean read_only { get; set; }
    }

    public class bookRoomButtonModel
    {
        public Boolean reject { get; set; }
        public Boolean approve { get; set; }
        public Boolean cancel { get; set; }
        public Boolean save_draft { get; set; }
        public Boolean save_send { get; set; }
        public Boolean save_revise { get; set; }
        public Boolean gen_qrCode { get; set; }
    }

    public class bookRoomEmpModel
    {
        public string emp_code { get; set; }
        public string depart { get; set; }
        public string name { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string checkin_date { get; set; }
        public string checkin_time { get; set; }
        public string checkin_status { get; set; }
    }

    public class checkbox_list
    {
        public string id { get; set; }
        public string text { get; set; }
        public Boolean opt { get; set; } = false;

    }

    public class bookRoomMasterModel
    {
        //public List<dropdown> roomType { get; set; } = new List<dropdown>();
        public List<dropdown> room { get; set; } = new List<dropdown>();
        public List<dropdown> status { get; set; } = new List<dropdown>();
    }

    public class calendarMeetingRoomViewModel
    {
        public string roomName { get; set; }
        public string qrCode { get; set; }
        public string roomTopic { get; set; }
        public List<sp_bookroom_calendar_Result> dataList { get; set; } = new List<sp_bookroom_calendar_Result>();
    }

}