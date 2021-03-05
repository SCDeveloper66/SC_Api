using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;

namespace StandardCanMobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitController : BaseController
    {

        private readonly IBenefitService _benefitService;

        public BenefitController(IBenefitService benefitService)
        {
            _benefitService = benefitService;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(BenefitViewModel))]
        public async Task<BenefitViewModel> GetOTSummary(string language, string start_date, string stop_date)
        {
            DateTime? startDate = null;
            DateTime? stopDate = null;
            if (!String.IsNullOrEmpty(start_date))
            {
                var tempSDateM = start_date.Split('-');
                if (tempSDateM.Length > 3)
                {
                    startDate = DateTime.ParseExact(tempSDateM[0] + "-" + tempSDateM[1] + "-" + tempSDateM[2], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                var tempSDateP = start_date.Split(' ');
                if (tempSDateP.Length > 1)
                {
                    startDate = DateTime.ParseExact(tempSDateP[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
            }
            if (!String.IsNullOrEmpty(stop_date))
            {
                var tempSDateM = stop_date.Split('-');
                if (tempSDateM.Length > 3)
                {
                    stopDate = DateTime.ParseExact(tempSDateM[0] + "-" + tempSDateM[1] + "-" + tempSDateM[2], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                var tempSDateP = stop_date.Split(' ');
                if (tempSDateP.Length > 1)
                {
                    stopDate = DateTime.ParseExact(tempSDateP[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
            }

            var data = await _benefitService.GetOTSummaryAsync(language, startDate, stopDate);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(BenefitLoad))]
        public async Task<BenefitLoad> GetBenefitsEmployee(string language)
        {
            var data = await _benefitService.GetBenefitsEmployeeAsync(language);
            return data;
        }


        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(BenefitDepartmentViewModel))]
        public async Task<BenefitDepartmentViewModel> GetOTDepartmentSummary(string start_date, string stop_date, string emp_id, string sts_id, string language)
        {
            DateTime? startDate = null;
            DateTime? stopDate = null;
            if (!String.IsNullOrEmpty(start_date))
            {
                var tempSDateM = start_date.Split('-');
                if(tempSDateM.Length > 3)
                {
                    startDate = DateTime.ParseExact(tempSDateM[0] + "-" + tempSDateM[1] + "-" + tempSDateM[2], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                var tempSDateP = start_date.Split(' ');
                if(tempSDateP.Length > 1)
                {
                    startDate = DateTime.ParseExact(tempSDateP[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
            }
            if (!String.IsNullOrEmpty(stop_date))
            {
                var tempSDateM = stop_date.Split('-');
                if (tempSDateM.Length > 3)
                {
                    stopDate = DateTime.ParseExact(tempSDateM[0] + "-" + tempSDateM[1] + "-" + tempSDateM[2], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                var tempSDateP = stop_date.Split(' ');
                if (tempSDateP.Length > 1)
                {
                    stopDate = DateTime.ParseExact(tempSDateP[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
            }
            var data = await _benefitService.GetOTDepartmentSummaryAsync(startDate, stopDate, emp_id, sts_id, language);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(BenefitMasterLoad))]
        public async Task<BenefitMasterLoad> GetBenefitsDepartmentMaster(string language)
        {
            var data = await _benefitService.GetBenefitsDepartmentMasterAsync(language);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(BenefitLeave))]
        public async Task<BenefitLeave> GetBenefitsLeave(string start_date, string stop_date, string emp_id, string sts_id, string language)
        {
            DateTime? startDate = null;
            DateTime? stopDate = null;
            if (!String.IsNullOrEmpty(start_date))
            {
                var tempSDateM = start_date.Split('-');
                if (tempSDateM.Length > 3)
                {
                    startDate = DateTime.ParseExact(tempSDateM[0] + "-" + tempSDateM[1] + "-" + tempSDateM[2], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                var tempSDateP = start_date.Split(' ');
                if (tempSDateP.Length > 1)
                {
                    startDate = DateTime.ParseExact(tempSDateP[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
            }
            if (!String.IsNullOrEmpty(stop_date))
            {
                var tempSDateM = stop_date.Split('-');
                if (tempSDateM.Length > 3)
                {
                    stopDate = DateTime.ParseExact(tempSDateM[0] + "-" + tempSDateM[1] + "-" + tempSDateM[2], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                var tempSDateP = stop_date.Split(' ');
                if (tempSDateP.Length > 1)
                {
                    stopDate = DateTime.ParseExact(tempSDateP[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                }
            }
            var data = await _benefitService.GetBenefitsLeaveAsync(startDate, stopDate, emp_id, sts_id, language);
            return data;
        }
    }
}
