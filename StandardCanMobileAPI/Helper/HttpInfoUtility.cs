using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Helper
{
    public class HttpInfoUtility
    {
        public static string GetCurrentPage()
        {

            if (null != HttpHelper.HttpContext)
            {
                return HttpHelper.HttpContext.Features.Get<IHttpRequestFeature>().RawTarget;
            }
            return "Couldn't get page!";
        }

        public static string GetCurrentMethod()
        {

            if (null != HttpHelper.HttpContext)
            {
                return HttpHelper.HttpContext.Request.Method;
            }
            return "Couldn't get page!";
        }

        public static string GetCurrentHost()
        {
            if (null != Environment.MachineName)
            {
                return Environment.MachineName;
            }
            return "Couldn't get host!";
        }

        public static string GetCurrentIPAddress()
        {
            if (null != HttpHelper.HttpContext)
            {
                return HttpHelper.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return "Couldn't get IP address!";
        }

        public static string GetCurrentBrowser()
        {
            if (null != HttpHelper.HttpContext)
            {
                try
                {
                    return HttpHelper.HttpContext.Request.Browser()?.Type.ToString();
                }
                catch
                {
                    return "Couldn't get browser!";
                }
            }
            return "Couldn't get browser!";
        }

        public static string GetCurrentPlatform()
        {
            if (null != HttpHelper.HttpContext)
            {
                return HttpHelper.HttpContext.Request.Headers["User-Agent"].ToString() != null ? HttpHelper.HttpContext.Request.Headers["User-Agent"].ToString() : "";
            }
            return "Couldn't get platform!";
        }

        public static string GetCurrentDevice()
        {
            if (null != HttpHelper.HttpContext)
            {
                return HttpHelper.HttpContext.Request.Device()?.Type.ToString();
            }
            return "Couldn't get device details!";
        }

        public static string GetReferer()
        {

            if (null != HttpHelper.HttpContext)
            {
                return HttpHelper.HttpContext.Request.Headers["Referer"].ToString();
            }
            return "Couldn't get referer!";
        }

    }
}
