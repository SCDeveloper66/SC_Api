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
    public class hardwareController : ApiController
    {
        // GET: api/hardware
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/hardware/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/hardware
        public HttpResponseMessage Post([FromBody]hardwareModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            hardwareService service = new hardwareService();
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

        // PUT: api/hardware/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/hardware/5
        public void Delete(int id)
        {
        }
    }
}
