using KeystoneProject.Buisness_Logic.MISReport;
using KeystoneProject.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Report
{
    public class MISPatientWiseCollectionDetailsReportController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID;
        int PatientRegNo;
        string BillType;
        string PatientType;
        BL_MISPatientWiseCollectionDetailsReport BlReport = new BL_MISPatientWiseCollectionDetailsReport();
        MISPatientWiseCollectionDetailsReport objModel = new MISPatientWiseCollectionDetailsReport();

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        [HttpGet]
        public ActionResult MISPatientWiseCollectionDetailsReport()
        {
           // MISPatientWiseCollectionDetailsReport obj = new MISPatientWiseCollectionDetailsReport();
            return View();
        }

        public ActionResult MISPatientWiseCollectionDetailsReport(string DateFrom, string DateTo, string PatientRegNO, string UserID, string BillType, string PatientType)
        {


            BL_MISPatientWiseCollectionDetailsReport BlReport1 = new BL_MISPatientWiseCollectionDetailsReport();
            HospitlLocationID();

            //DateFrom = Request.Form["Fromdate"];
            //DateTo =Request.Form["Todate"];

            if ((PatientRegNO == "") || (PatientRegNO == "null"))
            {
                PatientRegNO = "%";
            }
            if ((UserID == "" )|| (UserID == "null"))
            {
                UserID = "%";
            }
            if ((BillType == "") || (BillType == "All"))
            {
                BillType = "%";
            }
            if ((PatientType == "") || (PatientType == "All"))
            {
                PatientType = "%";
            }

            return new JsonResult { Data = BlReport1.MISPatientWiseCollectionDetailsReportDetail(HospitalID, LocationID, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), PatientRegNO, UserID, BillType, PatientType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUserNameValue(string prefix)
        {
            BL_MISUserWiseCollection BlReport = new BL_MISUserWiseCollection();

            return new JsonResult { Data = BlReport.GetUserName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPatientNameValue(string prefix)
        {
            BL_MISServiceWiseCollectionReport BlReport = new BL_MISServiceWiseCollectionReport();

            return new JsonResult { Data = BlReport.GetPatientName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}