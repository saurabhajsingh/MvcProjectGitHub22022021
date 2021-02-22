using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
//using KeystoneProject.Buisness_Logic.PharmacyReport;
//using KeystoneProject.Models.PharmacyReport;
using System.Configuration;
namespace KeystoneProject.Controllers.PharmacyReport
{
    public class PharmacyReportController : Controller
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;
        private void Connect()
        {
            HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        }
        //
        // GET: /PatientReport/
        public ActionResult medicalBill()
        {
            return View();
        }

        public ActionResult medicalBillReturn()
        {
            return View();
        }

        public ActionResult Pur_Order()
        {
            return View();
        }

        public ActionResult PurchaseReport()
        {
            return View();
        }
    }
}