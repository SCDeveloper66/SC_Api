// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StandardCanMobileAPI.Models
{
    [Table("MAS_LEAVE")]
    public partial class MasLeave
    {
        [Key]
        [Column("ML_ID")]
        public int MlId { get; set; }
        [Column("ml_name")]
        [StringLength(50)]
        public string MlName { get; set; }
    }
}