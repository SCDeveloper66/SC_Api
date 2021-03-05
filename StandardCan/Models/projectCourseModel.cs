using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class projectCourseModel
    {
        public string method { get; set; }
        public string id { get; set; }
        public string year_from { get; set; }
        public string year_to { get; set; }
        public string project_from { get; set; }
        public string project_to { get; set; }
        public string course_name { get; set; }

        public string project_id { get; set; }
        public string course_id { get; set; }
        public string formula_id { get; set; }
        public string location_id { get; set; }
        public string expert_id { get; set; }
        public string status_id { get; set; }
        public string file_url { get; set; }
        public string file_name { get; set; }
        public string file_base64 { get; set; }
        public string remark { get; set; }
        public string score1 { get; set; }
        public string score2 { get; set; }
        public string score3 { get; set; }
        public string score4 { get; set; }
        public string score5 { get; set; }

        public string user_id { get; set; }



    }

    public class projectCourseDetailModel
    {
        public List<sp_project_course_detail_v4_Result> data { get; set; } = new List<sp_project_course_detail_v4_Result>();
        public List<dropdown> project { get; set; } = new List<dropdown>();
        public List<dropdown> location { get; set; } = new List<dropdown>();
        public List<dropdown> expert { get; set; } = new List<dropdown>();
        public List<dropdown> formula { get; set; } = new List<dropdown>();
    }

    public class projectCourseMasterModel
    {
        public List<dropdown> year { get; set; } = new List<dropdown>();
        public List<dropdown> project { get; set; } = new List<dropdown>();
    }



}