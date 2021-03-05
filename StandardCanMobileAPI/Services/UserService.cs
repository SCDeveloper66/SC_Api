using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StandardCanMobileAPI.Helper;
using StandardCanMobileAPI.Models;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services
{
    public class UserService : IUserService
    {
        const string secrectKey = "StandardCan_webapplication";
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        private readonly ISystemLogService _systemLogService;
        public UserService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISystemLogService systemLogService)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _systemLogService = systemLogService;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;

        public async Task<JsonWebToken> UserLogin(string username, string password, string token_noticiation)
        {
            var jwt = new JsonWebToken();
            jwt.message = new messageModel();
            using (var context = new StandardcanContext())
            {
                try
                {
                    var passEncrypt = Cipher.Encrypt(password, secrectKey);
                    var empDetail = context.EmpProfile.SingleOrDefault(a => a.EmpUserName.ToUpper() == username.ToUpper());
                    if (empDetail != null)
                    {
                        jwt.Pass_isdefault = empDetail.EmpPassIsdefault.ToString();
                    }

                    if (empDetail == null)
                    {
                        jwt.message.status = "3";
                        throw new Exception("The username or password is incorrect");
                    }
                    else if(empDetail.EmpPassword != passEncrypt)
                    {
                        throw new Exception("The username or password is incorrect");
                    }

                    var countBoss = context.EmpProfile.Where(a => a.EmpBossId == empDetail.EmpId).ToList().Count();
                    if (empDetail != null)
                    {
                        if (countBoss > 0)
                        {
                            jwt.Permission = "2";
                        }
                        else
                        {
                            jwt.Permission = "1";
                        }
                        var token = CreateToken(empDetail.EmpId.ToString(), jwt.Permission);
                        try
                        {
                            SqlParameter emp_id = new SqlParameter("emp_id", empDetail.EmpId.ToString() ?? "");
                            SqlParameter mobile_token = new SqlParameter("mobile_token", token_noticiation ?? "");
                            await context.Database.ExecuteSqlCommandAsync("sp_mb_update_mobile_token @emp_id, @mobile_token", emp_id, mobile_token);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Token Noticiation is Error");
                        }
                        jwt.message.status = "1";
                        jwt.message.msg = "Success";
                        jwt.Token_login = token;
                        
                    }
                    else
                    {
                        throw new Exception("The username or password is incorrect");
                    }
                }
                catch (Exception ex)
                {
                    jwt.message.status = !String.IsNullOrEmpty(jwt.message.status) ? jwt.message.status : "2";
                    jwt.message.msg = ex.Message;
                }
            }
            return jwt;
        }

        private string CreateToken(string userId, string permission)
        {
            var tokenString = "";
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddYears(1);

            using (var context = new StandardcanContext())
            {
                var empDetail = context.EmpProfile.SingleOrDefault(a => a.EmpId.ToString() == userId);
                var tokenHandler = new JwtSecurityTokenHandler();
                if (empDetail != null)
                {
                    var claimsIdentity = new ClaimsIdentity(new[]
                    {
                        new Claim("userId", empDetail.EmpId.ToString()),
                        new Claim("userCode", empDetail.EmpCode),
                        new Claim("userName", empDetail.EmpUserName),
                        new Claim("firstName", empDetail.EmpFname),
                        new Claim("lastName", empDetail.EmpLname),
                        new Claim("fullname", empDetail.EmpTitle + empDetail.EmpFname + " " + empDetail.EmpLname),
                        //new Claim("userGroupId", empDetail.EmpGroup.ToString()),
                        //new Claim("userGroupName", context.UserGroup.SingleOrDefault(a =>a.GroupId == empDetail.EmpGroup).GroupName ?? ""),
                        new Claim("userGroup", permission),
                        new Claim("img", ""),
                    });
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token =
                        (JwtSecurityToken)
                        tokenHandler.CreateJwtSecurityToken(
                            issuer: _configuration["JwtIssuer"],
                            audience: _configuration["JwtAudience"],
                            subject: claimsIdentity,
                            notBefore: issuedAt,
                            expires: expires,
                            signingCredentials: creds
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

        public async Task<EmpProfileViewModel> GetProfileAsync(string language)
        {
            var EmpProfile = new EmpProfileViewModel();
            EmpProfile.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = userId,
                        lang = language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/User/GetProfile",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    var empDetail = context.EmpProfile.SingleOrDefault(a => a.EmpId.ToString() == userId);
                    if (empDetail != null)
                    {
                        SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                        SqlParameter lang = new SqlParameter("lang", language ?? "");
                        var spData = context.SpMbEmpProfile.FromSqlRaw("sp_mb_emp_profile @emp_id, @lang", emp_id, lang).ToList();

                        foreach (var item in spData)
                        {
                            EmpProfile.url_img = item.url_img;
                            EmpProfile.name = item.name ?? "-";
                            EmpProfile.lastname = item.lastname ?? "-";
                            EmpProfile.id = item.id;
                            EmpProfile.tel = item.tel ?? "-";
                            EmpProfile.email = item.email ?? "-";
                            EmpProfile.line = item.line ?? "-";
                            EmpProfile.address = item.address ?? "-";
                            EmpProfile.dist = item.dist ?? "-";
                            EmpProfile.prov = item.prov ?? "-";
                            EmpProfile.country = item.country ?? "-";
                            EmpProfile.address_code = item.address_code ?? "-";
                            EmpProfile.outdoor_sts = item.outdoor_sts;
                        }
                        EmpProfile.message.status = "1";
                        EmpProfile.message.msg = "Success";
                    }
                    else
                    {
                        EmpProfile.message.status = "2";
                        EmpProfile.message.msg = "Data not found";
                    }
                }
            }
            catch (Exception ex)
            {
                EmpProfile.message.status = "2";
                EmpProfile.message.msg = ex.Message;
            }

            return EmpProfile;
        }

        public async Task<ReturnMsgViewModel> UpdateUserAsync(EmpProfileDataViewModel user)
        {
            ReturnMsgViewModel data = new ReturnMsgViewModel();
            data.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = userId,
                        tel = user.tel,
                        email = user.email,
                        line = user.line,
                        lang = user.language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/User/UpdateUser",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter tel = new SqlParameter("tel", user.tel ?? "");
                    SqlParameter email = new SqlParameter("email", user.email ?? "");
                    SqlParameter line = new SqlParameter("line", user.line ?? "");
                    SqlParameter lang = new SqlParameter("lang", user.language ?? "");
                    await context.Database.ExecuteSqlCommandAsync("sp_mb_update_profile @emp_id, @tel, @email, @line, @lang", emp_id, tel, email, line, lang);
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<ReturnMsgViewModel> UserChangePasswordAsync(ChangePasswordViewModel user)
        {
            ReturnMsgViewModel data = new ReturnMsgViewModel();
            data.message = new messageModel();
            try
            {
                var dt = DateTime.Now;
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    var userDetail = context.EmpProfile.SingleOrDefault(a => a.EmpId.ToString() == userId);
                    if (userDetail == null)
                    {
                        throw new Exception("Data not found");
                    }
                    var oldPassEncrypt = Cipher.Encrypt(user.old_password, secrectKey);
                    if (oldPassEncrypt != userDetail.EmpPassword)
                    {
                        throw new Exception("Old Password is incorrect");
                    }
                    var newPassEncrypt = Cipher.Encrypt(user.new_password, secrectKey);
                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter newPass = new SqlParameter("new_pass", newPassEncrypt ?? "");
                    await context.Database.ExecuteSqlCommandAsync("sp_mb_update_password @emp_id, @new_pass", emp_id, newPass);
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<ReturnMsgViewModel> UserResetPasswordAsync(ResetPasswordViewModel user)
        {
            ReturnMsgViewModel data = new ReturnMsgViewModel();
            data.message = new messageModel();
            try
            {
                using (var context = new StandardcanContext())
                {
                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        emp_id = user.emp_code,
                        lang = user.language
                    });
                    SystemLog systemLog = new SystemLog()
                    {
                        module = "api/Authentication/UserResetPassword",
                        data_log = jsonData
                    };
                    await _systemLogService.InsertSystemLogAsync(systemLog);

                    var newPassEncrypt = Cipher.Encrypt(user.new_pass, secrectKey);
                    SqlParameter emp_code = new SqlParameter("emp_code", user.emp_code ?? "");
                    SqlParameter new_pass = new SqlParameter("new_pass", newPassEncrypt ?? "");
                    await context.Database.ExecuteSqlCommandAsync("sp_mb_set_password @emp_code, @new_pass", emp_code, new_pass);
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }
            return data;
        }

        public async Task<UserNotiDetail> GetNotiDetailAsync(string noti_id, string language)
        {
            var data = new UserNotiDetail();
            data.message = new messageModel();
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                using (var context = new StandardcanContext())
                {
                    SqlParameter emp_id = new SqlParameter("emp_id", userId ?? "");
                    SqlParameter notiId = new SqlParameter("noti_id", noti_id ?? "");
                    SqlParameter lang = new SqlParameter("lang", language ?? "");
                    var spDataDetail = context.SpMbNotimapDetail.FromSqlRaw("sp_mb_notimap_detail @emp_id, @noti_id, @lang", emp_id, notiId, lang).ToList();
                    var spDataImg = context.SpMbNotimapImage.FromSqlRaw("sp_mb_notimap_image @emp_id, @noti_id, @lang", emp_id, notiId, lang).ToList();
                    data.img = new List<UserNotiDetailImg>();
                    foreach (var item in spDataDetail)
                    {
                        data.lat = item.lat;
                        data.lng = item.lng;
                        data.remark = item.remark;
                    }
                    foreach(var item in spDataImg)
                    {
                        UserNotiDetailImg detailImg = new UserNotiDetailImg();
                        detailImg.url = item.url;
                        data.img.Add(detailImg);
                    }
                    data.message.status = "1";
                    data.message.msg = "Success";
                }
            }
            catch (Exception ex)
            {
                data.message.status = "2";
                data.message.msg = ex.Message;
            }

            return data;
        }

    }
}
