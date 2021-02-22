using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
namespace KeystoneProject.Controllers.Master
{
    public class DoctorAppointmentScheduleController : Controller
    {
        //
        // GET: /DoctorAppointmentSchedule/
        [HttpGet]
        public ActionResult DoctorAppointmentSchedule()
        {
            DoctorAppointmentSchedule obj = new Models.Master.DoctorAppointmentSchedule();
            Buisness_Logic.Master.BL_AddDoctorAppointment Bl_obj = new Buisness_Logic.Master.BL_AddDoctorAppointment();
            obj.StoreAllDoctorAppointment = Bl_obj.SelectAllDoctorAppointment();
            return View(obj);
        }

        [HttpPost]
        public ActionResult DoctorAppointmentSchedule(DoctorAppointmentSchedule location)
        {

            try
            {


                Buisness_Logic.Master.BL_AddDoctorAppointment Bl_obj = new Buisness_Logic.Master.BL_AddDoctorAppointment();

                if (location.AppointmentType == "Monthly")
                {
                    if (Request.Form["month"] != null)
                    {
                        location.monthweek = Request.Form["month"].ToString();
                    }
                }
                if (location.AppointmentType == "Weekly")
                {
                    if (Request.Form["week1"] != null)
                    {
                        location.monthweek = Request.Form["week1"].ToString();
                    }
                }
                int ID = location.DoctorAppoinmentScheduleID;
                if (Bl_obj.save(location))
                {
                    if (ID > 0)
                    {
                        TempData["msg"] = "DoctorAppointment Updated Successfully";
                        ModelState.Clear();
                    }
                    else
                    {
                        TempData["msg"] = "DoctorAppointment Saved Successfully";
                        ModelState.Clear();
                    }
                    

                }
                location.StoreAllDoctorAppointment = Bl_obj.SelectAllDoctorAppointment();
                return RedirectToAction("DoctorAppointmentSchedule", "DoctorAppointmentSchedule", location);
            }
            catch(Exception ex)
            {
                TempData["msg"] = ex.Message;
               return RedirectToAction("DoctorAppointmentSchedule", "DoctorAppointmentSchedule", location);
            }
        }
       

