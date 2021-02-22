using KeystoneProject.Buisness_Logic.MISReport;
using KeystoneProject.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Report
{
    public class MISPatientAccountStatusController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID;
        int PatientRegNo;
        string BillType;
        string PatientType;
        BL_MISPatientAccountStatus BlReport = new BL_MISPatientAccountStatus();
        MISPatientAccountStatus objModel = new MISPatientAccountStatus();

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        [HttpGet]
        public ActionResult MISPatientAccountStatus()
        {
            return View();
        }


        public ActionResult MISPatientAccountStatus(DateTime DateFrom, DateTime DateTo, string PatientType, string DisplayType, string PatientRegNo, string BillNo, string OPDIPDID)
        {


            BL_MISPatientAccountStatus BlReport1 = new BL_MISPatientAccountStatus();
            HospitlLocationID();

            //DateFrom = Request.Form["Fromdate"];
            //DateTo =Request.Form["Todate"];

            if ((PatientRegNo == "") || (PatientRegNo == "null"))
            {
                PatientRegNo = "%";
            }
            if ((OPDIPDID == "") || (OPDIPDID == "null"))
            {
                OPDIPDID = "%";
            }
            if ((DisplayType == "") || (DisplayType == "null"))
            {
                DisplayType = "%";
            }
         
            if ((PatientType == "") || (PatientType == "All"))
            {
                PatientType = "%";
            }

            return new JsonResult { Data = BlReport1.MISPatientAccountStatus(Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), PatientType, DisplayType, PatientRegNo, BillNo, OPDIPDID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult MISPatientAccountStatusDgv1(DateTime DateFrom, DateTime DateTo, string PatientType, string DisplayType, string PatientRegNo, string BillNo, string OPDIPDID)
        {


            BL_MISPatientAccountStatus BlReport1 = new BL_MISPatientAccountStatus();
            HospitlLocationID();

            //DateFrom = Request.Form["Fromdate"];
            //DateTo =Request.Form["Todate"];

            if ((PatientRegNo == "") || (PatientRegNo == "null"))
            {
                PatientRegNo = "%";
            }
            if ((OPDIPDID == "") || (OPDIPDID == "null"))
            {
                OPDIPDID = "%";
            }

            if ((PatientType == "") || (PatientType == "All"))
            {
                PatientType = "%";
            }

            return new JsonResult { Data = BlReport1.MISPatientAccountStatusDgv1(HospitalID, LocationID, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), PatientType, DisplayType, PatientRegNo, BillNo, OPDIPDID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult MISPatientAccountStatusDgv2(DateTime DateFrom, DateTime DateTo, string PatientType, string DisplayType, string PatientRegNo, string BillNo, string OPDIPDID)
        {


            BL_MISPatientAccountStatus BlReport1 = new BL_MISPatientAccountStatus();
            HospitlLocationID();

            //DateFrom = Request.Form["Fromdate"];
            //DateTo =Request.Form["Todate"];

            if ((PatientRegNo == "") || (PatientRegNo == "null"))
            {
                PatientRegNo = "%";
            }
            if ((OPDIPDID == "") || (OPDIPDID == "null"))
            {
                OPDIPDID = "%";
            }

            if ((PatientType == "") || (PatientType == "All"))
            {
                PatientType = "%";
            }

            return new JsonResult { Data = BlReport1.MISPatientAccountStatusDgv2(HospitalID, LocationID, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), PatientType, DisplayType, PatientRegNo, BillNo, OPDIPDID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult RptMISPatientAccountStatusServiceWise()
        {

            return View();
        }
	}
}