using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Models
{
    public partial class TimeAttImage
    {
        public int TariId { get; set; }
        public int? TarId { get; set; }
        public string TariImage { get; set; }
        public DateTime? TariUpdateDate { get; set; }
    }
}
