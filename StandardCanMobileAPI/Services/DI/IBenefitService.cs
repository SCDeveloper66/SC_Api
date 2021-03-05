using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface IBenefitService
    {
        Task<BenefitViewModel> GetOTSummaryAsync(string language, DateTime? start_date, DateTime? stop_date);
        Task<BenefitLoad> GetBenefitsEmployeeAsync(string language);
        Task<BenefitDepartmentViewModel> GetOTDepartmentSummaryAsync(DateTime? start_date, DateTime? stop_date, string emp_id, string sts_id, string language);
        Task<BenefitMasterLoad> GetBenefitsDepartmentMasterAsync(string language);
        Task<BenefitLeave> GetBenefitsLeaveAsync(DateTime? start_date, DateTime? stop_date, string emp_id, string sts_id, string language);
    }
}
