using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class employeeModel
    {
        public string method { get; set; }

        public string emp_code { get; set; }
        public string emp_name { get; set; }
        public string sh_id { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public string note { get; set; }

        public string id { get; set; }
        public string date { get; set; }
        public string card_no { get; set; }
        public string status { get; set; }

        public string no { get; set; }
        public string grade { get; set; }

        public string detail { get; set; }
        public string detail_id { get; set; }
        public string score { get; set; }

        public string emp_code_start { get; set; }
        public string emp_code_stop { get; set; }
        public string sh_start { get; set; }
        public string sh_stop { get; set; }
        public string depart_start { get; set; }
        public string depart_stop { get; set; }
        public string emp_status_start { get; set; }
        public string emp_status_stop { get; set; }
        public string emp_fname { get; set; }
        public string emp_lname { get; set; }
        public string head_fname { get; set; }
        public string head_lname { get; set; }
        public string img { get; set; }
        public string user_id { get; set; }
    }

    public class empTabNoteModel
    {
        public string note { get; set; }
    }

    public class empTab3Model
    {
        public List<sp_emp_profile_tab3_quota_Result> quota { get; set; } = new List<sp_emp_profile_tab3_quota_Result>();
        public List<sp_emp_profile_tab3_Result> data { get; set; } = new List<sp_emp_profile_tab3_Result>();
    }

    public class BenefitViewModel
    {
        public messageModel message { get; set; }
        public string quota { get; set; }
        public string hours { get; set; }
        public List<Benefitline> quota_list { get; set; }
        public List<Benefitline> hours_list { get; set; }
    }
    public class Benefitline
    {
        public string line1 { get; set; }
        public string hours { get; set; }
    }

    public class empTab2Model
    {
        public string status1 { get; set; }
        public string status2 { get; set; }
        public string status3 { get; set; }
        public List<sp_emp_profile_tab2_Result> data { get; set; } = new List<sp_emp_profile_tab2_Result>();
    }

    public class empMasterModel
    {
        public List<sp_sh_search_Result> sh { get; set; } = new List<sp_sh_search_Result>();
        public List<sp_depart_search_Result> depart { get; set; } = new List<sp_depart_search_Result>();
        public List<sp_emp_status_search_Result> emp_status { get; set; } = new List<sp_emp_status_search_Result>();
    }

    public class leaveStatusModel
    {

    }

    public class dropdown
    {
        public string code { get; set; }
        public string text { get; set; }
    }

    public class empLeave
    {
        public string no { get; set; }
        public string id { get; set; }
        public string emp_code { get; set; }
        public string emp_name { get; set; }
        public string leave_start { get; set; }
        public string leave_stop { get; set; }
        public string sts_color { get; set; }
        public string sts_text { get; set; }
        public string remark { get; set; }
        public string typeLeave { get; set; }
    }
}