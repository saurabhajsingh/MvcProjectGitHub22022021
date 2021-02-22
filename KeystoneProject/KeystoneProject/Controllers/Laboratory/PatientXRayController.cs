using KeystoneProject.Buisness_Logic.Laboratory;
using KeystoneProject.Models.Laboratory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Laboratory
{
    public class PatientXRayController : Controller
    {

        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);

        BL_PatientXRay patientxray = new BL_PatientXRay();
        //
        // GET: /PatientXRay/
        public ActionResult PatientXRay()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PatientXRay(PatientXRay obj, FormCollection fc)
        {
            try
            {
                BL_PatientXRay _PatientXRay = new BL_PatientXRay();


                if (Request.Form["LabNo"] != null)
                {
                    obj.LabNo = Request.Form["LabNo"].ToString();
                }
                else
                {
                    obj.LabNo = "";
                }
                if (obj.TestID != null)
                    {
                        obj.TestID = fc["TestID"].ToString();
                    }
                    else
                    {
                        obj.TestID = "";
                    }

                if (_PatientXRay.SaveXRay(obj))
                {
                    
                    {
                        ModelState.Clear();

                        TempData["msg"] = "PatientXray Saved Successfully";
                        return RedirectToAction("PatientXRay", "PatientXRay");
                    }

                }

            }
            catch (Exception exe)
            {
                throw;
            }
            return View();
        }

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public JsonResult FillPatient(int PatientRegNO)
        {
            DataSet dsDepartment = new DataSet();
            dsDepartment = patientxray.GetPatientRegNo(PatientRegNO);

            List<PatientXRay> serch = new List<PatientXRay>();

            foreach (DataRow dr in dsDepartment.Tables[0].Rows)
            {
                serch.Add(new PatientXRay
                {

                    PatientRegNo = dr["PatientRegNO"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    DoctorName = dr["DoctorPrintName"].ToString()

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetPatientName(string prefix)
        {
            BL_PatientXRay objPatient = new BL_PatientXRay();
            List<PatientXRay> searchList = new List<PatientXRay>();
            DataSet ds1 = objPatient.GetPatient(prefix);

            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    PatientXRay Patient = new PatientXRay();

                    Patient.PatientRegNo = dr["PatientRegNo"].ToString();

                    Patient.PatientName = dr["PatientName"].ToString();

                    searchList.Add(Patient);
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult PatientBind(string FromDate, string Todate)
        {
            BL_PatientXRay BL_obj = new BL_PatientXRay();
            DataSet ds = new DataSet();
            List<PatientXRay> searclist = new List<Models.Laboratory.PatientXRay>();

            ds = BL_obj.SearchPatientLabResultEntryByNameID("%",Convert.ToDateTime(FromDate), Convert.ToDateTime(Todate));


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientXRay obj = new PatientXRay();

                obj.PatientName = dr["PatientName"].ToString();
                obj.PatientType = dr["PatientType"].ToString();
                obj.LabNo = dr["LabNo"].ToString();
                obj.LabDate = dr["LAB DATE"].ToString();

                searclist.Add(obj);

            }
            return Json(searclist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllClickData(int LabNo)
        {
            BL_PatientXRay BL_obj = new BL_PatientXRay();
            DataSet ds = new DataSet();
            List<PatientXRay> searclist = new List<Models.Laboratory.PatientXRay>();

            ds = BL_obj.GetPatientWiseResultForPatientSearch(LabNo);

                   

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                PatientXRay obj = new PatientXRay();
                obj.LabNo = ds.Tables[0].Rows[0]["LabNo"].ToString();
                obj.PatientRegNo = ds.Tables[0].Rows[0]["PatientRegNo"].ToString();
                obj.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                obj.DoctorName = ds.Tables[0].Rows[0]["DoctorPrintName"].ToString();


                obj.TestID = dr["TestID"].ToString();
                obj.Testname = dr["TestName"].ToString();

                searclist.Add(obj);

            }
            return Json(searclist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPatientWiseValue(int LabNo, int TestID)
        {
            BL_PatientXRay BL_obj = new BL_PatientXRay();
            DataSet ds = new DataSet();
            List<PatientXRay> searclist = new List<Models.Laboratory.PatientXRay>();

            ds = BL_obj.GetPatientWiseXrayResult(LabNo,TestID);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientXRay obj = new PatientXRay();
                obj.Footer = ds.Tables[0].Rows[0]["Footer"].ToString();
                obj.TestID = ds.Tables[0].Rows[0]["TestID"].ToString();
                obj.LabNo = ds.Tables[0].Rows[0]["LabNo"].ToString();


                searclist.Add(obj);

            }
            return Json(searclist, JsonRequestBehavior.AllowGet);
          
        }

    }
}