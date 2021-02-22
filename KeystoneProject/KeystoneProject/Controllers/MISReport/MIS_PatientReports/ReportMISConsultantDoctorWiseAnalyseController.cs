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
    public class ReportMISConsultantDoctorWiseAnalyseController : Controller
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

        BL_ReportMISConsultantDoctorWiseAnalyse BLReportMISConsultantDoctorWiseAnalyse = new BL_ReportMISConsultantDoctorWiseAnalyse();
        ReportMISConsultantDoctorWiseAnalyse obj = new ReportMISConsultantDoctorWiseAnalyse();
        //
        // GET: /ReportMISConsultantDoctorWiseAnalyse/

        [HttpGet]
        public ActionResult ReportMISConsultantDoctorWiseAnalyse()
        {
            return View();
        }

        public JsonResult ConsultantDoctor(string prefix)
        {
            return new JsonResult { Data = BLReportMISConsultantDoctorWiseAnalyse.BindConsultant(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult ReportMISConsultantDoctorWiseAnalyse(DateTime DateFrom, DateTime DateTo, string PatientType, string DoctorID)
        {
            if (PatientType == "All")
            {
                PatientType = "%";
            }
            if (PatientType == "OPD")
            {
                PatientType = "OPD";
            }

            if (PatientType == "IPD")
            {
                PatientType = "IPD";
            }

            if (DoctorID == "")
            {
                DoctorID = "%";
            }

            return new JsonResult { Data = BLReportMISConsultantDoctorWiseAnalyse.ReportMISConsultantDoctorWiseAnalyse(DateFrom, DateTo, PatientType, DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    
        }
        public ActionResult RptMISConsultantDoctorWiseAnalyse()
        {
           
            return View();
        }

	}
}