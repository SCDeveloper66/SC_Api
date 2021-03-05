using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface ITimeInOutService
    {
        Task<TimeInOutViewModel> GetInoutRealtimeAsync(string language);
        Task<InoutEmpRealtimeViewModel> GetInoutEmpRealtimeAsync(string language);
        Task<InoutEmpRealtimeSearchViewModel> GetInoutEmpRealtimeSearchAsync(string language);
        Task<ReturnMsgViewModel> CheckinOutdoorAsync(CheckinOutdoorViewModel dataCheckin);
        Task<ReturnMsgViewModel> CheckInTimeAsync(CheckInTimeViewModel dataCheckin);
        Task<SummaryTimeViewModel> GetSummaryTimeAsync(string language, string type, string year, string month, DateTime? start, DateTime? stop);
        Task<SummaryTimeFilterViewModel> GetSummaryTimeFilterAsync(string language);
    }
}
