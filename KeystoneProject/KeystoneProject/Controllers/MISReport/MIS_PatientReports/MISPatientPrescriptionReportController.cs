using KeystoneProject.Buisness_Logic.MISReport;
using KeystoneProject.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Report
{
    public class MISPatientPrescriptionReportController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID;

        BL_MISPatientPrescriptionReport BlReport = new BL_MISPatientPrescriptionReport();
        MISPatientPrescriptionReport objModel = new MISPatientPrescriptionReport();

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        [HttpGet]
        public ActionResult MISPatientPrescriptionReport()
        {
            return View();
        }
        public ActionResult MISPatientPrescriptionReport(string DateFrom, string DateTo, string ConsultantDoctore, string PatientType, string PatientPrescriptionID)
        {


            BL_MISPatientPrescriptionReport BlReport1 = new BL_MISPatientPrescriptionReport();
            HospitlLocationID();

            //DateFrom = Request.Form["Fromdate"];
            //DateTo =Request.Form["Todate"];

            if ((ConsultantDoctore == "") || (ConsultantDoctore == "null"))
            {
                ConsultantDoctore = "%";
            }
            if ((PatientPrescriptionID == "") || (PatientPrescriptionID == "null"))
            {
                PatientPrescriptionID = "%";
            }
           
            if ((PatientType == "") || (PatientType == "All"))
            {
                PatientType = "%";
            }

            return new JsonResult { Data = BlReport1.MISPatientPrescriptionReport(HospitalID, LocationID, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo),ConsultantDoctore,PatientType,PatientPrescriptionID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult GetDoctor(string DoctorID)
        {

            Buisness_Logic.MISReport.BL_MISPatientPrescriptionReport obj = new Buisness_Logic.MISReport.BL_MISPatientPrescriptionReport();

            return new JsonResult { Data = obj.GetDoctor(DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}