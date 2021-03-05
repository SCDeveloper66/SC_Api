using Spire.Xls;
using StandardCan.jwt;
using StandardCan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StandardCan.Service
{
    public class ImportDataService
    {

        public messageModel importFile(string fileType, string fileId, string user_id, HttpPostedFile postedFile)
        {
            messageModel result = new messageModel();

            try
            {
                if (String.IsNullOrEmpty(user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(postedFile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(postedFile.ContentLength);
                    using (MemoryStream ms2 = new MemoryStream(fileData))
                    {
                        using (var context = new StandardCanEntities())
                        {
                            Workbook workbook = new Workbook();
                            workbook.LoadFromStream(ms2);
                            Worksheet sheet = workbook.Worksheets[0];
                            int rowCount = sheet.Rows.Length;
                            var chkNull = false;
                            var tokenId = Guid.NewGuid().ToString();
                            var dt = DateTime.Now;
                            for (var i = 2; i <= rowCount; i++)
                            {
                                if (chkNull)
                                {
                                    break;
                                }
                                var data = sheet.Range[i, 1].Value;
                                if (!String.IsNullOrEmpty(data))
                                {
                                    IMPORT_DATA _import = new IMPORT_DATA();
                                    _import.Row_Id = Guid.NewGuid().ToString();
                                    _import.Token_Id = tokenId;
                                    _import.Item_No = i - 1;
                                    _import.Id_Card = sheet.Range[i, 1].Value;
                                    _import.Emp_Code = sheet.Range[i, 2].Value;
                                    _import.Emp_Name = sheet.Range[i, 3].Value;
                                    _import.Emp_Dep = sheet.Range[i, 4].Value;
                                    _import.Emp_Position = sheet.Range[i, 5].Value;
                                    _import.Create_Date = dt;
                                    _import.Create_By = Convert.ToInt32(userId);
                                    _import.Update_Date = dt;
                                    _import.Update_By = Convert.ToInt32(userId);
                                    context.IMPORT_DATA.Add(_import);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    chkNull = true;
                                    break;
                                }
                            }
                            context.sp_update_card(tokenId);
                        }
                    }
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
        public messageModel importFileScore(string fileType, string fileId, string user_id, HttpPostedFile postedFile)
        {
            messageModel result = new messageModel();

            try
            {
                if (String.IsNullOrEmpty(user_id))
                {
                    throw new Exception("Unauthorized Access");
                }
                var userId = JwtHelper.GetUserIdFromToken(user_id);
                if (String.IsNullOrEmpty(userId))
                {
                    throw new Exception("Unauthorized Access");
                }

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(postedFile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(postedFile.ContentLength);
                    using (MemoryStream ms2 = new MemoryStream(fileData))
                    {
                        using (var context = new StandardCanEntities())
                        {
                            Workbook workbook = new Workbook();
                            workbook.LoadFromStream(ms2);
                            Worksheet sheet = workbook.Worksheets[0];
                            int rowCount = sheet.Rows.Length;
                            var chkNull = false;
                            var tokenId = Guid.NewGuid().ToString();
                            var dt = DateTime.Now;
                            for (var i = 2; i <= rowCount; i++)
                            {
                                if (chkNull)
                                {
                                    break;
                                }
                                var data = sheet.Range[i, 1].Value;
                                if (!String.IsNullOrEmpty(data))
                                {
                                    string dmy = sheet.Range[i, 2].Value + "/" + sheet.Range[i, 3].Value + "/" + sheet.Range[i, 4].Value;
                                    IMPORT_SCORE _import = new IMPORT_SCORE();
                                    _import.Row_Id = Guid.NewGuid().ToString();
                                    _import.Token_Id = tokenId;
                                    _import.Item_No = i - 1;
                                    _import.Emp_Code = sheet.Range[i, 1].Value;
                                    _import.File_Date = DateTime.ParseExact(dmy, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    _import.File_Id = fileId;
                                    _import.File_Type = fileType;
                                    _import.Emp_Score = string.IsNullOrEmpty(sheet.Range[i, 5].Value) ? 0 : Convert.ToDouble(sheet.Range[i, 5].Value);
                                    _import.Create_Date = dt;
                                    _import.Create_By = Convert.ToInt32(userId);
                                    //_import.Update_Date = dt;
                                    _import.Update_By = Convert.ToInt32(userId);
                                    context.IMPORT_SCORE.Add(_import);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    chkNull = true;
                                    break;
                                }
                            }
                            context.sp_update_score(tokenId,userId);
                        }
                    }
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