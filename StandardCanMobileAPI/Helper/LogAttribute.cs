using Microsoft.AspNetCore.Mvc.Filters;
using StandardCanMobileAPI.Models.ViewModels;
using StandardCanMobileAPI.Services.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Helper
{
    public class LogAttribute : ActionFilterAttribute
    {
        private readonly ISystemLogService _systemLogService;
        private readonly Method _method;
        private readonly string _programName;

        public LogAttribute(
            ISystemLogService systemLogService,
            Method method,
            string programName
            )
        {
            _systemLogService = systemLogService;
            _method = method;
            _programName = programName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = JwtHelper.GetUserIdFromToken(HttpHelper.HttpContext);
            SystemLog systemLog = new SystemLog()
            {
                //IpAddress = HttpInfoUtility.GetCurrentIPAddress(),
                //Browser = HttpInfoUtility.GetCurrentBrowser(),
                //Date = DateTime.Now,
                //Device = HttpInfoUtility.GetCurrentDevice(),
                //Page = HttpInfoUtility.GetCurrentHost() + HttpInfoUtility.GetCurrentPage(),
                //Platform = HttpInfoUtility.GetCurrentPlatform(),
                //UserId = userId,
                //module = Enum.GetName(typeof(Method), _method),
                //ProgramName = _programName
            };
            Task.Run(() => _systemLogService.InsertSystemLogAsync(systemLog));

            base.OnActionExecuting(context);
        }

    }

    public enum Method
    {
        SEARCH = 1,
        INSERT = 2,
        UPDATE = 3,
        DELETE = 4,
        VIEW = 5,
        REPORT = 6,
    }

}
