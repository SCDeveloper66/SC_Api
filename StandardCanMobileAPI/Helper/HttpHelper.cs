using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Helper
{
    public static class HttpHelper
    {
        //private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _accessor;
        public static void Configure(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            //_configuration = configuration;
            _accessor = httpContextAccessor;
        }

        public static HttpContext HttpContext => _accessor.HttpContext;

        public static string GetFormToken(HttpContext httpContext)
        {
            if (!httpContext.Request.Form.Keys.Contains("Authorization") && !httpContext.Request.Form.Keys.Contains("authorization"))
                return "";

            var token = httpContext.Request.Form["Authorization"];
            if (token == "")
                return httpContext.Request.Form["authorization"];
            else
                return httpContext.Request.Form["Authorization"];
        }
    }
}
