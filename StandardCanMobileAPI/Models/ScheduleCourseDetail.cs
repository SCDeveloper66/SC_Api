// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StandardCanMobileAPI.Models
{
    [Table("SCHEDULE_COURSE_DETAIL")]
    public partial class ScheduleCourseDetail
    {
        [Key]
        [Column("SCD_ID")]
        public int ScdId { get; set; }
        [Column("SC_ID")]
        public int ScId { get; set; }
        [Column("MCS_ID")]
        public int McsId { get; set; }
        [Column("scd_status")]
        public byte? ScdStatus { get; set; }
        [Column("scd_cost_total")]
        public double? ScdCostTotal { get; set; }
        [Column("scd_emp_total")]
        public int? ScdEmpTotal { get; set; }
        [Column("scd_date", TypeName = "date")]
        public DateTime? ScdDate { get; set; }
        [Column("scd_start_time")]
        [StringLength(5)]
        public string ScdStartTime { get; set; }
        [Column("scd_stop_time")]
        [StringLength(5)]
        public string ScdStopTime { get; set; }
        [Column("scd_create_date", TypeName = "datetime")]
        public DateTime? ScdCreateDate { get; set; }
        [Column("scd_create_by")]
        [StringLength(20)]
        public string ScdCreateBy { get; set; }
        [Column("scd_update_date", TypeName = "datetime")]
        public DateTime? ScdUpdateDate { get; set; }
        [Column("scd_update_by")]
        [StringLength(20)]
        public string ScdUpdateBy { get; set; }
        [Column("token_update")]
        [StringLength(50)]
        public string TokenUpdate { get; set; }
    }
}