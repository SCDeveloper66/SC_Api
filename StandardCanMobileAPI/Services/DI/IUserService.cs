using StandardCanMobileAPI.Models;
using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface IUserService
    {
        Task<JsonWebToken> UserLogin(string username, string password, string token_noticiation);
        Task<EmpProfileViewModel> GetProfileAsync(string language);
        Task<ReturnMsgViewModel> UpdateUserAsync(EmpProfileDataViewModel user);
        Task<ReturnMsgViewModel> UserChangePasswordAsync(ChangePasswordViewModel user);
        Task<ReturnMsgViewModel> UserResetPasswordAsync(ResetPasswordViewModel user);
        Task<UserNotiDetail> GetNotiDetailAsync(string noti_id, string language);
    }
}
