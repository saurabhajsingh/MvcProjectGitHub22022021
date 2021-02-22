using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Master
{
    public class TemporaryDoctorMasterController : Controller
    {

        TemporaryDoctorMaster obj = new TemporaryDoctorMaster();
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);

        BL_TemporaryDoctorMaster temporary = new BL_TemporaryDoctorMaster();

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        // GET: TemporaryDoctorMaster

            [HttpGet]
        public ActionResult TemporaryDoctorMaster()
        {
            return View();
        }
        public ActionResult PatientRegNO_Leave(string RegNo)
        {
            List<TemporaryDoctorMaster> fillList = new List<TemporaryDoctorMaster>();
            TemporaryDoctorMaster catl = new TemporaryDoctorMaster();

            DataSet dsPatientOPD = new DataSet();
            DataSet dsPatientOPDDetails = new DataSet();

            BL_TemporaryDoctorMaster BL_obj = new BL_TemporaryDoctorMaster();


            DataSet dsPatient = new DataSet();
            dsPatient.Reset();

            if (RegNo != "")

                dsPatient = BL_obj.GetPatientForTemporaryDoctorMaster(HospitalID, LocationID, Convert.ToInt32(RegNo));

            if (dsPatient.Tables.Count > 0)
            {
                if (dsPatient.Tables[0].Rows.Count > 0)
                {
                    catl.PatientRegNo = dsPatient.Tables[0].Rows[0]["PatientRegNO"].ToString();
                    catl.PatientType = dsPatient.Tables[0].Rows[0]["PatientType"].ToString();
                    catl.patientname = dsPatient.Tables[0].Rows[0]["PatientName"].ToString();
                    catl.PatientOPDIPDNO = dsPatient.Tables[0].Rows[0]["PatientOPDIPDNO"].ToString();
                    catl.Consultant = "Consultant";
                    catl.Referred = "Referred";
                    if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
                    {
                        #region IPD Detail Of Patient


                        if (dsPatient.Tables[0].Rows[0]["TemporaryDoctor"].ToString() != "" || dsPatient.Tables[0].Rows[0]["RefferedTemporaryDoctor"].ToString() != "0")
                        {
                            catl.TemporaryDoctor = dsPatient.Tables[0].Rows[0]["TemporaryDoctor"].ToString();
                            catl.RefferedTemporaryDoctor = dsPatient.Tables[0].Rows[0]["RefferedTemporaryDoctor"].ToString();
                        }



                        #endregion
                    }
                    else if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "OPD")
                    {


                        #region OPD Detail Of Patient

                        if (dsPatient.Tables[0].Rows[0]["TemporaryDoctor"].ToString() != "" || dsPatient.Tables[0].Rows[0]["RefferedTemporaryDoctor"].ToString() != "0")
                        {
                            catl.TemporaryDoctor = dsPatient.Tables[0].Rows[0]["TemporaryDoctor"].ToString();
                            catl.RefferedTemporaryDoctor = dsPatient.Tables[0].Rows[0]["RefferedTemporaryDoctor"].ToString();
                        }
                        #endregion
                    }

                }
            }
            fillList.Add(catl);
            return Json(fillList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult TemporaryDoctorMaster(TemporaryDoctorMaster obj1, FormCollection fc)
        {

            DataSet dsPatientdetails = new DataSet();
            obj1.patientname = Request.Form["patient_name"].ToString();
            obj1.PatientOPDIPDNO = Request.Form["PatientOPDIPDNO"].ToString();
            obj1.PatientRegNo = Request.Form["PatientRegNO"].ToString();
            obj1.PatientType = Request.Form["PatientType1"].ToString();

            if (obj1.TemporaryDoctor !="")
            {
                obj1.ConsultantDoctorName = Request.Form["ConsultantDoctorName"].ToString();
                obj1.ConsultantDoctorID = Request.Form["ConsultantDoctorID"].ToString();

                if (Request.Form["DepartMentID"] == "" || Request.Form["DepartMentID"] == null)
                {
                    obj1.DepartMentID = "";
                   
                }
                else
                {
                    obj1.DepartMentID = Request.Form["DepartMentID"].ToString();
                }

            }

            if (obj1.RefferedTemporaryDoctor !="")
            {
                obj1.ReferredByDoctorName = Request.Form["ReferredByDoctorName"].ToString();
                obj1.ReferredByDoctorID = Request.Form["ReferredByDoctorID"].ToString();
            }


            temporary.UpdateDoctorID(obj1);

            if (Convert.ToInt32(obj1.PatientRegNo) > 0)
            {
                TempData["Msg"] = "Record Updated Successfully !";
                ModelState.Clear();
                RedirectToAction("TemporaryDoctorMaster", "TemporaryDoctorMaster");
            }
            else
            {
                return RedirectToAction("TemporaryDoctorMaster", "TemporaryDoctorMaster");
            }

            return View(obj);
        }

        public JsonResult ShowAllTemporaryDoctorStatus()
        {
            BL_TemporaryDoctorMaster db = new BL_TemporaryDoctorMaster();

            return new JsonResult { Data = db.ShowAllTemporaryDoctorStatus(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}