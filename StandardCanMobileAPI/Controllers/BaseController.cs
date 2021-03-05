using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StandardCanMobileAPI.Helper;

namespace StandardCanMobileAPI.Controllers
{
   
    public class BaseController : Controller
    {
        public BaseController()
        {
            //GlobalContext.Properties["page"] = HttpInfoUtility.GetCurrentPage();
            //GlobalContext.Properties["method"] = HttpInfoUtility.GetCurrentMethod();
            //GlobalContext.Properties["host"] = HttpInfoUtility.GetCurrentHost();
            //GlobalContext.Properties["ipaddress"] = HttpInfoUtility.GetCurrentIPAddress();
            //GlobalContext.Properties["browser"] = HttpInfoUtility.GetCurrentBrowser();
            //GlobalContext.Properties["platform"] = HttpInfoUtility.GetCurrentPlatform();
            //GlobalContext.Properties["appname"] = JwtHelper.GetSubFromToken(HttpHelper.HttpContext);
            //GlobalContext.Properties["device"] = HttpInfoUtility.GetCurrentDevice();
            //GlobalContext.Properties["userid"] = JwtHelper.GetUserIdFromToken(HttpHelper.HttpContext);

            ////logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly, "AdoNetAppender");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            base.OnActionExecuting(context);
        }
    }
}
