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
    public class projectCourseController : ApiController
    {
        // GET: api/projectCourse
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/projectCourse/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/projectCourse
        public HttpResponseMessage Post([FromBody] projectCourseModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            projectCourseService service = new projectCourseService();
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

                case "detail":
                    result = service.detail2(value);
                    break;

                case "save":
                    result = service.save(value);
                    break;
                     
                default:
                    break;

            }


            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/projectCourse/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/projectCourse/5
        public void Delete(int id)
        {
        }
    }
}
