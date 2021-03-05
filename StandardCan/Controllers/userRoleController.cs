using StandardCan.Models.ViewModels;
using StandardCan.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace StandardCan.Controllers
{
    public class userRoleController : ApiController
    {
        // GET: api/userRole
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/userRole/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/userRole
        public HttpResponseMessage Post(RoleViewModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            SettingService service = new SettingService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {

                case "save_role":
                    result = service.saveUserRole(value);
                    break;

                case "get_usergroup":
                    result = service.getDDLData();
                    break;

                //case "search":
                //    result = service.searchUserGroup();
                //    break;

                default:
                    break;

            }
            string json = js.Serialize(result);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/userRole/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/userRole/5
        public void Delete(int id)
        {
        }
    }
}
