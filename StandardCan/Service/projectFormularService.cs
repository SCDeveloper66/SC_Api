using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StandardCan.jwt;
using StandardCan.Models;

namespace StandardCan.Service
{
    public class projectFormularService
    {

        public projectFormularModel detail(projectFormularModel value)
        {
            projectFormularModel result = new projectFormularModel();
            if (value == null || string.IsNullOrEmpty(value.fml_id))
                return result;

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    var h = context.MAS_FORMULA.Where(p => p.MFL_ID.ToString().Equals(value.fml_id)).FirstOrDefault();
                    if (h != null)
                    {
                        result.fml_id = value.fml_id ?? "";
                        result.fml_name = h.MFL_NAME ?? "";
                        result.fml_type = h.MFL_TYPE ?? "";
                        result.fml_input_type = h.MFL_INPUT_TYPE == null ? "1" : h.MFL_INPUT_TYPE.ToString();
                    }

                    result.range = context.sp_formularrange_search(value.fml_id).ToList();
                    result.value = context.sp_formulavalue_search(value.fml_id).ToList();

                }




            }
            catch (Exception ex)
            {
                //result.status = "E";
                //result.message = ex.Message.ToString();
                throw new Exception(ex.Message);
            }

            return result;
        }

        public projectFormulaMasterModel master(projectFormularModel value)
        {
            projectFormulaMasterModel result = new projectFormulaMasterModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    string sql = "select convert(nvarchar(4), MFT_ID) code, MFT_NAME [text] ";
                    sql += " from MAS_FORMULA_TYPE ";
                    sql += " order by    MFT_NAME ";
                    result.type = context.Database.SqlQuery<dropdown>(sql).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }


        public IEnumerable<sp_formular_search_v2_Result> search(projectFormularModel value)
        {
            try
            {
                if (String.IsNullOrEmpty(value.user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }
                StandardCanEntities context = new StandardCanEntities();
                IEnumerable<sp_formular_search_v2_Result> result = context.sp_formular_search_v2(value.fml_type, value.fml_name).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public messageModel save(projectFormularModel value)
        {
            messageModel result = new messageModel();

            try
            {
                if (String.IsNullOrEmpty(value.user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }

                string fml_id = "";
                value.fml_id = value.fml_id == "0" ? "" : value.fml_id;
                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));
                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt_detail = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));
                using (var context = new StandardCanEntities())
                {
                    int ret = context.sp_formular_save(value.fml_id, value.fml_type, value.fml_name, value.fml_input_type, userId, myOutputParamInt);

                    if (myOutputParamInt.Value != null)
                    {
                        int r_id = Convert.ToInt32(myOutputParamInt.Value);
                        fml_id = r_id.ToString();
                    }

                    if (value.range != null)
                    {
                        foreach (var item in value.range)
                        {
                            int ret2 = context.sp_formularrange_insert(fml_id, item.no, item.score, item.display, userId, myOutputParamInt_detail);
                        }
                    }

                    if (value.value != null)
                    {
                        int no = 0;
                        foreach (var item in value.value)
                        {
                            no++;
                            int ret2 = context.sp_formularvalue_insert(fml_id, no.ToString(), item.value, item.text, userId, myOutputParamInt_detail);
                        }
                    }

                }

                result.status = "S";
                result.message = "";
                result.value = fml_id;

            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public messageModel insert(projectFormularModel value)
        {
            messageModel result = new messageModel();

            try
            {

                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("r_id", typeof(Int32));
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    int ret = context.sp_formular_insert(value.fml_name, value.fml_type, value.fml_input_type, userId, myOutputParamInt);
                    int countRange = value.formularRange.Count();
                    for (int i = 0; i < countRange; i++)
                    {
                        int ret2 = context.sp_formularrange_insert(ret.ToString(), value.formularRange[i].fml_range_no, value.formularRange[i].fml_range_score, value.formularRange[i].fml_range_display, userId, myOutputParamInt);
                    }

                    int countvalue = value.formularValue.Count();
                    for (int i = 0; i < countvalue; i++)
                    {
                        int ret2 = context.sp_formularvalue_insert(ret.ToString(), value.formularValue[i].fmlv_orderby, value.formularValue[i].fmlv_value, value.formularValue[i].fmlv_text, userId, myOutputParamInt);
                    }
                }


                if (myOutputParamInt.Value != null)
                {
                    int r_id = Convert.ToInt32(myOutputParamInt.Value);
                    result.status = "S";
                    result.message = "";
                    result.value = r_id.ToString();
                }
                else
                {
                    result.status = "E";
                    result.message = "";
                }


            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        //public messageModel update(formularModel value)
        //{
        //    messageModel result = new messageModel();

        //    try
        //    {
        //        using (var context = new StandardCanEntities())
        //        {
        //            int ret = context.sp_car_update(value.id, value.car_type, value.name, value.detail, value.user_id);
        //        }

        //        result.status = "S";
        //        result.message = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        result.status = "E";
        //        result.message = ex.Message.ToString();
        //    }

        //    return result;
        //}

        public messageModel delete(projectFormularModel value)
        {
            messageModel result = new messageModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }

                    //   int ret = context.sp_form(value.id, value.user_id);
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