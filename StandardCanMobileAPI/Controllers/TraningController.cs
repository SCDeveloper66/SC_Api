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
    public class TraningController : ControllerBase
    {
        private readonly ITraningService _traningService;

        public TraningController(ITraningService traningService)
        {
            _traningService = traningService;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(TraningViewModel))]
        public async Task<TraningViewModel> GetTraningCondition(string language)
        {
            var data = await _traningService.GetTraningConditionAsync(language);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(TraningDetailViewModel))]
        public async Task<TraningDetailViewModel> GetTraningDetail(string year, string project_id, string lot_id, string language)
        {
            var data = await _traningService.GetTraningDetailAsync(year, project_id, lot_id, language);
            return data;
        }
    }
}
