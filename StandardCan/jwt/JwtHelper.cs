using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;

namespace StandardCan.jwt
{
    public class JwtHelper
    {
        

        public static string GetUserIdFromToken(string tokenData)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(tokenData);
            var tokenS = handler.ReadToken(tokenData) as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "userId").Value;
            return jti;
        }

        public static string GetUserGroupFromToken(string tokenData)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(tokenData);
            var tokenS = handler.ReadToken(tokenData) as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "userGroup").Value;
            return jti;
        }

    }
}