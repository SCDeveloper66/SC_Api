using Spire.Xls;
using StandardCan.jwt;
using StandardCan.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;


namespace StandardCan.Controllers
{
    public class ImportDataController : ApiController
    {
        // GET: api/ImportData
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ImportData/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ImportData
        public HttpResponseMessage Post(string fileType, string fileId, string user_id)
        {
            HttpResponseMessage response = null;
            Object result = null;
            JavaScriptSerializer js = new JavaScriptSerializer();
            ImportDataService service = new ImportDataService();

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    if (fileType == "1") // บัตรพนักงาน
                        result = service.importFile(fileType, fileId, user_id, postedFile);
                    else if (fileType == "2") // คะแนนกิจกรรม
                        result = service.importFileScore(fileType, fileId, user_id, postedFile);
                    else if (fileType == "3") // คะแนนแลกพ้อย
                        result = service.importFileScore(fileType, fileId, user_id, postedFile);

                    string json = js.Serialize(result);
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                }
            }
            return response;
        }

        // PUT: api/ImportData/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ImportData/5
        public void Delete(int id)
        {
        }
    }
}
