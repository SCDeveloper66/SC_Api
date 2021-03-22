using StandardCan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardCan.Service
{
    public class approvalService
    {
        public List<empLeave> empLeave(employeeModel value)
        {
            List<empLeave> empLeaves = new List<empLeave>();
            try
            {
                empLeaves.Add(new empLeave
                {
                    id = "1",
                    no = "1",
                    emp_code = "e001",
                    emp_name = "fName001 lName001",
                    leave_start = "2021-03-12",
                    leave_stop = "2021-03-12",
                    remark = "ลาพักร้อน",
                    sts_color = "#ffa87d",
                    sts_text = "Submit"
                });
                empLeaves.Add(new empLeave
                {
                    id = "2",
                    no = "2",
                    emp_code = "e002",
                    emp_name = "fName002 lName002",
                    leave_start = "2021-03-13",
                    leave_stop = "2021-03-15",
                    remark = "ลาพักร้อน",
                    sts_color = "#FFE633",
                    sts_text = "Approve"
                });
                empLeaves.Add(new empLeave
                {
                    id = "3",
                    no = "3",
                    emp_code = "e003",
                    emp_name = "fName003 lName003",
                    leave_start = "2021-03-15",
                    leave_stop = "2021-03-21",
                    remark = "ลาพักร้อน",
                    sts_color = "#ffa87d",
                    sts_text = "Submit"
                });
                empLeaves.Add(new empLeave
                {
                    id = "4",
                    no = "4",
                    emp_code = "e004",
                    emp_name = "fName004 lName004",
                    leave_start = "2021-03-15",
                    leave_stop = "2021-03-21",
                    remark = "ลาพักร้อน",
                    sts_color = "#ffa87d",
                    sts_text = "Approval1"
                });
                empLeaves.Add(new empLeave
                {
                    id = "5",
                    no = "5",
                    emp_code = "e005",
                    emp_name = "fName005 lName005",
                    leave_start = "2021-03-15",
                    leave_stop = "2021-03-21",
                    remark = "ลาพักร้อน",
                    sts_color = "#FFE633",
                    sts_text = "Approval2"
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return empLeaves;
        }

        public empLeave empLeaveDetail(employeeModel value)
        {
            empLeave empLeaves = new empLeave();
            try
            {
                if (value.id == "1")
                {
                    empLeaves.id = "1";
                    empLeaves.emp_code = "e001";
                    empLeaves.emp_name = "fName001 lName001";
                    empLeaves.leave_start = "12/03/2021";
                    empLeaves.leave_stop = "12/03/2021";
                    empLeaves.remark = "ลาพักร้อน";
                    empLeaves.typeLeave = "Annual leave";
                    empLeaves.sts_text = "Submit";
                    empLeaves.leave_over = "N";
                }
                else if (value.id == "2")
                {
                    empLeaves.id = "2";
                    empLeaves.emp_code = "e002";
                    empLeaves.emp_name = "fName002 lName002";
                    empLeaves.leave_start = "13/03/2021";
                    empLeaves.leave_stop = "15/03/2021";
                    empLeaves.remark = "ลาพักร้อน";
                    empLeaves.typeLeave = "Annual leave";
                    empLeaves.sts_text = "Approve";
                    empLeaves.leave_over = "N";
                }
                else if (value.id == "3")
                {
                    empLeaves.id = "3";
                    empLeaves.emp_code = "e003";
                    empLeaves.emp_name = "fName003 lName003";
                    empLeaves.leave_start = "15/03/2021";
                    empLeaves.leave_stop = "21/03/2021";
                    empLeaves.remark = "ลาพักร้อน";
                    empLeaves.typeLeave = "Annual leave";
                    empLeaves.sts_text = "Submit";
                    empLeaves.leave_over = "Y";
                }
                else if (value.id == "4")
                {
                    empLeaves.id = "4";
                    empLeaves.emp_code = "e004";
                    empLeaves.emp_name = "fName004 lName004";
                    empLeaves.leave_start = "15/03/2021";
                    empLeaves.leave_stop = "21/03/2021";
                    empLeaves.remark = "ลาพักร้อน";
                    empLeaves.typeLeave = "Annual leave";
                    empLeaves.sts_text = "Approval1";
                    empLeaves.leave_over = "Y";
                }
                else if (value.id == "5")
                {
                    empLeaves.id = "5";
                    empLeaves.emp_code = "e005";
                    empLeaves.emp_name = "fName005 lName005";
                    empLeaves.leave_start = "15/03/2021";
                    empLeaves.leave_stop = "21/03/2021";
                    empLeaves.remark = "ลาพักร้อน";
                    empLeaves.typeLeave = "Annual leave";
                    empLeaves.sts_text = "Approval2";
                    empLeaves.leave_over = "Y";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return empLeaves;
        }
    }
}