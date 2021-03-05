using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

using StandardCan.Models;
using StandardCan.Service;

namespace StandardCan.Controllers
{
    public class carReasonController : ApiController
    {
        // GET: api/carReason
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/carReason/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/carReason
        public HttpResponseMessage Post([FromBody] carReasonModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            carReasonService service = new carReasonService();
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

        // PUT: api/carReason/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/carReason/5
        public void Delete(int id)
        {
        }
    }
}
