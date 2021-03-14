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
    public class employeeController : ApiController
    {
        // GET: api/employee
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/employee/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/employee
        public HttpResponseMessage Post([FromBody] employeeModel value)
        {
            if (value == null) return null;
            JavaScriptSerializer js = new JavaScriptSerializer();

            

            employeeService service = new employeeService();
            HttpResponseMessage response = null;
            Object result = null;
            switch (value.method)
            {
                case "master":
                    result = service.master(value);
                    break;

                case "upload_file":
                    result = service.uploadFile(value);
                    break;

                case "get_upload_file":
                    result = service.getUploadFile(value);
                    break;

                case "search":
                    result = service.search(value);
                    break;

                case "search_by_id":
                    result = service.search_byid(value);
                    break;

                case "search_by_popup":
                    result = service.search_by_popup(value);
                    break;

                case "tab_profile_search":
                    result = service.tab1_search(value);
                    break;

                case "tab_inout_search":
                    result = service.tab2_search(value);
                    break;

                case "tab_ot_search":
                    result = service.tab_ot_search(value);
                    break;

                case "tab_leave_search":
                    result = service.tab3_search(value);
                    break;

                case "tab_note_search":
                    result = service.tab_note_search(value);
                    break;

                case "tab_note_update":
                    result = service.tab_note_update(value);
                    break;

                case "tab_card_search":
                    result = service.tab_card_search(value);
                    break;

                case "tab_card_insertupdate":
                    result = service.tab_card_update(value);
                    break;

                case "tab_work_search":
                    result = service.tab_work_search(value);
                    break;

                case "tab_work_delete":
                    result = service.tab_work_delete(value);
                    break;

                case "tab_work_insertupdate":
                    result = service.tab_work_update(value);
                    break;

                //BEHAVIOR
                case "tab_behavior_search":
                    result = service.tab_behavior_search(value);
                    break;

                case "tab_behavior_delete":
                    result = service.tab_behavior_delete(value);
                    break;

                case "tab_behavior_insertupdate":
                    result = service.tab_behavior_update(value);
                    break;

                case "tab_behavior_export":
                    result = service.tab_behavior_export(value);
                    break;

                case "update_emp_score":
                    result = service.update_emp_score(value);
                    break;

                case "search-leve":
                    result = service.empLeave(value);
                    break;

                default:
                    break;

            }


            string json = js.Serialize(result);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/employee/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/employee/5
        public void Delete(int id)
        {
        }
    }
}
