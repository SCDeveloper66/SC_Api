using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface ISettingService
    {
        Task<NotiSettingDataViewModel> GetNotiSettingAsync(string language);
        Task<ReturnMsgViewModel> NotiSettingAsync(NotiSettingViewModel setting);

    }
}
