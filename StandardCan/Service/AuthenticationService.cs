using Microsoft.IdentityModel.Tokens;
using StandardCan.Helper;
using StandardCan.Models;
using StandardCan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;
using StandardCan.jwt;
using Newtonsoft.Json;

namespace StandardCan.Service
{
    public class AuthenticationService
    {
        const string secrectKey = "StandardCan_webapplication";

        public messageAuthenticationModel Login(string username, string password)
        {
            messageAuthenticationModel result = new messageAuthenticationModel();
            try
            {
                using (var context = new StandardCanEntities())
                {
                    result.message = new messageModel();
                    var passEncrypt = Cipher.Encrypt(password, secrectKey);
                    var userGroupList = context.USER_GROUP.ToList();
                    var empDetail = context.EMP_PROFILE.SingleOrDefault(a => a.emp_user_name.ToUpper() == username.ToUpper() && a.emp_password == passEncrypt);
                    if (empDetail != null)
                    {
                        var countBoss = context.EMP_PROFILE.Where(a => a.emp_boss_id == empDetail.EMP_ID).ToList().Count();
                        result.message.status = "1";
                        result.message.message = "";
                        var groupName = userGroupList.SingleOrDefault(a => a.Group_ID == empDetail.emp_group);
                        if (countBoss > 0)
                        {
                            result.permission = "2";
                        }
                        else if (groupName.Group_Name == "พนักงาน")
                        {
                            result.permission = "1";
                        }
                        else if (groupName.Group_Name == "ผู้ดูแล")
                        {
                            result.permission = "3";
                        }
                        else if (groupName.Group_Name == "บัญชี")
                        {
                            result.permission = "4";
                        }
                        result.token_login = CreateToken(empDetail.EMP_ID.ToString(), result.permission, groupName.Group_Name);
                    }
                    else
                    {
                        throw new Exception("The username or password is incorrect");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The username or password is incorrect");
            }

            return result;
        }

        public messageAuthenticationModel LoginToken(string token)
        {
            messageAuthenticationModel result = new messageAuthenticationModel();
            try
            {
                using (var context = new StandardCanEntities())
                {
                    result.message = new messageModel();
                    var userId = JwtHelper.GetUserIdFromToken(token);
                    //var passEncrypt = Cipher.Encrypt(password, secrectKey);
                    var userGroupList = context.USER_GROUP.ToList();
                    var empDetail = context.EMP_PROFILE.SingleOrDefault(a => a.EMP_ID.ToString() == userId);
                    if (empDetail != null)
                    {
                        var countBoss = context.EMP_PROFILE.Where(a => a.emp_boss_id == empDetail.EMP_ID).ToList().Count();
                        result.message.status = "1";
                        result.message.message = "";
                        var groupName = userGroupList.SingleOrDefault(a => a.Group_ID == empDetail.emp_group);
                        if (countBoss > 0)
                        {
                            result.permission = "2";
                        }
                        else if (groupName.Group_Name == "EMP")
                        {
                            result.permission = "1";
                        }
                        else if (groupName.Group_Name == "ADMIN")
                        {
                            result.permission = "3";
                        }
                        else if (groupName.Group_Name == "ACC")
                        {
                            result.permission = "4";
                        }
                        result.token_login = CreateToken(empDetail.EMP_ID.ToString(), result.permission, groupName.Group_Name);
                    }
                    else
                    {
                        throw new Exception("The username or password is incorrect");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The username or password is incorrect");
            }

            return result;
        }


        private string CreateToken(string userId, string permission, string userGroup_Name)
        {
            var tokenString = "";
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddYears(1);

            using (var context = new StandardCanEntities())
            {
                var empDepartment = context.EMP_PROFILE.FirstOrDefault(a => a.emp_boss_id.ToString() == userId);

                var empDetail = context.EMP_PROFILE.SingleOrDefault(a => a.EMP_ID.ToString() == userId);
                var tokenHandler = new JwtSecurityTokenHandler();
                var userGroupDetail = context.USER_GROUP.SingleOrDefault(a =>a.Group_Name == userGroup_Name);
                var roleList = context.USER_ROLE.Where(a => a.Group_ID == userGroupDetail.Group_ID.ToString()).ToList();
                var programList = context.MAS_PROGRAM.Where(a => a.Active).ToList();
                if (empDetail != null)
                {
                    var claimsIdentity = new ClaimsIdentity(new[]
                    {
                        new Claim("userId", empDetail.EMP_ID.ToString()),
                        new Claim("userCode", empDetail.emp_code),
                        new Claim("userName", empDetail.emp_user_name),
                        new Claim("firstName", empDetail.emp_fname),
                        new Claim("lastName", empDetail.emp_lname),
                        new Claim("fullname", empDetail.emp_title + empDetail.emp_fname + " " + empDetail.emp_lname),
                        new Claim("userGroup", permission),
                        new Claim("isDP", empDepartment == null ? "0" : "1"),
                        new Claim("img", ""),
                        new Claim("roleList", JsonConvert.SerializeObject(roleList.Select(s => new {
                        roleId = s.Role_ID,
                        userGroupId = s.Group_ID,
                        programGroupId = programList.SingleOrDefault(a =>a.Program_ID.ToString() == s.Program_ID)?.Program_Group_Id,
                        programGroupName = programList.SingleOrDefault(a =>a.Program_ID.ToString() == s.Program_ID)?.Program_Group_Name,
                        programId = s.Program_ID,
                        programName = programList.SingleOrDefault(a =>a.Program_ID.ToString() == s.Program_ID)?.Program_Name,
                        url = programList.SingleOrDefault(a =>a.Program_ID.ToString() == s.Program_ID)?.Url_Path,
                        }))),
                    });

                    var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secrectKey));
                    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                    var token =
                        (JwtSecurityToken)
                        tokenHandler.CreateJwtSecurityToken(
                            issuer: "Standard-Can",
                            audience: "StandardCan",
                            subject: claimsIdentity,
                            notBefore: issuedAt,
                            expires: expires,
                            signingCredentials: signingCredentials
                            );
                    tokenString = tokenHandler.WriteToken(token);
                }
                else
                {
                    throw new Exception("The username or password is incorrect");
                }
            }

            return tokenString;
        }

    }
}