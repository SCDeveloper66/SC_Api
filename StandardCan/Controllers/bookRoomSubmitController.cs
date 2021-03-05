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
    public class bookRoomSubmitController : ApiController
    {
        // GET: api/bookRoomSubmit
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/bookRoomSubmit/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/bookRoomSubmit
        public HttpResponseMessage Post([FromBody] bookRoomDetailModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            bookRoomService service = new bookRoomService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {               

                case "save_draft":
                    result = service.save_draft(value);
                    break;

                case "save_send":
                    result = service.save_draft(value);
                    break;

                case "save_revise":
                    result = service.save_status(value);
                    break;

                case "save_cancel":
                    result = service.save_status(value);
                    break;

                case "save_approve":
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

        // PUT: api/bookRoomSubmit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/bookRoomSubmit/5
        public void Delete(int id)
        {
        }
    }
}
