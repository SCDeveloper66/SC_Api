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
    public class bookCarSubmitController : ApiController
    {
        // GET: api/bookCarSubmit
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/bookCarSubmit/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/bookCarSubmit
        public HttpResponseMessage Post([FromBody] bookCarDetailModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            bookCarService service = new bookCarService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {

                case "save_draft":
                    result = service.save_draft(value);
                    break;

                case "save_update":
                    result = service.save_draft(value);
                    break;

                case "save_send":
                    result = service.save_draft(value);
                    break;

                case "save_cancel":
                    result = service.save_status(value);
                    break;

                case "save_approve":
                    result = service.save_status(value);
                    break;
                case "save_approve_admin":
                    result = service.save_status(value);
                    break;
                case "save_approve_sh":
                    result = service.save_status(value);
                    break;

                case "save_reject":
                    result = service.save_status(value);
                    break;

                default:
                    break;

            }

            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/bookCarSubmit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/bookCarSubmit/5
        public void Delete(int id)
        {
        }
    }
}
