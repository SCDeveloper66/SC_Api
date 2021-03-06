// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StandardCanMobileAPI.Models
{
    [Table("MAS_CAR")]
    public partial class MasCar
    {
        [Key]
        [Column("MCA_ID")]
        public int McaId { get; set; }
        [Column("MCT_ID")]
        public int? MctId { get; set; }
        [Column("MCA_NAME")]
        [StringLength(50)]
        public string McaName { get; set; }
        [Column("MCA_DETAIL")]
        [StringLength(200)]
        public string McaDetail { get; set; }
        [Column("MCA_STATUS")]
        public byte? McaStatus { get; set; }
        [Column("MCA_CREATE_BY")]
        public int? McaCreateBy { get; set; }
        [Column("MCA_CREATE_DATE", TypeName = "datetime")]
        public DateTime? McaCreateDate { get; set; }
        [Column("MCA_UPDATE_BY")]
        public int? McaUpdateBy { get; set; }
        [Column("MCA_UPDATE_DATE", TypeName = "datetime")]
        public DateTime? McaUpdateDate { get; set; }
    }
}