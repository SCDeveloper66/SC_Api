using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
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

    public class BenefitLoad
    {
        public messageModel message { get; set; }
        public List<BenefitContent> content { get; set; }
    }


    public class BenefitContent
    {
        public string title { get; set; }
        public List<BenefitContentChlids> chlids { get; set; }
    }

    public class BenefitContentChlids
    {
        public string title { get; set; }
        public string sum { get; set; }
        public string now { get; set; }
        public string used { get; set; }
        public string current { get; set; }
    }

    public class BenefitDepartmentViewModel
    {
        public messageModel message { get; set; }
        public BenefitDepartmentHeadViewModel head { get; set; }
        public BenefitDepartmentBudgetViewModel budget { get; set; }
        public List<BenefitDepartmentListViewModel> quota_list { get; set; }
        public List<BenefitDepartmentListViewModel> hours_list { get; set; }
    }

    public class BenefitDepartmentHeadViewModel
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
    }

    public class BenefitDepartmentBudgetViewModel
    {
        public string quota { get; set; }
        public string hours { get; set; }
    }

    public class BenefitDepartmentListViewModel
    {
        public string title_group { get; set; }
        public List<BenefitDepartmentChildsViewModel> childs { get; set; }
    }

    public class BenefitDepartmentChildsViewModel
    {
        public string title { get; set; }
        public string value { get; set; }
        public string value_color { get; set; }
    }

    public class BenefitMasterLoad
    {
        public messageModel message { get; set; }
        public List<BenefitEmpsMasterLoad> emps { get; set; }
        public List<BenefitStatusMasterLoad> status { get; set; }
    }

    public class BenefitEmpsMasterLoad
    {
        public string name { get; set; }
        public string emp_id { get; set; }
    }

    public class BenefitStatusMasterLoad
    {
        public string name { get; set; }
        public string sts_id { get; set; }
    }

    public class BenefitLeave
    {
        public messageModel message { get; set; }
        public BenefitDepartmentHeadViewModel head { get; set; }
        public List<BenefitLeaveListViewModel> list { get; set; }
    }

    public class BenefitLeaveListViewModel
    {
        public string title_group { get; set; }
        public List<BenefitLeaveChildsViewModel> childs { get; set; }
    }

    public class BenefitLeaveChildsViewModel
    {
        public string title { get; set; }
        public string detail { get; set; }
        public string appr_status { get; set; }
        public string color { get; set; }
    }

}
