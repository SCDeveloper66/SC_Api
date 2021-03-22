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
    public class approvalController : ApiController
    {
        // GET: api/approval
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/approval/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/approval
        public HttpResponseMessage Post([FromBody] employeeModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();



            approvalService service = new approvalService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {
                case "search-leve":
                    result = service.empLeave(value);
                    break;

                case "leave-detail":
                    result = service.empLeaveDetail(value);
                    break;

                case "search-machine":
                    result = service.empMachine(value);
                    break;

                //case "machine-detail":
                //    result = service.empLeaveDetail(value);
                //    break;

                default:
                    break;

            }


            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/approval/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/approval/5
        public void Delete(int id)
        {
        }
    }
}
