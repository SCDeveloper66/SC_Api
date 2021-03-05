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
    public class empCheckInRegularController : ApiController
    {
        // GET: api/empCheckInRegular
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/empCheckInRegular/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/empCheckInRegular
        public HttpResponseMessage Post([FromBody] empCheckInModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            empCheckInService service = new empCheckInService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {                 

                case "search":
                    result = service.search_Regular(value);
                    break;


                case "save":
                    result = service.save_Regular(value);
                    break;



                default:
                    break;

            }


            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/empCheckInRegular/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/empCheckInRegular/5
        public void Delete(int id)
        {
        }
    }
}
