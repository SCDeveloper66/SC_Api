using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StandardCan.jwt;
using StandardCan.Models;

namespace StandardCan.Service
{
    public class holidayService
    {
        public IEnumerable<sp_holiday_search_Result> search(holidayModel value)
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
                IEnumerable<sp_holiday_search_Result> result = context.sp_holiday_search(value.year).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<sp_holiday_search_Result> calendarHoliday(holidayModel value)
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
                IEnumerable<sp_holiday_search_Result> result = context.sp_holiday_search(value.year).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public holidayMasterModel master(holidayModel value)
        {
            holidayMasterModel result = new holidayMasterModel();

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

                    string sql = "select		convert(nvarchar(4), year(mhd_date)) code ";
                    sql += " , convert(nvarchar(4), year(mhd_date)) [text] ";
                    sql += " from MAS_HOLIDAY ";
                    sql += " group by    year(mhd_date) ";
                    sql += " order by    year(mhd_date) ";
                    result.year = context.Database.SqlQuery<dropdown>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public IEnumerable<sp_mb_get_holiday_Result> getHoliday(holidayModel value)
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
                IEnumerable<sp_mb_get_holiday_Result> result = context.sp_mb_get_holiday().AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<sp_mb_get_schedule_Result> getSchedule(holidayModel value)
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
                IEnumerable<sp_mb_get_schedule_Result> result = context.sp_mb_get_schedule(value.user_id).AsEnumerable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}