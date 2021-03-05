using StandardCanMobileAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services.DI
{
    public interface ICompanyService
    {
        Task<CompanyViewModel> GetBenefitsCompanyAsync(string lastId, string language);
        Task<AboutCompanyViewModel> GetAboutCompanyAsync(string language);

    }
}
