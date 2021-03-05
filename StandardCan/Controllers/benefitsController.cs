using StandardCan.Models;
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
    public class benefitsController : ApiController
    {
        // GET: api/benefits
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/benefits/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/benefits
        public HttpResponseMessage Post([FromBody] benefitsModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            benefitsService service = new benefitsService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {
                case "search":
                    result = service.search(value);
                    break;

                case "insert":
                    result = service.insert(value);
                    break;

                case "update":
                    result = service.update(value);
                    break;

                case "delete":
                    result = service.delete(value);
                    break;

                default:
                    break;

            }

            string json = js.Serialize(result);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/benefits/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/benefits/5
        public void Delete(int id)
        {
        }
    }
}
