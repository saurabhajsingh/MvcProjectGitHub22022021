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
    public class MISHospitalDayBookReportController : Controller
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
        //
        // GET: /MISHospitalDayBookReport/
        public ActionResult MISHospitalDayBookReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MISHospitalDayBookReport(DateTime Type, string Type1)
        {
            Session["Type1"] = Type1;
            Session["Date"] = Type;
         

            return new JsonResult { Data = Type, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    
        public ActionResult MISHospitalDayBook(string Type1, string Date)
        {
            Session["Type1"] = Type1;
            Session["Date"] = Date;
            return View();
        }

      
	}
}