using StandardCan.jwt;
using StandardCan.Models;
using StandardCan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Service
{
    public class SettingService
    {
        public messageModel saveUserRole(RoleViewModel role)
        {
            messageModel result = new messageModel();

            try
            {
                if (String.IsNullOrEmpty(role.user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(role.user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardCanEntities())
                {
                    var dt = DateTime.Now;
                    if (String.IsNullOrEmpty(role.usergroup_id))
                    {
                        var checkName = context.USER_GROUP.SingleOrDefault(a => a.Group_Name.ToUpper() == role.usergroup_Desc.ToUpper());
                        if (checkName != null)
                        {
                            throw new Exception("Data is duplicate");
                        }
                        USER_GROUP uSER_GROUP = new USER_GROUP();
                        uSER_GROUP.Group_Name = role.usergroup_Desc.ToUpper();
                        uSER_GROUP.Active = role.active ?? false;
                        context.USER_GROUP.Add(uSER_GROUP);
                        context.SaveChanges();
                        if (role.program_list != null)
                        {
                            foreach (var item in role.program_list)
                            {
                                USER_ROLE data = new USER_ROLE();
                                data.Group_ID = uSER_GROUP.Group_ID.ToString();
                                data.Program_ID = item.program_id;
                                data.Active = role.active ?? false;
                                data.Create_Date = dt;
                                data.Create_By = Convert.ToInt32(userId);
                                data.Update_Date = dt;
                                data.Update_By = Convert.ToInt32(userId);
                                context.USER_ROLE.Add(data);
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var checkName = context.USER_GROUP.SingleOrDefault(a => a.Group_Name.ToUpper() == role.usergroup_Desc.ToUpper() && a.Group_ID.ToString() != role.usergroup_id);
                        if (checkName != null)
                        {
                            throw new Exception("Data is duplicate");
                        }
                        var userGroupDetail = context.USER_GROUP.SingleOrDefault(a =>a.Group_ID.ToString() == role.usergroup_id);
                        if(userGroupDetail == null)
                        {
                            throw new Exception("Data is not found");
                        }
                        userGroupDetail.Group_Name = role.usergroup_Desc.ToUpper();
                        userGroupDetail.Active = role.active ?? false;
                        context.SaveChanges();

                        var roleOldList = context.USER_ROLE.Where(a => a.Group_ID == role.usergroup_id).ToList();
                        context.USER_ROLE.RemoveRange(roleOldList);
                        context.SaveChanges();

                        if (role.program_list != null)
                        {
                            foreach (var item in role.program_list)
                            {
                                USER_ROLE data = new USER_ROLE();
                                data.Group_ID = userGroupDetail.Group_ID.ToString();
                                data.Program_ID = item.program_id;
                                data.Active = role.active ?? false;
                                data.Create_Date = dt;
                                data.Create_By = Convert.ToInt32(userId);
                                data.Update_Date = dt;
                                data.Update_By = Convert.ToInt32(userId);
                                context.USER_ROLE.Add(data);
                                context.SaveChanges();
                            }
                        }
                    }
                }

                result.status = "S";
                result.message = "";
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public UserRoleDDL getDDLData()
        {
            UserRoleDDL result = new UserRoleDDL();
            using (var context = new StandardCanEntities())
            {
                result.UserGroupList = new List<DDLViewModel>();
                var programList = context.MAS_PROGRAM.ToList();
                result.UserGroupList = context.USER_GROUP.Select(a => new DDLViewModel
                {
                    id = a.Group_ID.ToString(),
                    name = a.Group_Name,
                    detail = a.Group_Name,
                    active = a.Active == true ? "True" : "False",
                    roleList = context.USER_ROLE.Where(x => x.Group_ID == a.Group_ID.ToString()).Select(r => new UserRoleProgramViewModel
                    {
                        program_id = r.Program_ID,
                       // program_name = programList.SingleOrDefault(x => x.Program_ID.ToString() == r.Program_ID) != null ? programList.SingleOrDefault(x => x.Program_ID.ToString() == r.Program_ID).Program_Name : ""
                    }).ToList()
                }).ToList();
                result.ProgramList = new List<DDLViewModel>();
                result.ProgramList = context.MAS_PROGRAM.Where(a => a.Active).Select(a => new DDLViewModel
                {
                    id = a.Program_ID.ToString(),
                    name = a.Program_Name,
                    detail = a.Url_Path,
                    order = a.Order_Item
                }).OrderBy(a => a.order.Value).ToList();
            }

            return result;
        }

        public List<UserRoleViewModel> searchUserGroup()
        {
            List<UserRoleViewModel> result = new List<UserRoleViewModel>();
            using (var context = new StandardCanEntities())
            {
                //result = context.USER_GROUP.Where().Select(a => new UserRoleViewModel
                //{
                //    roleId = a.role,
                //    Name = a.OfficeName
                //}).ToList();
                //var data = new List<USER_ROLE>();
                //if (String.IsNullOrEmpty(role.usergroup))
                //{
                //    data = context.USER_ROLE.ToList();
                //}
                //else
                //{
                //    data = context.USER_ROLE.Where(a => a.Group_ID.Equals(role.group_id)).ToList();
                //}
                //foreach (var item in data)
                //{
                //    UserRoleViewModel model = new UserRoleViewModel();
                //    model.roleId = item.Role_ID.ToString();
                //    model.userGroupId = item.Group_ID;
                //    model.userGroup = context.USER_GROUP.SingleOrDefault(a => a.Group_Name == item.Group_ID).Group_Detail;
                //    model.programId = item.Program_ID;
                //    model.programName = item.Program_Name;
                //    model.active = item.Active ? "True" : null;
                //    result.Add(model);
                //}
            }

            return result;
        }

    }
}