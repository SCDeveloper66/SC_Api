using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models
{
    public class projectFormularModel
    {
        public string method { get; set; }
        public string fml_id { get; set; }
        public string fml_name { get; set; }
        public string fml_type { get; set; }
        public string fml_input_type { get; set; }
        public List<formularRangeModel> formularRange { get; set; } = new List<formularRangeModel>();
        public List<formularValueModel> formularValue { get; set; } = new List<formularValueModel>();
        public List<sp_formularrange_search_Result> range { get; set; } = new List<sp_formularrange_search_Result>();
        public List<sp_formulavalue_search_Result> value { get; set; } = new List<sp_formulavalue_search_Result>();
        public string user_id { get; set; }
    }

    public class formularRangeModel
    {
        public string fml_range_no { get; set; }

        public string fml_range_score { get; set; }

        public string fml_range_display { get; set; }

    }
    public class formularValueModel
    {
        public string fmlv_orderby { get; set; }

        public string fmlv_value { get; set; }

        public string fmlv_text { get; set; }
    }

    public class projectFormulaMasterModel
    {
        public List<dropdown> type { get; set; } = new List<dropdown>(); 
    }

}