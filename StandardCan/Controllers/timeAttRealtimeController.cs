using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using StandardCan.Models;
using StandardCan.Service;

namespace StandardCan.Controllers
{
    public class timeAttRealtimeController : ApiController
    {
        // GET: api/timeAttRealtime
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/timeAttRealtime/5
        public HttpResponseMessage Get(string user_id, string fileName, string dataDate)
        {
            timeAttRealService service = new timeAttRealService();
            var resultFile = service.exportFileJob(fileName, dataDate);
            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(resultFile.FileContents);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = resultFile.FileDownloadName;
            return result;
        }

        // POST: api/timeAttRealtime
        public HttpResponseMessage Post([FromBody] timeAttRealModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            timeAttRealService service = new timeAttRealService();
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
                    result = service.detail(value);
                    break;

                //case "exportFile":
                //    resultFile = service.exportFileWeb(value);
                //    break;

                //case "insert":
                //    result = service.insert(value);
                //    break;

                //case "update":
                //    result = service.update(value);
                //    break;

                //case "delete":
                //    result = service.delete(value);
                //    break;

                default:
                    break;

            }

            if(value.method == "exportFile")
            {
                var resultFile = service.exportFileWeb(value);
                HttpResponseMessage resultExport = null;
                resultExport = Request.CreateResponse(HttpStatusCode.OK);
                resultExport.Content = new ByteArrayContent(resultFile.FileContents);
                resultExport.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                resultExport.Content.Headers.ContentDisposition.FileName = resultFile.FileDownloadName;
                return resultExport;
            }

            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/timeAttRealtime/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/timeAttRealtime/5
        public void Delete(int id)
        {
        }
    }
}
