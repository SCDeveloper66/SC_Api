using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;

namespace StandardCanMobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(CompanyViewModel))]
        public async Task<CompanyViewModel> GetBenefitsCompany(string last_id, string language)
        {
            var data = await _companyService.GetBenefitsCompanyAsync(last_id, language);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(AboutCompanyViewModel))]
        public async Task<AboutCompanyViewModel> GetAboutCompany(string language)
        {
            var data = await _companyService.GetAboutCompanyAsync(language);
            return data;
        }
    }
}
