using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StandardCan.Models;

namespace StandardCan.Service
{
    public class scheduleCourseService
    {



        public scheduleCourseModel detail(scheduleCourseModel value)
        {
            scheduleCourseModel result = new scheduleCourseModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    //string sql = "";
                    //sql += " select convert(nvarchar(4), MPJ_YEAR) code, convert(nvarchar(4), MPJ_YEAR) [text] ";
                    //sql += " from MAS_PROJECT ";
                    //sql += " where MPJ_YEAR is not null ";
                    //sql += " group by    MPJ_YEAR ";
                    //result.year = context.Database.SqlQuery<dropdown>(sql).ToList();

                    var mas_expen = context.sp_expense_search("").ToList();
                    List<courseCostDetail> cost_source = new List<courseCostDetail>();
                    if (mas_expen != null)
                    {
                        foreach (var e in mas_expen)
                        {
                            cost_source.Add(new courseCostDetail
                            {
                                id = e.id.ToString(),
                                name = e.name ?? "",
                                cost = ""
                            });
                        }

                    }


                    var h = context.sp_schedule_course_search_v2(value.id, "", "", "", "", "", "", "").FirstOrDefault();
                    if (h != null)
                    {
                        result.id = value.id;
                        result.course_name = h.course_name ?? "";
                        result.start_date = h.start_date ?? "";
                        result.stop_date = h.stop_date ?? "";
                        result.status = h.status ?? "";
                    }

                    var cost_all = context.sp_schedule_course_cost_v2(value.id).ToList();
                    var emp_all = context.sp_schedule_course_emp_v2(value.id).ToList();

                    var detail = context.sp_schedule_course_detail_v2(value.id).ToList();
                    if (detail != null)
                    {
                        foreach (var item in detail)
                        {
                            var cost = cost_all.Where(p => p.ref_id.Equals(item.id)).ToList();
                            foreach (var c in cost_source)
                            {
                                var q = cost.Where(p => p.cost_id.Equals(c.id)).FirstOrDefault();
                                if (q != null)
                                {
                                    c.cost = q.cost ?? "";
                                }
                                else
                                {
                                    c.cost = "";
                                }
                            }

                            var emp = emp_all.Where(p => p.ref_id.Equals(item.id)).ToList();

                            result.data.Add(new courseDetailModel
                            {
                                id = item.id ?? "",
                                course_id = item.course_id,
                                name = item.course_name,
                                type_name = item.course_type ?? "",
                                expert_name = item.MEP_NAME ?? "",
                                cost_total = item.cost_total ?? "",
                                emp_detail = emp,
                                cost_detail = cost_source,
                            });

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public projectMasterModel master(scheduleCourseModel value)
        {
            projectMasterModel result = new projectMasterModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    string sql = "select convert(nvarchar(4), MPJ_YEAR) code, convert(nvarchar(4), MPJ_YEAR) [text] ";
                    sql += " from MAS_PROJECT ";
                    sql += " where MPJ_YEAR is not null ";
                    sql += " group by    MPJ_YEAR ";
                    result.year = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select		convert(nvarchar(5), MPJ_ID) code ";
                    sql += " , MPJ_NAME [text] ";
                    sql += " from MAS_PROJECT ";
                    sql += " where MPJ_STATUS = 1 ";
                    sql += " order by MPJ_NAME ";
                    result.project = context.Database.SqlQuery<dropdown>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public IEnumerable<sp_schedule_course_search_Result> search(scheduleCourseModel value)
        {
            StandardCanEntities context = new StandardCanEntities();
            IEnumerable<sp_schedule_course_search_Result> result = context.sp_schedule_course_search(value.year_from, value.year_to, value.prj_from, value.prj_to, value.course_name, value.start_date, value.stop_date).AsEnumerable();
            return result;
        }

        public messageModel save(scheduleCourseModel value)
        {
            messageModel result = new messageModel();

            try
            {
                value.user_id = "10001";
                int r_id = -1;

                string token_update = Guid.NewGuid().ToString();

                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));
                System.Data.Entity.Core.Objects.ObjectParameter c_myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));

                using (var context = new StandardCanEntities())
                {
                    var total_course = 0;
                    var total_emp = 0;
                    if (value.data != null && value.data.Count() > 0)
                    {
                        total_course = value.data.Count();

                        foreach (var h in value.data)
                        {
                            if (h.emp_detail != null && h.emp_detail.Count > 0)
                            {
                                h.emp_total = value.data[0].emp_detail.Count().ToString();
                                total_emp += value.data[0].emp_detail.Count();

                                decimal total_expen = 0;
                                if (h.cost_detail != null)
                                {
                                    foreach (var e in h.cost_detail)
                                    {
                                        if (!string.IsNullOrEmpty(e.cost))
                                        {
                                            total_expen += Convert.ToDecimal(e.cost);
                                        }
                                    }
                                }
                                h.cost_total = total_expen.ToString();

                            }

                        }


                    }

                    int ret = context.sp_schedule_course_save(value.id, value.course_name, value.start_date, value.stop_date, total_course.ToString(), total_emp.ToString(), value.status, value.user_id, myOutputParamInt);
                    if (myOutputParamInt.Value != null)
                    {
                        r_id = Convert.ToInt32(myOutputParamInt.Value);
                    }

                    foreach (var d in value.data)
                    {
                        int c_id = -1;
                        context.sp_schedule_course_detail_save(r_id.ToString(), d.id, d.course_id, d.date, d.start_time, d.stop_time, d.cost_total, d.emp_total, value.user_id, token_update, c_myOutputParamInt);
                        if (c_myOutputParamInt.Value != null)
                        {
                            c_id = Convert.ToInt32(c_myOutputParamInt.Value);
                        }

                        context.sp_schedule_course_emp_delete(c_id.ToString());
                        foreach (var e in d.emp_detail)
                        {
                            context.sp_schedule_course_emp_save(c_id.ToString(), e.emp_code, e.score_part_1, e.score_part_2, e.score_part_3, e.score_part_4, e.score_part_5, e.score_total, e.score_result, value.user_id);
                        }

                        context.sp_schedule_course_cost_delete(c_id.ToString());
                        foreach(var c in d.cost_detail)
                        {
                            if (string.IsNullOrEmpty(c.cost))
                                continue;

                            context.sp_schedule_course_cost_save(r_id.ToString(), c_id.ToString(), c.id, c.cost, value.user_id);
                        }

                        context.sp_schedule_course_detail_delete(c_id.ToString(), token_update);

                    }



                }






                result.status = "S";
                result.message = "";
                result.value = r_id.ToString();


            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel update(nodeModel value)
        {
            messageModel result = new messageModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    int ret = context.sp_node_update(value.id, value.ip, value.name, value.ref_id, value.user_id);
                }

                result.status = "S";
                result.message = "";
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel delete(nodeModel value)
        {
            messageModel result = new messageModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    int ret = context.sp_node_delete(value.id, value.user_id);
                }

                result.status = "S";
                result.message = "";
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

    }
}