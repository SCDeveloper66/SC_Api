// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StandardCanMobileAPI.Models
{
    [Table("MAS_NODE")]
    public partial class MasNode
    {
        [Key]
        [Column("MN_ID")]
        public int MnId { get; set; }
        [Column("mn_ip")]
        [StringLength(20)]
        public string MnIp { get; set; }
        [Column("mn_name")]
        [StringLength(200)]
        public string MnName { get; set; }
        [Column("mn_refid")]
        [StringLength(50)]
        public string MnRefid { get; set; }
        [Column("mn_status")]
        public byte? MnStatus { get; set; }
        [Column("mn_create_by")]
        [StringLength(20)]
        public string MnCreateBy { get; set; }
        [Column("mn_create_date", TypeName = "datetime")]
        public DateTime? MnCreateDate { get; set; }
        [Column("mn_update_by")]
        [StringLength(20)]
        public string MnUpdateBy { get; set; }
        [Column("mn_update_date", TypeName = "datetime")]
        public DateTime? MnUpdateDate { get; set; }
        [Column("mn_job_process", TypeName = "datetime")]
        public DateTime? MnJobProcess { get; set; }
    }
}