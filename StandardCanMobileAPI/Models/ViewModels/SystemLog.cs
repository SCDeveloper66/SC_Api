using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models.ViewModels
{
    public class SystemLog
    {
        public string ID { get; set; }
        public string module { get; set; }
        public string data_log { get; set; }
        public DateTime? update_date { get; set; }
    }
}
