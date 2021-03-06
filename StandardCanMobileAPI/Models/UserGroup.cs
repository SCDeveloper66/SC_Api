// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StandardCanMobileAPI.Models
{
    [Table("USER_GROUP")]
    public partial class UserGroup
    {
        [Key]
        [Column("Group_ID")]
        public int GroupId { get; set; }
        [Required]
        [Column("Group_Name")]
        [StringLength(100)]
        public string GroupName { get; set; }
        [Column("Group_Detail")]
        [StringLength(250)]
        public string GroupDetail { get; set; }
        [Column("Group_Status")]
        public bool GroupStatus { get; set; }
    }
}