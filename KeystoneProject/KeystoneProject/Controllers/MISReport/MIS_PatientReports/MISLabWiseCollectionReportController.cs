using KeystoneProject.Buisness_Logic.MISReport;
using KeystoneProject.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Report
{
    public class MISLabWiseCollectionReportController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID;

        BL_MISLabWiseCollectionReport BlReport = new BL_MISLabWiseCollectionReport();
        MISLabWiseCollectionReport objModel = new MISLabWiseCollectionReport();

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        [HttpGet]
        public ActionResult MISLabWiseCollectionReport()
        {
            return View();
        }

        public ActionResult MISLabWiseCollectionReport(string DateFrom, string DateTo, string TestID, string CategoryID, string PatientType)
        {
            BL_MISLabWiseCollectionReport BlReport1 = new BL_MISLabWiseCollectionReport();
            HospitlLocationID();

            //DateFrom = Request.Form["Fromdate"];
            //DateTo =Request.Form["Todate"];

            if ((TestID == "") || (TestID == "null") || (TestID==null))
            {
                TestID = "%";
            }
            if ((CategoryID == "") || (CategoryID == "null") || (CategoryID == null))
            {
                CategoryID = "%";
            }
            if ((PatientType == "") || (PatientType == "All") )
            {
                PatientType = "ALL";
            }

            return new JsonResult { Data = BlReport1.MISLabWiseCollectionReport(HospitalID, LocationID, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), TestID, CategoryID, PatientType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult MISLabWiseCollectionReportDgv1(string DateFrom, string DateTo, string TestID, string CategoryID, string PatientType)
        {


            BL_MISLabWiseCollectionReport BlReport1 = new BL_MISLabWiseCollectionReport();
            HospitlLocationID();

            //DateFrom = Request.Form["Fromdate"];
            //DateTo =Request.Form["Todate"];

            if ((TestID == "") || (TestID == "null") || (TestID == null))
            {
                TestID = "%";
            }
            if ((CategoryID == "") || (CategoryID == "null") || (CategoryID == null))
            {
                CategoryID = "%";
            }

            if ((PatientType == "") || (PatientType == "All"))
            {
                PatientType = "ALL";
            }
            if ((PatientType == "%") )
            {
                PatientType = "ALL";
            }

            return new JsonResult { Data = BlReport1.MISLabWiseCollectionReportDgv1(HospitalID, LocationID, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), TestID, CategoryID, PatientType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetTestName(string prefix)
        {
            BL_MISLabWiseCollectionReport BlReport = new BL_MISLabWiseCollectionReport();

            return new JsonResult { Data = BlReport.GetTestName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetServiceGroupName(string prefix)
        {
            BL_MISLabWiseCollectionReport BlReport = new BL_MISLabWiseCollectionReport();

            return new JsonResult { Data = BlReport.GetServiceGroupName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public ActionResult MISLabWiseCollectionReport()
        //{

        //    return new JsonResult { Data = "Report", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //}

        public ActionResult RptMISLabWiseCollectionInDetail()
        {

            return View();

        }


	}
}