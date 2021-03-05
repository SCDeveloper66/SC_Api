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
    public class NotiController : BaseController
    {
        private readonly INotiService _notiService;

        public NotiController(INotiService notiService)
        {
            _notiService = notiService;
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json", Type = typeof(NotiViewModel))]
        public async Task<NotiViewModel> GetNotiList(string last_id, string language)
        {
            var data = await _notiService.GetNotiListAsync(last_id, language);
            return data;
        }



    }
}
