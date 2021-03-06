// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StandardCanMobileAPI.Models
{
    [Table("MAS_HARDWARE")]
    public partial class MasHardware
    {
        [Key]
        [Column("MHW_ID")]
        public int MhwId { get; set; }
        [Required]
        [Column("MHW_NAME")]
        [StringLength(300)]
        public string MhwName { get; set; }
        [Column("MHW_STATUS")]
        public byte MhwStatus { get; set; }
        [Column("MHW_CREATE_DATE", TypeName = "datetime")]
        public DateTime MhwCreateDate { get; set; }
        [Column("MHW_CREATE_BY")]
        public int MhwCreateBy { get; set; }
        [Column("MHW_UPDATE_DATE", TypeName = "datetime")]
        public DateTime MhwUpdateDate { get; set; }
        [Column("MHW_UPDATE_BY")]
        public int MhwUpdateBy { get; set; }

        [ForeignKey(nameof(MhwCreateBy))]
        [InverseProperty(nameof(Users.MasHardwareMhwCreateByNavigation))]
        public virtual Users MhwCreateByNavigation { get; set; }
        [ForeignKey(nameof(MhwUpdateBy))]
        [InverseProperty(nameof(Users.MasHardwareMhwUpdateByNavigation))]
        public virtual Users MhwUpdateByNavigation { get; set; }
    }
}