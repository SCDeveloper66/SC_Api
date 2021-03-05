﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StandardCanMobileAPI.Models
{
    [Table("xCNF_MEETING")]
    public partial class XCnfMeeting
    {
        [Key]
        [Column("CMT_ID")]
        public int CmtId { get; set; }
        [Column("CMT_NAME")]
        [StringLength(200)]
        public string CmtName { get; set; }
        [Column("CMT_STATUS")]
        public byte? CmtStatus { get; set; }
        [Column("CMT_SCORE")]
        public double? CmtScore { get; set; }
        [Column("CMT_CREATE_BY")]
        public int? CmtCreateBy { get; set; }
        [Column("CMT_CREATE_DATE", TypeName = "datetime")]
        public DateTime? CmtCreateDate { get; set; }
        [Column("CMT_UPDATE_BY")]
        public int? CmtUpdateBy { get; set; }
        [Column("CMT_UPDATE_DATE", TypeName = "datetime")]
        public DateTime? CmtUpdateDate { get; set; }
    }
}