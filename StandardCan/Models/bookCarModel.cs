using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class bookCarModel
    {
        public string method { get; set; }

        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }

        public string car_type_from { get; set; }
        public string car_type_to { get; set; }
        public string car_license_from { get; set; }
        public string car_license_to { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }
        public string car_reason_from { get; set; }
        public string car_reason_to { get; set; }
        public string status_from { get; set; }
        public string status_to { get; set; }

        public string room_from { get; set; }
        public string room_to { get; set; }

        public string user_id { get; set; }

    }

    public class bookCarDetailModel
    {
        public string method { get; set; }
        public bookRoomButtonModel button { get; set; } = new bookRoomButtonModel();
        public string id { get; set; }
        public string topic { get; set; }
        public string start_date { get; set; }
        public string start_time { get; set; }
        public string stop_date { get; set; }
        public string stop_time { get; set; }
        public string person_total { get; set; }
        public string car_type_id { get; set; }
        public string car_id { get; set; }
        public string reason_id { get; set; }
        public string dest_id { get; set; }
        public string remark { get; set; }
        public string bc_request { get; set; }
        public string bc_request_name { get; set; }
        public List<sp_bookcar_emp_Result> emp_list { get; set; } = new List<sp_bookcar_emp_Result>();
        public List<dropdown> car_reason { get; set; } = new List<dropdown>();
        public List<dropdown> car_type { get; set; } = new List<dropdown>();
        public List<dropdown> car_license { get; set; } = new List<dropdown>();
        public List<dropdown> car_dest { get; set; } = new List<dropdown>();

        public string user_id { get; set; }
        public Boolean read_only { get; set; }
    }

    public class bookCarButtonModel
    {
        public Boolean reject { get; set; }
        public Boolean approve { get; set; }
        public Boolean cancel { get; set; }
        public Boolean save_draft { get; set; }
        public Boolean save_send { get; set; }
    }

    public class bookCarEmpModel
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
     

    public class bookCarMasterModel
    {
        //public List<dropdown> roomType { get; set; } = new List<dropdown>();
        public List<dropdown> room { get; set; } = new List<dropdown>();
        public List<dropdown> status { get; set; } = new List<dropdown>();
    }


}