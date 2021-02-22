
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Controllers.Patient;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
namespace KeystoneProject.Controllers.Patient
{
    public class PatientAppointmentScheduleController : Controller
    {
        int NoOfAppointment = 0;
        //
        // GET: /PatientAppointmentSchedule/
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        List<PatientAppointmentSchedule> PatientAppointmentscheduleList = new List<PatientAppointmentSchedule>();
        BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
        PatientAppointmentSchedule PatientAppointment = new PatientAppointmentSchedule();

        public JsonResult BindPatientPrefix(string prefix)
        {
            return new JsonResult { Data = _PatientAppointment.BindPrefixPatient(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindDoctorAppointment(string prefix)
        
        {
            return new JsonResult { Data = _PatientAppointment.BindAppointmentDoctor(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindDoctorAppointmenttype(string prefix)
        {
            return new JsonResult { Data = _PatientAppointment.BindDoctorAppointmenttype(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindPatientAppointmentID(string PatientID)
        {
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            PatientAppointmentSchedule PatientAppointment = new PatientAppointmentSchedule();
            DataSet ds = _PatientAppointment.GetPatientAppointmentsAppointID(HospitalID, LocationID, Convert.ToInt32(PatientID));
            List<PatientAppointmentSchedule> PatientAppointmentscheduleList = new List<PatientAppointmentSchedule>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientAppointment.PatientID = dr["PatientID"].ToString();
                PatientAppointment.PFPatientName = dr["PFPatientName"].ToString();
                PatientAppointment.PFirstName = dr["PFirstName"].ToString();
                PatientAppointment.PMiddleName = dr["PMiddleName"].ToString();
                PatientAppointment.PLastName = dr["PLastName"].ToString();
                PatientAppointment.MobileNo = dr["MobileNo"].ToString();
                PatientAppointment.Age = dr["Age"].ToString();
                PatientAppointment.DateOFBirth = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd");
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr1 in ds.Tables[1].Rows)
                {
                    PatientAppointment.PatientAppointmentScheduleID = dr1["PatientAppointmentScheduleID"].ToString();
                    PatientAppointment.DoctorAppoinmentScheduleID = dr1["DoctorAppoinmentScheduleID"].ToString();
                    PatientAppointment.DoctorID = dr1["DoctorID"].ToString();
                    PatientAppointment.AppointmentDate = Convert.ToDateTime(dr1["AppointmentDate"]).ToString("yyyy-MM-dd");
                    PatientAppointment.AppointmentTime = dr1["FromTime"].ToString() + " To " + dr1["ToTime"].ToString();
                    PatientAppointment.AppointmentType = dr1["AppointmentType"].ToString();
                    PatientAppointment.DoctorPrintName = dr1["DoctorPrintName"].ToString();
                    PatientAppointment.NoOfAppointment = dr1["NoOfAppointment"].ToString();
                }
            }
            PatientAppointmentscheduleList.Add(PatientAppointment);
            return new JsonResult { Data = PatientAppointmentscheduleList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CalGetDaysAge(string datebirth)
        {
            DateTime dtpDateOfBirth = Convert.ToDateTime(datebirth);
            List<PatientOPD> add = new List<Models.Patient.PatientOPD>();
            PatientOPD obj = new Models.Patient.PatientOPD();
            DateTime dtToday = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);
            int Year = 0;
            int Month = 0;
            int Day = 0;

            dtBeforeDate = dtBeforeDate.AddYears(1);
            while (dtToday >= dtBeforeDate)
            {
                dtBeforeDate = dtBeforeDate.AddYears(1);
                Year++;
            }
            dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);
            dtBeforeDate = dtBeforeDate.AddMonths(1);
            while (dtToday >= dtBeforeDate)
            {
                dtBeforeDate = dtBeforeDate.AddMonths(1);
                Month++;
            }

            dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);

            dtBeforeDate = dtBeforeDate.AddMonths(Month);
            var a = dtBeforeDate;
            //if(a== getTime())
            //{

            //}
            TimeSpan diffResult = dtToday.Date - dtBeforeDate.Date;

            int TotalDay = Convert.ToInt32(diffResult.TotalDays);
            int TotalMonth = Month % 12;

            obj.day = TotalDay.ToString();

            obj.Month = TotalMonth.ToString();

            obj.year = Year.ToString();
            add.Add(obj);
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult BindPatientName(string prefix)
        {
            return new JsonResult { Data = _PatientAppointment.GetPatientName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetDoctorAppointmentSchedule(int DoctorAppoinmentScheduleID, int DoctorID)
        {
            return new JsonResult { Data = _PatientAppointment.GetDoctorAppointmentSchedule(DoctorAppoinmentScheduleID, DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetPatientAppointmentDetailForBindAppointmentType(int DoctorID)
        {
            return new JsonResult { Data = _PatientAppointment.GetPatientAppointmentDetailForBindAppointmentType(DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllPatient(DateTime AppointDateID)
        {
            return new JsonResult { Data = _PatientAppointment.GetAllPatient(AppointDateID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult TotalAppointment()
        {
            return new JsonResult { Data = _PatientAppointment.TotalAppointment(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult RegCount()
        {
            return new JsonResult { Data = _PatientAppointment.RegCount(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllPatientAppointmentDetail()
        {
            return new JsonResult { Data = _PatientAppointment.GetAllPatientAppointmentDetail(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //public JsonResult GetNextOutsidePatientID()
        //{
        //   // return new JsonResult { Data = _PatientAppointment.GetNextOutsidePatientID(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        //public JsonResult GetPatientAppointmentDetailForDoctorDetail(int DoctorID, string AppointmentType, string DayDate, DateTime AppointmentDate)
        //{
        //    return new JsonResult { Data = _PatientAppointment.GetPatientAppointmentDetailForDoctorDetail(DoctorID, AppointmentType, DayDate, AppointmentDate), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        DataSet dsDoctorSchedule = new DataSet();
        public void GetEmptyDatasetPatientSchedule()
        {
            dsDoctorSchedule.Reset();
            dsDoctorSchedule.Tables.Add(new DataTable());
            dsDoctorSchedule.Tables[0].Columns.Add("FromToTime");
            dsDoctorSchedule.Tables[0].Columns.Add("NoOfAppointment");
            dsDoctorSchedule.Tables[0].Columns.Add("FromTime");
            dsDoctorSchedule.Tables[0].Columns.Add("ToTime");
            dsDoctorSchedule.Tables[0].Columns.Add("DoctorAppoinmentScheduleID");
        }
        public JsonResult GetDoctorAppointmentSchedule1(int DoctorAppoinmentScheduleID, int DoctorID)
        {
            return new JsonResult { Data = _PatientAppointment.GetDoctorAppointmentSchedule1(DoctorAppoinmentScheduleID, DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult EditGetPatientAppointmentDetails(int PatientAppointmentScheduleID)
        {
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            ModelState.Clear();
            return new JsonResult { Data = _PatientAppointment.GetPatientAppointmentDetails(PatientAppointmentScheduleID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //  return View(db.GetDepartment(id));
        }
        public ActionResult BindFromToTime(string AppointmentType, string DoctorID, string AppointmentDate)
        {
            string Mode = "";
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            PatientAppointment.PatientAppointmentNO = AppointmentType;
            PatientAppointment.DoctorID = DoctorID;
            DataSet dsDoctorTimedetail = new DataSet();
            if (PatientAppointment.PatientAppointmentNO == "Weekly")
            {
                dsDoctorTimedetail = _PatientAppointment.GetPatientAppointmentDetailForDoctorDetail(HospitalID, LocationID, Convert.ToInt32(PatientAppointment.DoctorID), PatientAppointment.PatientAppointmentNO, Convert.ToString(Convert.ToDateTime(AppointmentDate).DayOfWeek.ToString()), Convert.ToDateTime(AppointmentDate));
            }
            else
                if (PatientAppointment.PatientAppointmentNO == "Monthly")
                {
                    dsDoctorTimedetail = _PatientAppointment.GetPatientAppointmentDetailForDoctorDetail(HospitalID, LocationID, Convert.ToInt32(PatientAppointment.DoctorID), PatientAppointment.PatientAppointmentNO, Convert.ToString(Convert.ToDateTime(AppointmentDate).Day.ToString()), Convert.ToDateTime(AppointmentDate));
                }
                else
                {
                    dsDoctorTimedetail = _PatientAppointment.GetPatientAppointmentDetailForDoctorDetail(HospitalID, LocationID, Convert.ToInt32(PatientAppointment.DoctorID), PatientAppointment.PatientAppointmentNO, "Daily", Convert.ToDateTime(AppointmentDate));
                }
            GetEmptyDatasetPatientSchedule();
            foreach (DataRow drMainDetail in dsDoctorTimedetail.Tables[0].Rows)
            {
                if (dsDoctorTimedetail.Tables[0].Rows.Count > 0)
                {
                    string FromTime;
                    string Totime;
                    DateTime Endtime;
                    int PerAppointment = 0;
                    string Totime1;
                    if (dsDoctorTimedetail.Tables[3].Rows.Count > 0)
                    {
                        FromTime = Convert.ToDateTime(dsDoctorTimedetail.Tables[3].Rows[0]["FromTime"]).ToString("hh:mm tt");
                        Totime = Convert.ToDateTime(dsDoctorTimedetail.Tables[3].Rows[0]["ToTime"]).ToString("hh:mm tt");
                        Totime1 = Convert.ToDateTime(dsDoctorTimedetail.Tables[3].Rows[0]["ToTime"]).ToString("hh:mm tt");
                    }
                    else
                    {
                        FromTime = Convert.ToDateTime(drMainDetail["FromTime"]).ToString("hh:mm tt");
                        Totime = Convert.ToDateTime(drMainDetail["ToTime"]).ToString("hh:mm tt");
                        Totime1 = Convert.ToDateTime(drMainDetail["ToTime"]).ToString("hh:mm tt");
                    }
                    //Endtime = Convert.ToDateTime(drMainDetail["ToTime"].ToString());
                    PerAppointment = Convert.ToInt32(drMainDetail["PerAppointmentTimeinMinute"].ToString());
                    if (PatientAppointment.PatientAppointmentNO != "OnCalls")
                    {
                        for (int i = 1; Convert.ToInt32(drMainDetail["NoOfAppointment"].ToString()) >= i; i++)
                        {
                            DataView dvDetail = new DataView(dsDoctorTimedetail.Tables[1], "DoctorAppoinmentScheduleID = " + drMainDetail["DoctorAppoinmentScheduleID"].ToString() + " and NoOfAppointment = " + i + "", "", DataViewRowState.CurrentRows);
                            Totime = Convert.ToDateTime(FromTime).AddMinutes(+PerAppointment).ToString("hh:mm tt");
                            if (Convert.ToDateTime(Totime) > Convert.ToDateTime(Totime1))
                            {
                                break;
                            }
                            if (Mode.ToString() == "Edit")
                            {
                                if (i == NoOfAppointment)
                                {
                                    dvDetail = new DataView(dsDoctorTimedetail.Tables[1], "DoctorAppoinmentScheduleID = " + drMainDetail["DoctorAppoinmentScheduleID"].ToString() + " and NoOfAppointment = " + 0 + "", "", DataViewRowState.CurrentRows);
                                }
                            }
                            if (dvDetail.Count == 0)
                            {
                                DataRow dr = dsDoctorSchedule.Tables[0].NewRow();
                                dr["FromTime"] = FromTime;
                                dr["ToTime"] = Totime;
                                dr["FromToTime"] = "" + Convert.ToDateTime(FromTime).ToString("hh:mm tt") + " To " + Convert.ToDateTime(Totime).ToString("hh:mm tt") + "";
                                dr["DoctorAppoinmentScheduleID"] = drMainDetail["DoctorAppoinmentScheduleID"].ToString();
                                dr["NoOfAppointment"] = i;
                                dsDoctorSchedule.Tables[0].Rows.Add(dr);
                            }
                            else
                            {
                                if (dsDoctorTimedetail.Tables[3].Rows.Count > 0)
                                {
                                    Connect();
                                    SqlCommand cmd = new SqlCommand("update PatientAppointmentDetails set FromTime='" + FromTime + "' , ToTime='" + Totime + "'  where DoctorAppoinmentScheduleID=" + dsDoctorTimedetail.Tables[0].Rows[0]["DoctorAppoinmentScheduleID"].ToString() + " and NoOfAppointment=" + i + "  and AppointmentDate='" + AppointmentDate + "' and HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0", con);
                                    con.Open();
                                    int a = cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            //---
                            FromTime = Totime;
                        }
                    }
                    else
                    {
                        DataRow dr = dsDoctorSchedule.Tables[0].NewRow();
                        dr["FromTime"] = Convert.ToString(FromTime);
                        dr["ToTime"] = Convert.ToString(Totime);
                        dr["FromToTime"] = "" + FromTime + " To " + Totime + "";
                        dr["DoctorAppoinmentScheduleID"] = drMainDetail["DoctorAppoinmentScheduleID"].ToString();
                        dr["NoOfAppointment"] = 0;
                        dsDoctorSchedule.Tables[0].Rows.Add(dr);
                    }
                }
            }
            DataRow drAppointment = dsDoctorSchedule.Tables[0].NewRow();
            List<string[]> fillFromTO = new List<string[]>();
            List<string> fillFromTO1 = new List<string>();
            if (dsDoctorTimedetail.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr in dsDoctorTimedetail.Tables[3].Rows)
                {
                    if (Convert.ToBoolean(dr["CancelAppointment"].ToString()) == true)
                    {
                        dsDoctorTimedetail.Reset();
                        return new JsonResult { Data = "Doctor Not Comming", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
            }
            foreach (DataRow dr in dsDoctorSchedule.Tables[0].Rows)
            {
                fillFromTO.Add(new string[] { dr["FromToTime"].ToString(), dr["NoOfAppointment"].ToString() });
            }
            if (Mode.ToString() == "Edit")
            {
                NoOfAppointment = 0;
            }
            return new JsonResult { Data = fillFromTO, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        DateTime FromTime;
        DateTime Totime;
        public ActionResult BindFromToTime1(string AppointmentType, string DoctorID, string AppointmentDate)
        {
            string Mode = "";
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            PatientAppointment.PatientAppointmentNO = AppointmentType;
            PatientAppointment.DoctorID = DoctorID;
            DataSet dsDoctorTimedetail = new DataSet();
            if (PatientAppointment.PatientAppointmentNO == "Weekly")
            {
                dsDoctorTimedetail = _PatientAppointment.GetPatientAppointmentDetailForDoctorDetail(HospitalID, LocationID, Convert.ToInt32(PatientAppointment.DoctorID), PatientAppointment.PatientAppointmentNO, Convert.ToString(Convert.ToDateTime(AppointmentDate).DayOfWeek.ToString()), Convert.ToDateTime(AppointmentDate));
            }
            else
                if (PatientAppointment.PatientAppointmentNO == "Monthly")
                {
                    dsDoctorTimedetail = _PatientAppointment.GetPatientAppointmentDetailForDoctorDetail(HospitalID, LocationID, Convert.ToInt32(PatientAppointment.DoctorID), PatientAppointment.PatientAppointmentNO, Convert.ToString(Convert.ToDateTime(AppointmentDate).Day), Convert.ToDateTime(AppointmentDate));
                }
                else
                {
                    dsDoctorTimedetail = _PatientAppointment.GetPatientAppointmentDetailForDoctorDetail(HospitalID, LocationID, Convert.ToInt32(PatientAppointment.DoctorID), PatientAppointment.PatientAppointmentNO, "Daily", Convert.ToDateTime(AppointmentDate));
                }
            GetEmptyDatasetPatientSchedule();
            //foreach (DataRow drMainDetail in dsDoctorTimedetail.Tables[0].Rows)
            //{
            string FromTime;
            string ToTime;
            //   // DateTime Endtime;
            //    FromTime = Convert.ToDateTime(drMainDetail["FromTime"]).ToString("hh:mm:ss");
            //    Totime = Convert.ToDateTime(drMainDetail["ToTime"]).ToString("hh:mm:ss");
            //}
            //    if (dsDoctorTimedetail.Tables[0].Rows.Count > 0)
            //    {
            //        int PerAppointment = 0;
            //        //Endtime = Convert.ToDateTime(drMainDetail["ToTime"].ToString());
            //        PerAppointment = Convert.ToInt32(drMainDetail["PerAppointmentTimeinMinute"].ToString());
            //        if (PatientAppointment.PatientAppointmentNO != "OnCalls")
            //        {
            //            for (int i = 1; Convert.ToInt32(drMainDetail["NoOfAppointment"].ToString()) >= i; i++)
            //            {
            //                DataView dvDetail = new DataView(dsDoctorTimedetail.Tables[1], "DoctorAppoinmentScheduleID = " + drMainDetail["DoctorAppoinmentScheduleID"].ToString() + " and NoOfAppointment = " + i + "", "", DataViewRowState.CurrentRows);
            //                Totime = Convert.ToDateTime(FromTime).AddMinutes(+PerAppointment).ToString("hh:mm:ss");
            //                if (Mode.ToString() == "Edit")
            //                {
            //                    if (i == NoOfAppointment)
            //                    {
            //                        dvDetail = new DataView(dsDoctorTimedetail.Tables[1], "DoctorAppoinmentScheduleID = " + drMainDetail["DoctorAppoinmentScheduleID"].ToString() + " and NoOfAppointment = " + 0 + "", "", DataViewRowState.CurrentRows);
            //                    }
            //                }
            //                if (dvDetail.Count == 0)
            //                {
            //                    DataRow dr = dsDoctorSchedule.Tables[0].NewRow();
            //                    dr["FromTime"] = FromTime;
            //                    dr["ToTime"] = Totime;
            //                    dr["FromToTime"] = "" + Convert.ToDateTime(FromTime).TimeOfDay + " To " + Convert.ToDateTime(Totime).TimeOfDay + "";
            //                    dr["DoctorAppoinmentScheduleID"] = drMainDetail["DoctorAppoinmentScheduleID"].ToString();
            //                    dr["NoOfAppointment"] = i;
            //                    dsDoctorSchedule.Tables[0].Rows.Add(dr);
            //                }
            //                FromTime = Totime;
            //            }
            //        }
            //        else
            //        {
            //            DataRow dr = dsDoctorSchedule.Tables[0].NewRow();
            //            dr["FromTime"] = Convert.ToString(FromTime);
            //            dr["ToTime"] = Convert.ToString(Totime);
            //            dr["FromToTime"] = "" + FromTime + " To " + Totime + "";
            //            dr["DoctorAppoinmentScheduleID"] = drMainDetail["DoctorAppoinmentScheduleID"].ToString();
            //            dr["NoOfAppointment"] = 0;
            //            dsDoctorSchedule.Tables[0].Rows.Add(dr);
            //        }
            //    }
            //}
            // DataRow drAppointment = dsDoctorSchedule.Tables[0].NewRow();
            List<string[]> fillFromTO = new List<string[]>();
            List<string> fillFromTO1 = new List<string>();
            foreach (DataRow dr in dsDoctorTimedetail.Tables[0].Rows)
            {
                fillFromTO.Add(new string[] { 
              FromTime = Convert.ToDateTime(dr["FromTime"]).ToString("hh:mm"),
              ToTime = Convert.ToDateTime(dr["ToTime"]).ToString("hh:mm"),
                //FromTime = Convert.ToDateTime(dr["FromTime"].ToString("hh:mm:ss")), 
                // ToTime=  Convert.ToDateTime(dr["ToTime"].ToString("hh:mm:ss"))
                });
            }
            //if (Mode.ToString() == "Edit")
            //{
            //    NoOfAppointment = 0;
            //}
            return new JsonResult { Data = fillFromTO, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult BindNoOfAppointment(string AppointmentTime, string DoctorID)
        {
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select FromTime,ToTime, NoOfAppointment, FromTime+' To '+ToTime as FromToTime  from DoctorAppointmentSchedule where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and DoctorID='" + DoctorID + "' and RowStatus=0", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            DateTime Endtime;
            int PerAppointment = 0;
            List<string> list = new List<string>();
            int I = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string AppointmentTime1 = Convert.ToDateTime(ds.Tables[0].Rows[I]["FromTime"]).ToString("hh:mm") + " To " + Convert.ToDateTime(ds.Tables[0].Rows[I]["ToTime"]).ToString("hh:mm");
                if (AppointmentTime1 == AppointmentTime)
                {
                    FromTime = Convert.ToDateTime(ds.Tables[0].Rows[I]["FromTime"]);
                    Totime = Convert.ToDateTime(ds.Tables[0].Rows[I]["Totime"]);
                    DataView dvDetail = new DataView(ds.Tables[0], "FromToTime LIKE '" + AppointmentTime + "'", "", DataViewRowState.CurrentRows);
                    list.Add(ds.Tables[0].Rows[I]["NoOfAppointment"].ToString());
                }
                I++;
            }
            return Json(list);
        }
        [HttpPost]
        public JsonResult Delete(int PatientID)
        {
            string del = null;
           // PatientAppointmentScheduleID = Convert.ToInt32(Request.Form["PatientAppointmentScheduleID"]);
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            try
            {
                string DependaincyName = _PatientAppointment.DeletePatientAppointmentDetail(Convert.ToInt32(PatientID));
                del = "PatientAppointment Deleted Successfully";
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = del, JsonRequestBehavior = JsonRequestBehavior.AllowGet }; ;
        }
        public JsonResult GetDoctorRescheduleAppointment(int DoctorID, DateTime AppointmentDate, string AppointmentType)
        {
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            ModelState.Clear();
            return new JsonResult { Data = _PatientAppointment.GetDoctorRescheduleAppointment(DoctorID, AppointmentDate, AppointmentType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //  return View(db.GetDepartment(id));
        }
        public JsonResult GetAllFinancialYear()
        {
            DataSet ds = _PatientAppointment.GetAllFinancialYear();
            DataView dvTest = new DataView(ds.Tables[0], "FinancialYearID = " + ds.Tables[1].Rows[0]["FinancialYearID"].ToString() + " ", "", DataViewRowState.CurrentRows);
            dvTest.ToTable().Rows[0]["FinancialYear"].ToString();
            foreach (DataRow dr in dvTest.ToTable().Rows)
            {
                PatientAppointmentscheduleList.Add(new PatientAppointmentSchedule
                {
                    FinancialYear = dr["FinancialYear"].ToString(),
                    FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]).ToString()
                });
            }
            return new JsonResult { Data = PatientAppointmentscheduleList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult PatientAppointmentSchedule()
        {
            PatientAppointmentSchedule objReschedule = new Models.Patient.PatientAppointmentSchedule();
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            objReschedule.PatientID = _PatientAppointment.GetNextOutsidePatientID();
            return View(objReschedule);
        }
        [HttpPost]
        public ActionResult Save(string DoctorName, string DoctorID, string Reason, string FromTime1, string ToTime, string AppointmentDate, string AppointmentType, string CancelAppointment)
        {
            string message = "";
            try
            {
                BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
                PatientAppointmentSchedule objReschedule = new Models.Patient.PatientAppointmentSchedule();
                //objReschedule.HospitalID = HospitalID;
                //objReschedule.LocationID = LocationID;
                objReschedule.HospitalID = Convert.ToInt32(Session["HospitalID"]);
                objReschedule.LocationID = Convert.ToInt32(Session["LocationID"]);
                int UserID = Convert.ToInt32(Session["UserID"]);
                objReschedule.RescheduleID = "0";
                objReschedule.Reason = Reason;
                objReschedule.DoctorName = DoctorName;
                objReschedule.DoctorID = DoctorID;
                objReschedule.DoctorAppoinmentID = "0";
                objReschedule.PatientAppointmentID = "0";
                objReschedule.Date = Convert.ToDateTime(AppointmentDate);
                if (CancelAppointment == "true")
                {
                    objReschedule.CancelAppointment = true;
                }
                else
                {
                    objReschedule.CancelAppointment = false;
                }
                objReschedule.AppointmentType = AppointmentType;
                objReschedule.FromTime = FromTime1;
                objReschedule.ToTime = ToTime;
                if (_PatientAppointment.AddDoctorReschedule(objReschedule))
                {
                    //ModelState.Clear();
                    //ViewData["flag"] = "Done";
                    //TempData["msg"] = "Appointment Added Successfully";
                    ModelState.Clear();
                    message = "Appointment Added Successfully";
                    //TempData["msg"] = "Doctor Appointment Reschedule Successfully";
                   
                    return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                return RedirectToAction("PatientAppointmentSchedule", "PatientAppointmentSchedule");

            }
            catch (Exception ex)
            {
                return RedirectToAction("PatientAppointmentSchedule", "PatientAppointmentSchedule");

            }
            //return RedirectToAction("PatientAppointmentSchedule", "PatientAppointmentSchedule");
        }
        [HttpPost]
        public ActionResult PatientAppointmentSchedule(PatientAppointmentSchedule objPatientAppointmentSchedule, FormCollection fc)
        {
            // PatientAppointmentSchedule objPatientAppointmentSchedule=new Models.Patient.PatientAppointmentSchedule();
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            var delimiters = new char[] { 'T', 'o' };
            var results = objPatientAppointmentSchedule.FromTime.Split(delimiters);
            objPatientAppointmentSchedule.FinancialYearID = fc["FinancialYearID"];
            objPatientAppointmentSchedule.PatientRegNo = fc["0"];
            objPatientAppointmentSchedule.PatientID = fc["AppointmentNo"];
            objPatientAppointmentSchedule.PatientAppointmentNO = fc["AppointmentNo"];
            objPatientAppointmentSchedule.PatientName = fc["FirstName"];
            objPatientAppointmentSchedule.PFirstName = fc["FirstName"];
            objPatientAppointmentSchedule.PMiddleName = fc["PMiddleName"];
            objPatientAppointmentSchedule.PLastName = fc["PLastName"];
            objPatientAppointmentSchedule.PatientStatus = fc["False"];
            objPatientAppointmentSchedule.Day = fc["Day"];
            objPatientAppointmentSchedule.Month = fc["Month"];
            objPatientAppointmentSchedule.Year = fc["Year"];
            objPatientAppointmentSchedule.PFPatientName = fc["PFPatientName"];
            objPatientAppointmentSchedule.DateOFBirth = fc["DateOFBirth"];
            // objPatientAppointmentSchedule.Gender = Request.Form["Gender"];
            if (Request.Form["Days"] != "")
            {
                objPatientAppointmentSchedule.Day = fc["Day"].ToString();
                objPatientAppointmentSchedule.Age = objPatientAppointmentSchedule.Day.ToString();
                objPatientAppointmentSchedule.AgeType = "Days";
            }
            if (Request.Form["Months"] != "")
            {
                objPatientAppointmentSchedule.Month = fc["Month"].ToString();
                objPatientAppointmentSchedule.Age = objPatientAppointmentSchedule.Month;
                objPatientAppointmentSchedule.AgeType = "Month";
            }
            if (Request.Form["Years"] != "")
            {
                objPatientAppointmentSchedule.Year = fc["Year"].ToString();
                objPatientAppointmentSchedule.Age = objPatientAppointmentSchedule.Year;
                objPatientAppointmentSchedule.AgeType = "Year";
            }
            objPatientAppointmentSchedule.AppointmentDate = fc["AppointDateID"];
            //  objPatientAppointmentSchedule.AppointmentType = Request.Form["AppointmentType"];
            objPatientAppointmentSchedule.AppointmentType = fc["AppointType"];
            objPatientAppointmentSchedule.DoctorPrintName = fc["ConsultantDoctorName"];
            objPatientAppointmentSchedule.PatientAppointmentScheduleID = fc["PatientAppointmentScheduleID"];
            if (objPatientAppointmentSchedule.DoctorAppoinmentScheduleID == "")
            {
                objPatientAppointmentSchedule.DoctorAppoinmentScheduleID = fc["DoctorAppoinmentScheduleID"];
            }
            else
            {
                objPatientAppointmentSchedule.DoctorAppoinmentScheduleID = fc["DoctorAppoinmentScheduleID"];
            }
            objPatientAppointmentSchedule.FromTime = results[0];
            objPatientAppointmentSchedule.ToTime = results[2];
            objPatientAppointmentSchedule.MobileNo = fc["MobileNo"].ToString();
            objPatientAppointmentSchedule.AppointmentNoID = fc["AppointmentNoID"];
            #region DoctorSchedule
            BindFromToTime(objPatientAppointmentSchedule.AppointmentType, objPatientAppointmentSchedule.DoctorID, fc["AppointDateID"].ToString());
            BindNoOfAppointment(fc["AppointmentTime"], objPatientAppointmentSchedule.DoctorID);
            #endregion
            //dsDoctorSchedule = _PatientAppointment.GetPatientAppointmentDetails(HospitalID, LocationID, Convert.ToInt32(objPatientAppointmentSchedule.PatientID));
            if (_PatientAppointment.Save(objPatientAppointmentSchedule, dsDoctorSchedule))
            {
                ModelState.Clear();
                ViewData["flag"] = "Done";
                TempData["msg"] = "Appointment Save Sucessfully";
            }
            else
            {
                ViewData["flag"] = "Error";
            }
            return RedirectToAction("PatientAppointmentSchedule", "PatientAppointmentSchedule");
        }

        public JsonResult GetAllPatientAppointmentData(string Date)
        {
            BL_PatientAppointmentSchedule _PatientAppointment = new BL_PatientAppointmentSchedule();
            PatientAppointmentSchedule PatientAppointment = new PatientAppointmentSchedule();
            DataSet ds = _PatientAppointment.GetAllPatientAppointmentData(Date);
            List<PatientAppointmentSchedule> PatientAppointmentscheduleList = new List<PatientAppointmentSchedule>();
 
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientAppointmentscheduleList.Add(new PatientAppointmentSchedule
                {
                    PatientID = dr["PatientID"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    DoctorName = dr["Doctor Name"].ToString(),
                    AppointmentDate = Convert.ToDateTime(dr["AppointmentDate"]).ToString("yyyy-MM-dd"),
                    FromTime = dr["FromTime"].ToString(),
                    ToTime = dr["ToTime"].ToString(),
                    //PatientAppointment.Age = dr["Age"].ToString();
                    //PatientAppointment.DateOFBirth = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd");
                });

            }

            // PatientAppointmentscheduleList.Add(PatientAppointment);

            return new JsonResult { Data = PatientAppointmentscheduleList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //public JsonResult CalGetDaysAge(string datebirth)
        //{
        //    DateTime dtpDateOfBirth = Convert.ToDateTime(datebirth);
        //    List<PatientOPD> add = new List<Models.Patient.PatientOPD>();
        //    PatientOPD obj = new Models.Patient.PatientOPD();
        //    DateTime dtToday = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        //    DateTime dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);
        //    int Year = 0;
        //    int Month = 0;
        //    int Day = 0;

        //    dtBeforeDate = dtBeforeDate.AddYears(1);
        //    while (dtToday >= dtBeforeDate)
        //    {
        //        dtBeforeDate = dtBeforeDate.AddYears(1);
        //        Year++;
        //    }
        //    dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);
        //    dtBeforeDate = dtBeforeDate.AddMonths(1);
        //    while (dtToday >= dtBeforeDate)
        //    {
        //        dtBeforeDate = dtBeforeDate.AddMonths(1);
        //        Month++;
        //    }

        //    dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);

        //    dtBeforeDate = dtBeforeDate.AddMonths(Month);
        //    var a = dtBeforeDate;
        //    //if(a== getTime())
        //    //{

        //    //}
        //    TimeSpan diffResult = dtToday.Date - dtBeforeDate.Date;

        //    int TotalDay = Convert.ToInt32(diffResult.TotalDays);
        //    int TotalMonth = Month % 12;

        //    obj.day = TotalDay.ToString();

        //    obj.Month = TotalMonth.ToString();

        //    obj.year = Year.ToString();
        //    add.Add(obj);
        //    return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //}
        public JsonResult BindCaculteAge(string date)
        {
            string aa = "";
            BL_PatientOPDBill db = new BL_PatientOPDBill();
            if (date != "NaN-NaN-NaN")
            {
                aa = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            }
            return new JsonResult { Data = aa, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}




  
