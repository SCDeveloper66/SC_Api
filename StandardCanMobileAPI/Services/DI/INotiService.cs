using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface INotiService
    {
        Task<NotiViewModel> GetNotiListAsync(string last_id, string language);
    }
}
