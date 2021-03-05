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
    public class PayrollController : BaseController
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }


        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(PayrollViewModel))]
        public async Task<PayrollViewModel> GetPayroll(string language)
        {
            var data = await _payrollService.GetPayrollAsync(language);
            return data;
        }


    }

}
