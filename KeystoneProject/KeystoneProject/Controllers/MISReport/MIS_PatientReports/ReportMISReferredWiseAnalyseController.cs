using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace KeystoneProject.Controllers.MISReport.MIS_PatientReports
{
    public class ReportMISReferredWiseAnalyseController : Controller
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        BL_ReportMISReferredWiseAnalyse BLobj = new BL_ReportMISReferredWiseAnalyse();
        ReportMISReferredWiseAnalyse obj = new ReportMISReferredWiseAnalyse();
        //
        // GET: /ReportMISReferredWiseAnalyse/
        public ActionResult ReportMISReferredWiseAnalyse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportMISReferredWiseAnalyse(ReportMISReferredWiseAnalyse obj)
        {
            return View();
        }

        public JsonResult ReffDoctor(string prefix)
        {
            return new JsonResult { Data = BLobj.BindConsultant(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult ReportMISReffDoctorWiseAnalyse(DateTime DateFrom, DateTime DateTo, string PatientType, string DoctorID)
        {
            var type = PatientType;
            if (PatientType == "All")
            {
                type = "%";
            }
            else
            {
                type = PatientType;

            }

            if (DoctorID == "")
            {
                DoctorID = "%";
            }

            return new JsonResult { Data = BLobj.ReportMISReffDoctorWiseAnalyse(DateFrom, DateTo, type, DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult RptReportMISReffDoctorWiseAnalyse()
        {
            //BLobj.ReportMISReffDoctorWiseAnalyse(DateFrom, DateTo, PatientType, DoctorID);

            return View();
        }

	}
}