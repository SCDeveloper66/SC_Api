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
    public class dashBoardController : ApiController
    {
        // GET: api/dashBoard
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/dashBoard/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/dashBoard
        public HttpResponseMessage Post([FromBody] dashBoardModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            dashBoardService service = new dashBoardService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {
                case "search":
                    result = service.search(value);
                    break;

                case "searchCalendar":
                    result = service.searchCalendar(value);
                    break;

                case "searchCalendar_v2":
                    result = service.searchCalendar_v2(value);
                    break;

                case "searchCalendarStore":
                    result = service.searchCalendar(value);
                    break;

                case "searchWorkList":
                    result = service.searchWorkList(value);
                    break;

                case "workListApproveSubmit":
                    result = service.workListApproveSubmit(value);
                    break;

                case "workListCancelSubmit":
                    result = service.workListCancelsSubmit(value);
                    break;

                default:
                    break;

            }

            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/dashBoard/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/dashBoard/5
        public void Delete(int id)
        {
        }
    }
}
