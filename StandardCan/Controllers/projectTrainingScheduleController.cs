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
    public class projectTrainingScheduleController : ApiController
    {
        // GET: api/projectTrainingSchedule
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/projectTrainingSchedule/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/projectTrainingSchedule
        public HttpResponseMessage Post([FromBody] projectTrainingScheduleModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            projectTrainingScheduleService service = new projectTrainingScheduleService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {
                case "master":
                    result = service.master(value);
                    break;

                case "searchTraining":
                    result = service.searchTraining(value);
                    break;

                case "trainingDetail":
                    result = service.trainingDetail(value);
                    break;

                case "searchCourse":
                    result = service.searchCourse(value);
                    break;

                case "searchDestination":
                    result = service.searchDestination(value);
                    break;

                case "searchExpert":
                    result = service.searchExpert(value);
                    break;

                case "searchEmp":
                    result = service.searchEmp(value);
                    break;

                case "insert":
                    result = service.insert(value);
                    break;

                case "update":
                    result = service.update(value);
                    break;

                case "searchFomular":
                    result = service.searchFomulay(value);
                    break;

                default:
                    break;

            }
            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/projectTrainingSchedule/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/projectTrainingSchedule/5
        public void Delete(int id)
        {
        }
    }
}
