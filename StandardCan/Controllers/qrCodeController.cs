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
    public class qrCodeController : ApiController
    {
        // GET: api/qrCode
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/qrCode/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/qrCode
        public HttpResponseMessage Post([FromBody] qrCodeViewModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            timeAttRealService service = new timeAttRealService();
            HttpResponseMessage response = null;
            Object result = null;

            if (value.method == "QRCode")
            {
                var resultFile = service.exportFileQRCode(value);
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

        // PUT: api/qrCode/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/qrCode/5
        public void Delete(int id)
        {
        }


    }
}
