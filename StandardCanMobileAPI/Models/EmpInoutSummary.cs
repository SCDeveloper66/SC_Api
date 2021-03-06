// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StandardCanMobileAPI.Models
{
    [Table("EMP_INOUT_SUMMARY")]
    public partial class EmpInoutSummary
    {
        [Key]
        [Column("ETS_ID")]
        public int EtsId { get; set; }
        [Column("emp_code")]
        [StringLength(20)]
        public string EmpCode { get; set; }
        [Column("ets_date", TypeName = "date")]
        public DateTime? EtsDate { get; set; }
        [Column("ets_shift")]
        [StringLength(10)]
        public string EtsShift { get; set; }
        [Column("ets_time_in")]
        [StringLength(5)]
        public string EtsTimeIn { get; set; }
        [Column("ets_time_in_status")]
        [StringLength(30)]
        public string EtsTimeInStatus { get; set; }
        [Column("ets_time_out")]
        [StringLength(5)]
        public string EtsTimeOut { get; set; }
        [Column("ets_time_out_status")]
        [StringLength(30)]
        public string EtsTimeOutStatus { get; set; }
        [Column("ets_type")]
        [StringLength(30)]
        public string EtsType { get; set; }
        [Column("ets_update_date", TypeName = "datetime")]
        public DateTime? EtsUpdateDate { get; set; }
    }
}