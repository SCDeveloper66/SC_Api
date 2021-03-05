using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using StandardCanMobileAPI.Models;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services
{
    public class SystemLogService : ISystemLogService
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;

        public SystemLogService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _accessor = httpContextAccessor;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;


        public async Task InsertSystemLogAsync(SystemLog systemLog)
        {
            try
            {
                using (var context = new StandardcanContext())
                {
                    InterfaceLog interfaceLog = new InterfaceLog();
                    interfaceLog.Module = systemLog.module;
                    interfaceLog.UpdateDate = DateTime.Now;
                    interfaceLog.DataLog = systemLog.data_log;
                    await context.InterfaceLog.AddAsync(interfaceLog);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
