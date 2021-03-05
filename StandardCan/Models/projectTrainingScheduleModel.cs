using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class projectTrainingScheduleModel
    {
        public string method { get; set; }
        public string id { get; set; }

        public string course_id { get; set; }

        public string project_year { get; set; }
        public string project_id { get; set; }
        public string project_name { get; set; }
        public string training_id { get; set; }
        public string training_name { get; set; }
        public string date_form { get; set; }
        public string date_to { get; set; }
        public byte training_status { get; set; }

        // Search Course
        public string searchCourse_project { get; set; }
        public string searchCourse_name { get; set; }
        // Search Destination
        public string searchDestination_name { get; set; }
        // search Expert
        public string searchExpert_name { get; set; }
        // search Emp
        public string searchEmp_code { get; set; }
        public string searchEmp_name { get; set; }
        public string searchEmp_depart { get; set; }

        public string user_id { get; set; }
        public string status_id { get; set; }
        public List<projectTrainingScheduleItemModel> training_item { get; set; }
    }


    public class projectTrainingScheduleItemModel
    {
        public string course_id { get; set; }
        public string destination_id { get; set; }
        public string item_date { get; set; }
        public string time_start { get; set; }
        public string time_stop { get; set; }
        public List<projectTrainingScheduleExpertModel> expert_list { get; set; }
        public List<projectTrainingScheduleExpenseModel> expense_list { get; set; }
        public List<projectTrainingScheduleEmpModel> emp_list { get; set; }
    }

    public class projectTrainingScheduleExpertModel
    {
        public string expert_id { get; set; }
    }

    public class projectTrainingScheduleExpenseModel
    {
        public string expense_id { get; set; }
        public string expense_price { get; set; }
    }

    public class projectTrainingScheduleEmpModel
    {
        public int emp_id { get; set; }
        public string emp_code { get; set; }
        public string emp_score1 { get; set; }
        public string emp_score2 { get; set; }
        public string emp_score3 { get; set; }
        public string emp_score4 { get; set; }
        public string emp_score5 { get; set; }
        public string emp_score_total { get; set; }
        public string emp_result { get; set; }
    }


    public class projectTrainingMasterModel
    {
        public List<dropdown> project_year { get; set; } = new List<dropdown>();
        public List<dropdown> project_name { get; set; } = new List<dropdown>();
        public List<projectTrainingExpenseModel> project_expense { get; set; } = new List<projectTrainingExpenseModel>();
        public List<sp_depart_search_Result> depart { get; set; } = new List<sp_depart_search_Result>();
    }

    public class projectTrainingExpenseModel
    {
        public string id { get; set; }
        public string expense_name { get; set; }
    }

    public class projectTrainingSearchModel
    {
        public messageModel message { get; set; }
        public List<projectTrainingSearchResult> data_list { get; set; }
    }

    public class projectTrainingSearchResult
    {
        public string traning_id { get; set; }
        public string traning_name { get; set; }
        public string date_form { get; set; }
        public string date_To { get; set; }
        public string course_count { get; set; }
        public string expense_total { get; set; }
    }

    public class projectTrainingDetailResult
    {
        public messageModel message { get; set; }
        public projectTrainingDetailDataResult data { get; set; }
    }

    public class projectFomularResult
    {
        public messageModel message { get; set; }
        public List<fomularField> data { get; set; } = new List<fomularField>();
    }

    public class fomularField
    {
        public int order_by { get; set; }
        public string value { get; set; }
        public string text { get; set; }        
    }

    public class projectTrainingDetailDataResult
    {
        public string training_id { get; set; }
        public string training_name { get; set; }
        public string date_form { get; set; }
        public string date_to { get; set; }
        public string time_start { get; set; }
        public string time_stop { get; set; }
        public string training_status { get; set; }
        public List<projectTrainingItemDetailDataResult> training_item { get; set; }
    }

    public class projectTrainingItemDetailDataResult
    {
        public string training_item_id { get; set; }
        public string course_id { get; set; }
        public string course_name { get; set; }
        public string course_type { get; set; }
        public string destination_id { get; set; }
        public string destination_name { get; set; }
        public string item_date { get; set; }
        public string time_start { get; set; }
        public string time_stop { get; set; }
        public List<projectTrainingItemDetailExpertDataResult> expert_list { get; set; }
        public List<projectTrainingItemDetailExpenseDataResult> expense_list { get; set; }
        public string expense_total { get; set; }
        public List<projectTrainingItemDetailEmpDataResult> emp_list { get; set; }
    }

    public class projectTrainingItemDetailExpertDataResult
    {
        public string expert_id { get; set; }
        public string expert_name { get; set; }
    }

    public class projectTrainingItemDetailExpenseDataResult
    {
        public string expense_id { get; set; }
        public string expense_name { get; set; }
        public string expense_price { get; set; }
    }

    public class projectTrainingItemDetailEmpDataResult
    {
        public string emp_code { get; set; }
        public string emp_depart { get; set; }
        public string emp_name { get; set; }
        public string emp_tel { get; set; }
        public string emp_email { get; set; }
        public string emp_score1 { get; set; }
        public string emp_score2 { get; set; }
        public string emp_score3 { get; set; }
        public string emp_score4 { get; set; }
        public string emp_score5 { get; set; }
        public string emp_score_total { get; set; }
        public string emp_result { get; set; }
    }

}