using StandardCan.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StandardCan.Controllers
{
    public class timeAttRealtimeReportController : ApiController
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
            var resultFile = service.exportFileJobSCMM(fileName, dataDate);
            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(resultFile.FileContents);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = resultFile.FileDownloadName;
            return result;
        }

    }
}
