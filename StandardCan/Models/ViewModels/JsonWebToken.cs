using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Models.ViewModels
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}