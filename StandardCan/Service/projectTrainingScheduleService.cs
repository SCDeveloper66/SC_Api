using StandardCan.jwt;
using StandardCan.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace StandardCan.Service
{
    public class projectTrainingScheduleService
    {
        public projectTrainingMasterModel master(projectTrainingScheduleModel value)
        {
            projectTrainingMasterModel result = new projectTrainingMasterModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    //if (String.IsNullOrEmpty(value.user_id))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    //if (String.IsNullOrEmpty(userId))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "Training_master",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    string sql = "select convert(nvarchar(4), MPJ_YEAR) code, convert(nvarchar(4), MPJ_YEAR) [text] ";
                    sql += " from MAS_PROJECT ";
                    sql += " where MPJ_YEAR is not null ";
                    sql += " group by    MPJ_YEAR ";
                    result.project_year = context.Database.SqlQuery<dropdown>(sql).ToList();

                    sql = "select convert(nvarchar(5), MPJ_ID) code ";
                    sql += " , MPJ_NAME [text] ";
                    sql += " from MAS_PROJECT ";
                    sql += " where MPJ_STATUS = 1 ";
                    sql += " order by MPJ_NAME ";
                    result.project_name = context.Database.SqlQuery<dropdown>(sql).ToList();

                    var expenseData = context.MAS_EXPENSE.ToList();
                    foreach (var item in expenseData)
                    {
                        projectTrainingExpenseModel expenseModel = new projectTrainingExpenseModel();
                        expenseModel.id = item.MEXP_ID.ToString();
                        expenseModel.expense_name = item.MEXP_NAME;
                        result.project_expense.Add(expenseModel);
                    }
                    result.depart = context.sp_depart_search().ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public IEnumerable<sp_trainning_search_Result> searchTraining(projectTrainingScheduleModel value)
        {
            try
            {
                //if (String.IsNullOrEmpty(value.user_id))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                //if (String.IsNullOrEmpty(userId))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                StandardCanEntities context = new StandardCanEntities();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);
                context.interface_log.Add(new interface_log
                {
                    data_log = json,
                    module = "searchTraining",
                    update_date = DateTime.Now
                });
                context.SaveChanges();

                IEnumerable<sp_trainning_search_Result> result = context.sp_trainning_search(value.project_year, value.project_id, value.date_form, value.date_to, value.training_name, "", value.status_id).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public projectTrainingDetailResult trainingDetail(projectTrainingScheduleModel value)
        {
            projectTrainingDetailResult result = new projectTrainingDetailResult();
            result.message = new messageModel();
            try
            {
                using (StandardCanEntities context = new StandardCanEntities())
                {
                    //if (String.IsNullOrEmpty(value.user_id))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    //if (String.IsNullOrEmpty(userId))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "trainingDetail",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    result.data = new projectTrainingDetailDataResult();
                    result.data.training_item = new List<projectTrainingItemDetailDataResult>();

                    var _training = context.TRAININGs.SingleOrDefault(a => a.TrainingId.ToString() == value.training_id);
                    //var empList = context.EMP_PROFILE.ToList();
                    //var departList = context.MAS_DEPARTMENT.ToList();
                    //List<EMP_PROFILE> empList = null;

                    if (_training != null)
                    {
                        result.data.training_id = _training.TrainingId.ToString();
                        result.data.training_name = _training.TrainingName;
                        //result.data.date_form = _training.TrainingDateFrom != null ? _training.TrainingDateFrom.Value.ToString("dd/MM/yyyy") : "";
                        //result.data.date_to = _training.TrainingDateTo != null ? _training.TrainingDateTo.Value.ToString("dd/MM/yyyy") : "";
                        result.data.date_form = _training.TrainingDateFrom != null ? _training.TrainingDateFrom.Value.ToString("yyyy-MM-dd", new System.Globalization.CultureInfo("en-US")) : "";
                        result.data.date_to = _training.TrainingDateTo != null ? _training.TrainingDateTo.Value.ToString("yyyy-MM-dd", new System.Globalization.CultureInfo("en-US")) : "";
                        result.data.training_status = _training.TrainingStatus.Value.ToString();

                        var _trainingItem = context.TRAINING_ITEM.Where(a => a.TrainingId.ToString() == value.training_id).ToList();
                        foreach (var item in _trainingItem)
                        {
                            projectTrainingItemDetailDataResult projectTraining = new projectTrainingItemDetailDataResult();
                            projectTraining.training_item_id = item.TrainingItemId.ToString();
                            projectTraining.course_id = item.MAS_COURSE.MCS_ID.ToString();
                            projectTraining.course_name = item.MAS_COURSE.MCS_NAME;
                            projectTraining.course_type = item.MAS_COURSE.MCS_TYPE;
                            projectTraining.destination_id = item.MAS_DESTINATION.MDT_ID.ToString();
                            projectTraining.destination_name = item.MAS_DESTINATION.MDT_NAME;
                            projectTraining.item_date = item.TrainingItemDate != null ? item.TrainingItemDate.Value.ToString("yyyy-MM-dd", new System.Globalization.CultureInfo("en-US")) : "";
                            projectTraining.time_start = item.TrainingItemStart;
                            projectTraining.time_stop = item.TrainingItemStop;

                            projectTraining.expert_list = new List<projectTrainingItemDetailExpertDataResult>();
                            foreach (var expert in item.TRAINING_EXPERT)
                            {
                                projectTrainingItemDetailExpertDataResult detailExpertDataResult = new projectTrainingItemDetailExpertDataResult();
                                detailExpertDataResult.expert_id = expert.MAS_EXPERT.MEP_ID.ToString();
                                detailExpertDataResult.expert_name = expert.MAS_EXPERT.MEP_NAME;
                                projectTraining.expert_list.Add(detailExpertDataResult);
                            }

                            projectTraining.expense_list = new List<projectTrainingItemDetailExpenseDataResult>();
                            double total = 0.00;
                            foreach (var expense in item.TRAINING_EXPENSE)
                            {
                                projectTrainingItemDetailExpenseDataResult itemDetailExpenseDataResult = new projectTrainingItemDetailExpenseDataResult();
                                total += expense.TrainingExpensePrice != null ? expense.TrainingExpensePrice.Value : 0;
                                itemDetailExpenseDataResult.expense_id = expense.ExpenseId.ToString();
                                itemDetailExpenseDataResult.expense_name = expense.MAS_EXPENSE.MEXP_NAME;
                                itemDetailExpenseDataResult.expense_price = expense.TrainingExpensePrice.ToString();
                                projectTraining.expense_list.Add(itemDetailExpenseDataResult);
                            }
                            projectTraining.expense_total = total.ToString();

                            projectTraining.emp_list = new List<projectTrainingItemDetailEmpDataResult>();

                            string sql = "";
                            sql += "select		a.TrainingEmpCode emp_code, c.md_name emp_depart, b.emp_code + ' ' + isnull(b.emp_title,'') + b.emp_fname + ' ' + isnull(b.emp_lname,'') emp_name ";
                            sql += "            , isnull(b.emp_tel, '') emp_tel, isnull(b.emp_email, '') emp_email ";
                            sql += "            , TrainingEmpScore1 emp_score1, TrainingEmpScore2 emp_score2, TrainingEmpScore3 emp_score3, TrainingEmpScore4 emp_score4 ";
                            sql += "            , TrainingEmpScore5 emp_score5, TrainingEmpScoreTotal emp_score_total, TrainingEmpResult emp_result ";
                            sql += "from        TRAINING_EMP a left join EMP_PROFILE b on a.TrainingEmpCode = b.emp_code ";
                            sql += "            left join MAS_DEPARTMENT c on b.emp_depart_2 = c.MD_ID ";
                            sql += "where       a.TrainingId = " + value.training_id;
                            sql += "            and a.TrainingItemId = " + item.TrainingItemId.ToString();
                            sql += "order by    a.TrainingEmpId ";
                            projectTraining.emp_list = context.Database.SqlQuery<projectTrainingItemDetailEmpDataResult>(sql).ToList();

                            //if (empList != null)
                            //{
                            //    foreach (var emp in item.TRAINING_EMP)
                            //    {
                            //        var _emp = empList.SingleOrDefault(a => a.emp_code == emp.TrainingEmpCode);
                            //        var _depart = departList.SingleOrDefault(a => a.MD_ID.ToString() == _emp.emp_depart_2);
                            //        projectTrainingItemDetailEmpDataResult itemDetailEmpDataResult = new projectTrainingItemDetailEmpDataResult();
                            //        itemDetailEmpDataResult.emp_code = emp.TrainingEmpCode;
                            //        itemDetailEmpDataResult.emp_depart = _depart.md_name;
                            //        itemDetailEmpDataResult.emp_name = (_emp.emp_title ?? "") + _emp.emp_fname + " " + _emp.emp_lname;
                            //        itemDetailEmpDataResult.emp_tel = _emp.emp_tel;
                            //        itemDetailEmpDataResult.emp_email = _emp.emp_email;
                            //        itemDetailEmpDataResult.emp_score1 = emp.TrainingEmpScore1;
                            //        itemDetailEmpDataResult.emp_score2 = emp.TrainingEmpScore2;
                            //        itemDetailEmpDataResult.emp_score3 = emp.TrainingEmpScore3;
                            //        itemDetailEmpDataResult.emp_score4 = emp.TrainingEmpScore4;
                            //        itemDetailEmpDataResult.emp_score5 = emp.TrainingEmpScore5;
                            //        itemDetailEmpDataResult.emp_score_total = emp.TrainingEmpScoreTotal;
                            //        itemDetailEmpDataResult.emp_result = emp.TrainingEmpResult;
                            //        projectTraining.emp_list.Add(itemDetailEmpDataResult);
                            //    }
                            //}


                            result.data.training_item.Add(projectTraining);
                        }
                    }


                    result.message.status = "S";
                    result.message.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.message.status = "E";
                result.message.message = ex.Message.ToString();
            }
            return result;
        }

        public projectFomularResult searchFomulay(projectTrainingScheduleModel value)
        {
            projectFomularResult result = new projectFomularResult();
            result.message = new messageModel();
            try
            {
                using (StandardCanEntities context = new StandardCanEntities())
                {
                    //if (String.IsNullOrEmpty(value.user_id))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    //if (String.IsNullOrEmpty(userId))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "searchFomulay",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    string sql = "";
                    sql += "select		convert(int, b.MFV_ORDER_BY) order_by, MFV_VALUE value, MFV_TEXT text ";
                    sql += " from MAS_COURSE a inner ";
                    sql += " join MAS_FORMULA_VALUE b on a.MFL_ID = b.MFL_ID ";
                    sql += " where a.MCS_ID = "+ value.course_id +" and MFV_STATUS = 1 ";
                    sql += " order by    b.MFV_ORDER_BY ";
                    result.data = context.Database.SqlQuery<fomularField>(sql).ToList();



                    result.message.status = "S";
                    result.message.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.message.status = "E";
                result.message.message = ex.Message.ToString();
            }
            return result;
        }

        public IEnumerable<sp_project_course_search_v2_Result> searchCourse(projectTrainingScheduleModel value)
        {
            try
            {
                //if (String.IsNullOrEmpty(value.user_id))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                //if (String.IsNullOrEmpty(userId))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                StandardCanEntities context = new StandardCanEntities();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);
                context.interface_log.Add(new interface_log
                {
                    data_log = json,
                    module = "searchCourse",
                    update_date = DateTime.Now
                });
                context.SaveChanges();

                IEnumerable<sp_project_course_search_v2_Result> result = context.sp_project_course_search_v2("", "", value.searchCourse_project, value.searchCourse_project, value.searchCourse_name, "1").AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public IEnumerable<sp_destination_search_Result> searchDestination(projectTrainingScheduleModel value)
        {
            try
            {
                //if (String.IsNullOrEmpty(value.user_id))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                //if (String.IsNullOrEmpty(userId))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                StandardCanEntities context = new StandardCanEntities();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);
                context.interface_log.Add(new interface_log
                {
                    data_log = json,
                    module = "searchDestination",
                    update_date = DateTime.Now
                });
                context.SaveChanges();

                IEnumerable<sp_destination_search_Result> result = context.sp_destination_search(value.searchDestination_name ?? "").AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<sp_expert_search_Result> searchExpert(projectTrainingScheduleModel value)
        {
            try
            {
                //if (String.IsNullOrEmpty(value.user_id))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                //if (String.IsNullOrEmpty(userId))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                StandardCanEntities context = new StandardCanEntities();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);
                context.interface_log.Add(new interface_log
                {
                    data_log = json,
                    module = "searchExpert",
                    update_date = DateTime.Now
                });
                context.SaveChanges();

                IEnumerable<sp_expert_search_Result> result = context.sp_expert_search(value.searchExpert_name ?? "").AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public IEnumerable<sp_emp_search_by_popup_Result> searchEmp(projectTrainingScheduleModel value)
        {
            try
            {
                //if (String.IsNullOrEmpty(value.user_id))
                //{
                //    throw new Exception("Unauthorized Access");
                //}
                //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                //if (String.IsNullOrEmpty(userId))
                //{
                //    throw new Exception("Unauthorized Access");
                //}

                StandardCanEntities context = new StandardCanEntities();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(value);
                context.interface_log.Add(new interface_log
                {
                    data_log = json,
                    module = "searchEmp",
                    update_date = DateTime.Now
                });
                context.SaveChanges();

                IEnumerable<sp_emp_search_by_popup_Result> result = context.sp_emp_search_by_popup(value.searchEmp_code ?? "", value.searchEmp_name ?? "", value.searchEmp_depart ?? "").AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public messageModel insert(projectTrainingScheduleModel value)
        {
            messageModel result = new messageModel();
            using (var context = new StandardCanEntities())
            {
                var dbContextTransaction = context.Database.BeginTransaction();
                try
                {

                    //if (String.IsNullOrEmpty(value.user_id))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    //if (String.IsNullOrEmpty(userId))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "Training_insert",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var dt = DateTime.Now;
                    TRAINING training = new TRAINING();
                    training.TrainingName = value.training_name;
                    training.TrainingDateFrom = String.IsNullOrEmpty(value.date_form) ? (DateTime?)null : DateTime.ParseExact(value.date_form, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    training.TrainingDateTo = String.IsNullOrEmpty(value.date_to) ? (DateTime?)null : DateTime.ParseExact(value.date_to, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    training.TrainingStatus = value.training_status;
                    //training.CreateBy = Convert.ToInt32(userId);
                    training.CreateDate = dt;
                    //training.UpdateBy = Convert.ToInt32(userId);
                    training.UpdateDate = dt;
                    context.TRAININGs.Add(training);
                    context.SaveChanges();
                    result.value = training.TrainingId.ToString();
                    foreach (var item in value.training_item)
                    {
                        TRAINING_ITEM training_item = new TRAINING_ITEM();
                        training_item.TrainingId = training.TrainingId;
                        training_item.TrainingCourseId = !String.IsNullOrEmpty(item.course_id) ? Convert.ToInt32(item.course_id) : (int?)null;
                        training_item.TrainingDestinationId = !String.IsNullOrEmpty(item.destination_id) ? Convert.ToInt32(item.destination_id) : (int?)null;
                        training_item.TrainingItemDate = String.IsNullOrEmpty(item.item_date) ? (DateTime?)null : DateTime.ParseExact(item.item_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        training_item.TrainingItemStart = item.time_start ?? "";
                        training_item.TrainingItemStop = item.time_stop ?? "";
                        context.TRAINING_ITEM.Add(training_item);
                        context.SaveChanges();
                        if (item.expert_list != null)
                        {
                            foreach (var expert in item.expert_list)
                            {
                                if (!String.IsNullOrEmpty(expert.expert_id))
                                {
                                    TRAINING_EXPERT training_expert = new TRAINING_EXPERT();
                                    training_expert.TrainingId = training.TrainingId;
                                    training_expert.TrainingItemId = training_item.TrainingItemId;
                                    training_expert.ExpertId = Convert.ToInt32(expert.expert_id);
                                    context.TRAINING_EXPERT.Add(training_expert);
                                    context.SaveChanges();
                                }
                            }
                        }
                        if (item.expense_list != null)
                        {
                            foreach (var expense in item.expense_list)
                            {
                                if (!String.IsNullOrEmpty(expense.expense_id))
                                {
                                    TRAINING_EXPENSE training_expense = new TRAINING_EXPENSE();
                                    training_expense.TrainingId = training.TrainingId;
                                    training_expense.TrainingItemId = training_item.TrainingItemId;
                                    training_expense.ExpenseId = Convert.ToInt32(expense.expense_id);
                                    training_expense.TrainingExpensePrice = String.IsNullOrEmpty(expense.expense_price) ? 0 : Convert.ToDouble(expense.expense_price);
                                    context.TRAINING_EXPENSE.Add(training_expense);
                                    context.SaveChanges();
                                }
                            }
                        }
                        if (item.emp_list != null)
                        {
                            foreach (var emp in item.emp_list)
                            {
                                if (!String.IsNullOrEmpty(emp.emp_code))
                                {
                                    TRAINING_EMP training_emp = new TRAINING_EMP();
                                    training_emp.TrainingId = training.TrainingId;
                                    training_emp.TrainingItemId = training_item.TrainingItemId;
                                    training_emp.TrainingEmpCode = emp.emp_code;
                                    training_emp.TrainingEmpScore1 = emp.emp_score1;
                                    training_emp.TrainingEmpScore2 = emp.emp_score2;
                                    training_emp.TrainingEmpScore3 = emp.emp_score3;
                                    training_emp.TrainingEmpScore4 = emp.emp_score4;
                                    training_emp.TrainingEmpScore5 = emp.emp_score5;
                                    training_emp.TrainingEmpScoreTotal = emp.emp_score_total;
                                    training_emp.TrainingEmpResult = emp.emp_result;
                                    context.TRAINING_EMP.Add(training_emp);
                                    context.SaveChanges();
                                }
                            }
                        }
                    }

                    dbContextTransaction.Commit();
                    result.status = "S";
                    result.message = "Success";
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    result.status = "E";
                    result.message = ex.Message.ToString();
                }
            }

            return result;
        }

        public messageModel update(projectTrainingScheduleModel value)
        {
            messageModel result = new messageModel();
            using (var context = new StandardCanEntities())
            {
                var dbContextTransaction = context.Database.BeginTransaction();
                try
                {

                    //if (String.IsNullOrEmpty(value.user_id))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    //var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    //if (String.IsNullOrEmpty(userId))
                    //{
                    //    throw new Exception("Unauthorized Access");
                    //}
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(value);
                    context.interface_log.Add(new interface_log
                    {
                        data_log = json,
                        module = "Training_update",
                        update_date = DateTime.Now
                    });
                    context.SaveChanges();

                    var dt = DateTime.Now;
                    var training = context.TRAININGs.SingleOrDefault(a => a.TrainingId.ToString() == value.training_id);

                    if (training != null)
                    {
                        result.value = training.TrainingId.ToString();
                        training.TrainingName = value.training_name;
                        //training.TrainingDateFrom = String.IsNullOrEmpty(value.date_form) ? (DateTime?)null : DateTime.ParseExact(value.date_form, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //training.TrainingDateTo = String.IsNullOrEmpty(value.date_to) ? (DateTime?)null : DateTime.ParseExact(value.date_to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        training.TrainingDateFrom = String.IsNullOrEmpty(value.date_form) ? (DateTime?)null : DateTime.ParseExact(value.date_form, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        training.TrainingDateTo = String.IsNullOrEmpty(value.date_to) ? (DateTime?)null : DateTime.ParseExact(value.date_to, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        training.TrainingStatus = value.training_status;
                        //training.CreateBy = Convert.ToInt32(userId);
                        training.CreateDate = dt;
                        //training.UpdateBy = Convert.ToInt32(userId);
                        training.UpdateDate = dt;
                        context.SaveChanges();

                        var _emp = context.TRAINING_EMP.Where(a => a.TrainingId == training.TrainingId).ToList();
                        context.TRAINING_EMP.RemoveRange(_emp);
                        context.SaveChanges();

                        var _expense = context.TRAINING_EXPENSE.Where(a => a.TrainingId == training.TrainingId).ToList();
                        context.TRAINING_EXPENSE.RemoveRange(_expense);
                        context.SaveChanges();

                        var _expert = context.TRAINING_EXPERT.Where(a => a.TrainingId == training.TrainingId).ToList();
                        context.TRAINING_EXPERT.RemoveRange(_expert);
                        context.SaveChanges();

                        var _training_item = context.TRAINING_ITEM.Where(a => a.TrainingId == training.TrainingId).ToList();
                        context.TRAINING_ITEM.RemoveRange(_training_item);
                        context.SaveChanges();

                        foreach (var item in value.training_item)
                        {
                            TRAINING_ITEM training_item = new TRAINING_ITEM();
                            training_item.TrainingId = training.TrainingId;
                            training_item.TrainingCourseId = !String.IsNullOrEmpty(item.course_id) ? Convert.ToInt32(item.course_id) : (int?)null;
                            training_item.TrainingDestinationId = !String.IsNullOrEmpty(item.destination_id) ? Convert.ToInt32(item.destination_id) : (int?)null;
                            training_item.TrainingItemDate = String.IsNullOrEmpty(item.item_date) ? (DateTime?)null : DateTime.ParseExact(item.item_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            training_item.TrainingItemStart = item.time_start ?? "";
                            training_item.TrainingItemStop = item.time_stop ?? "";
                            context.TRAINING_ITEM.Add(training_item);
                            context.SaveChanges();
                            if (item.expert_list != null)
                            {
                                foreach (var expert in item.expert_list)
                                {
                                    if (!String.IsNullOrEmpty(expert.expert_id))
                                    {
                                        TRAINING_EXPERT training_expert = new TRAINING_EXPERT();
                                        training_expert.TrainingId = training.TrainingId;
                                        training_expert.TrainingItemId = training_item.TrainingItemId;
                                        training_expert.ExpertId = Convert.ToInt32(expert.expert_id);
                                        context.TRAINING_EXPERT.Add(training_expert);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            if (item.expense_list != null)
                            {
                                foreach (var expense in item.expense_list)
                                {
                                    if (!String.IsNullOrEmpty(expense.expense_id))
                                    {
                                        TRAINING_EXPENSE training_expense = new TRAINING_EXPENSE();
                                        training_expense.TrainingId = training.TrainingId;
                                        training_expense.TrainingItemId = training_item.TrainingItemId;
                                        training_expense.ExpenseId = Convert.ToInt32(expense.expense_id);
                                        training_expense.TrainingExpensePrice = String.IsNullOrEmpty(expense.expense_price) ? 0 : Convert.ToDouble(expense.expense_price);
                                        context.TRAINING_EXPENSE.Add(training_expense);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            if (item.emp_list != null)
                            {
                                foreach (var emp in item.emp_list)
                                {
                                    if (!String.IsNullOrEmpty(emp.emp_code))
                                    {
                                        TRAINING_EMP training_emp = new TRAINING_EMP();
                                        training_emp.TrainingId = training.TrainingId;
                                        training_emp.TrainingItemId = training_item.TrainingItemId;
                                        training_emp.TrainingEmpCode = emp.emp_code;
                                        training_emp.TrainingEmpScore1 = emp.emp_score1;
                                        training_emp.TrainingEmpScore2 = emp.emp_score2;
                                        training_emp.TrainingEmpScore3 = emp.emp_score3;
                                        training_emp.TrainingEmpScore4 = emp.emp_score4;
                                        training_emp.TrainingEmpScore5 = emp.emp_score5;
                                        training_emp.TrainingEmpScoreTotal = emp.emp_score_total;
                                        training_emp.TrainingEmpResult = emp.emp_result;
                                        context.TRAINING_EMP.Add(training_emp);
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }

                        dbContextTransaction.Commit();
                        result.status = "S";
                        result.message = "Success";
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        result.status = "E";
                        result.message = "Data not found";
                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    result.status = "E";
                    result.message = ex.Message.ToString();
                }
            }

            return result;
        }

    }
}