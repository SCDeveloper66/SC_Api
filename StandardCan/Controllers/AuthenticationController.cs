using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using StandardCan.Models;
using StandardCan.Models.ViewModels;
using StandardCan.Service;

namespace StandardCan.Controllers
{
    public class AuthenticationController : ApiController
    {
        // GET: api/Authentication
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Authentication/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Authentication
        public HttpResponseMessage Post([FromBody] UserViewModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            AuthenticationService service = new AuthenticationService();
            HttpResponseMessage response = null;
            Object result = null;

            if (string.IsNullOrEmpty(value.Token))
                result = service.Login(value.UserName, value.Password);
            else
            {
                result = service.LoginToken(value.Token);
            }

            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }


    }
}
