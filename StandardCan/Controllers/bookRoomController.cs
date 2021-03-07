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
    public class bookRoomController : ApiController
    {
        // GET: api/bookRoom
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/bookRoom/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/bookRoom
        public HttpResponseMessage Post([FromBody] bookRoomModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            bookRoomService service = new bookRoomService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {
                case "master":
                    result = service.master(value);
                    break;

                case "search":
                    result = service.search(value);
                    break;

                case "search_all_calendar":
                    result = service.search_all_calendar(value);
                    break;

                case "search_room_calendar":
                    result = service.search_room_calendar(value);
                    break;

                case "detail":
                    result = service.detail(value);
                    break;
                default:
                    break;

            }


            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/bookRoom/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/bookRoom/5
        public void Delete(int id)
        {
        }
    }
}
