using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class scheduleCourseModel
    {
        public string method { get; set; }
        public string year_from { get; set; }
        public string year_to { get; set; }
        public string prj_from { get; set; }
        public string prj_to { get; set; }
        public string course_name { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }


        public string id { get; set; }
        //public string ip { get; set; }
        //public string name { get; set; }
        public string status { get; set; }
        public string user_id { get; set; }

        public List<courseDetailModel> data { get; set; } = new List<courseDetailModel>();
    }

    public class courseDetailModel
    {
        public string id { get; set; }
        public string course_id { get; set; }
        public string name { get; set; }
        public string type_name { get; set; }
        public string expert_name { get; set; }
        public string cost_total { get; set; }
        public string emp_total { get; set; }
        public string date { get; set; }
        public string start_time { get; set; }
        public string stop_time { get; set; }
        public List<courseCostDetail> cost_detail { get; set; } = new List<courseCostDetail>();
        public List<sp_schedule_course_emp_v2_Result> emp_detail { get; set; } = new List<sp_schedule_course_emp_v2_Result>();
    }

    public class courseCostDetail
    {
        public string id { get; set; }
        public string name { get; set; }
        public string cost { get; set; }
    }
    public class courseEmpModel
    {
        public string emp_code { get; set; }
        public string course_name { get; set; }
        public string department { get; set; }
        public string full_name { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string score_part_1 { get; set; }
        public string score_part_2 { get; set; }
        public string score_part_3 { get; set; }
        public string score_part_4 { get; set; }
        public string score_part_5 { get; set; }
        public string score_total { get; set; }
        public string score_result { get; set; }
    }

}