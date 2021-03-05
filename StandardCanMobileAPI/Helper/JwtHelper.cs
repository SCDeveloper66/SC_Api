using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Helper
{
    public class JwtHelper
    {
        public static string GetUserIdFromToken(HttpContext httpContext)
        {
            return httpContext.User.FindFirst("userId")?.Value;
        }


    }
}
