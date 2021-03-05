using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface ITraningService
    {
        Task<TraningViewModel> GetTraningConditionAsync(string language);
        Task<TraningDetailViewModel> GetTraningDetailAsync(string year, string project_id, string lot_id, string language);
    }
}