        public JsonResult GetDoctor(string prefix)
        {
            DoctorAppointmentSchedule obj = new Models.Master.DoctorAppointmentSchedule();
            Buisness_Logic.Master.BL_AddDoctorAppointment Bl_obj = new Buisness_Logic.Master.BL_AddDoctorAppointment();
            DataSet ds = Bl_obj.GetDoctor(prefix);
            List<DoctorAppointmentSchedule> searchList = new List<DoctorAppointmentSchedule>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new DoctorAppointmentSchedule
                {
                    DoctorPrintName = dr["DoctorPrintName"].ToString(),
                    DoctorID = dr["DoctorID"].ToString()
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ShowAllDoctorAppointment()
        {
            DoctorAppointmentSchedule obj = new Models.Master.DoctorAppointmentSchedule();
            Buisness_Logic.Master.BL_AddDoctorAppointment Bl_obj = new Buisness_Logic.Master.BL_AddDoctorAppointment();
            obj.StoreAllDoctorAppointment = Bl_obj.SelectAllDoctorAppointment();
            List<DoctorAppointmentSchedule> searchList = new List<DoctorAppointmentSchedule>();
            foreach (DataRow dr in obj.StoreAllDoctorAppointment.Tables[0].Rows)
            {
                searchList.Add(new DoctorAppointmentSchedule
                {
                    DoctorPrintName = dr["DoctorPrintName"].ToString(),
                    DoctorID = dr["DoctorID"].ToString()

                });

            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult GetBindDoctor(string DoctorName)
        {
            List<string[]> search = new List<string[]>();
            try
            {
                DoctorAppointmentSchedule obj = new Models.Master.DoctorAppointmentSchedule();
                Buisness_Logic.Master.BL_AddDoctorAppointment Bl_obj = new Buisness_Logic.Master.BL_AddDoctorAppointment();
                DataSet ds = new DataSet();
                DataSet dsDoctorID = new DataSet();
                dsDoctorID = Bl_obj.DoctorName(DoctorName);
                if (dsDoctorID.Tables[0].Rows.Count > 0)
                {
                    string DoctorID = dsDoctorID.Tables[0].Rows[0]["DoctorID"].ToString();
                    obj.dgvDoctorSchedule = Bl_obj.GetDoctorAppointment(0, Convert.ToInt32(DoctorID));
                }

                if (obj.dgvDoctorSchedule.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in obj.dgvDoctorSchedule.Tables[2].Rows)
                    {
                        search.Add(new string[] { dr["DoctorAppoinmentScheduleID"].ToString(), dr["AppointmentType"].ToString(), dr["Doctor Name"].ToString(), dr["FromTime"].ToString(), dr["ToTime"].ToString() });
                    }
                }
                else
                {
                    search = null;
                }
            }

            catch (Exception ex)
            {

            }
            return Json(search);
        }
        public JsonResult EditDoctorAppointmentData(int DoctorAppoinmentScheduleID, int DoctorID)
        
        {
            DoctorAppointmentSchedule AddDoctorAppoinment = new Models.Master.DoctorAppointmentSchedule();
            Buisness_Logic.Master.BL_AddDoctorAppointment objAppointment = new Buisness_Logic.Master.BL_AddDoctorAppointment();
           
          //  Business_Logic.Master.HospitalMaster.BL_AddDoctorAppointment objAppointment = new Business_Logic.Master.HospitalMaster.BL_AddDoctorAppointment(); //calling class DBdata

            DataSet ds = objAppointment.GetDoctorAppointment(DoctorAppoinmentScheduleID, DoctorID);

            ArrayMonthDay[] objmonthweek = new ArrayMonthDay[ds.Tables[1].Rows.Count];

            //AddDoctorAppoinment.DoctorID = ds.Tables[0].Rows[0]["DoctorID"].ToString();
            AddDoctorAppoinment.DoctorPrintName = ds.Tables[0].Rows[0]["DoctorPrintName"].ToString();
            AddDoctorAppoinment.DoctorID = ds.Tables[0].Rows[0]["DoctorID"].ToString();
            AddDoctorAppoinment.DoctorAppoinmentScheduleID = Convert.ToInt32(ds.Tables[0].Rows[0]["DoctorAppoinmentScheduleID"].ToString());

            AddDoctorAppoinment.AppointmentType = ds.Tables[0].Rows[0]["AppointmentType"].ToString();
            AddDoctorAppoinment.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromDate"]);
            AddDoctorAppoinment.FromTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromTime"]);
            AddDoctorAppoinment.ToTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["ToTime"]);

            AddDoctorAppoinment.FromDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromDate"]).ToString("yyyy-MM-dd");
            AddDoctorAppoinment.FromTime1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromTime"]).ToString("hh:mm");
            AddDoctorAppoinment.ToTime1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["ToTime"]).ToString("hh:mm");

            AddDoctorAppoinment.NoOfAppointment = Convert.ToInt32(ds.Tables[0].Rows[0]["NoOfAppointment"].ToString());
            AddDoctorAppoinment.PerAppointmentTimeinMinute = Convert.ToInt32(ds.Tables[0].Rows[0]["PerAppointmentTimeinMinute"].ToString());
            AddDoctorAppoinment.EndDurationStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["EndDurationStatus"].ToString());



            List<DoctorAppointmentSchedule> search = new List<DoctorAppointmentSchedule>();

            if (AddDoctorAppoinment.AppointmentType == "Weekly")
            {


                string[] strArray = new string[ds.Tables[1].Rows.Count];
                int i = 0;
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    objmonthweek[i] = new ArrayMonthDay();
                    objmonthweek[i].monthweek = Convert.ToString(dr["DayDate"]);
                    i++;
                }

            }
               

            if (AddDoctorAppoinment.AppointmentType == "Monthly")
            {
                int j = 0;
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                  objmonthweek[j] = new ArrayMonthDay();
                    objmonthweek[j].monthweek =Convert.ToString( dr["DayDate"]);
                    j++;
                }
            }

            AddDoctorAppoinment.contact = objmonthweek;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                AddDoctorAppoinment.DayDate = dr["DayDate"].ToString();
            }
            search.Add(AddDoctorAppoinment);

            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult DeleteDoctorAppointment(int DoctorAppoinmentScheduleID)
        {
            string _Del = null;
            try
            {
                DoctorAppointmentSchedule AddDoctorAppoinment = new Models.Master.DoctorAppointmentSchedule();
                Buisness_Logic.Master.BL_AddDoctorAppointment objdb = new Buisness_Logic.Master.BL_AddDoctorAppointment();
         
                string DependaincyName = objdb.DeleteDoctorAppointmentSchedule(Convert.ToInt32(DoctorAppoinmentScheduleID));

                if (DependaincyName == "Delete")
                {
                    _Del = "DoctorAppointment Deleted Successfully";
                }
                else
                {
                    _Del = "Can not Delete";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ParAppoimentTime(string FromTime, string ToTime, string No)
        {
         //   TimeSpan time = Convert.ToDateTime(FromTime) + Convert.ToDateTime(ToTime);

            decimal PerAppointment = 0;
            TimeSpan result = DateTime.Parse(ToTime) - DateTime.Parse(FromTime);
            decimal TotalMinute = Convert.ToInt32(result.TotalMinutes);
            if (TotalMinute < 0)
            {
                TotalMinute = 1440 + TotalMinute;
            }
            PerAppointment = Convert.ToInt32(No);
            PerAppointment = TotalMinute / PerAppointment;
            string NoPerOppoiment = PerAppointment.ToString();


            return new JsonResult { Data = NoPerOppoiment, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}