using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models.ViewModels
{
    public class RoleViewModel
    {
        public string usergroup_id { get; set; }
        public string usergroup_Desc { get; set; }
        public List<UserRoleProgramViewModel> program_list { get; set; }
        public bool? active { get; set; }

        public string user_id { get; set; }
        public string method { get; set; }
    }

    public class UserRoleProgramViewModel
    {
        public string program_id { get; set; }
        public string program_name { get; set; }
    }

    public class UserRoleDDL
    {
        public List<DDLViewModel> UserGroupList { get; set; }
        public List<DDLViewModel> ProgramList { get; set; }
    }

    public class DDLViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public string active { get; set; }
        public int? order { get; set; }
        public List<UserRoleProgramViewModel> roleList { get; set; } = new List<UserRoleProgramViewModel>();
    }

    public class UserRoleViewModel
    {
        public string userGroupId { get; set; }
        public string userGroupName { get; set; }
        public string active { get; set; }
    }
}