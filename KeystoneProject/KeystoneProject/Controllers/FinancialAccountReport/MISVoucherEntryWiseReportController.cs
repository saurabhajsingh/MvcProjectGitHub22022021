using KeystoneProject.Buisness_Logic.FinancialAccountReport;
using KeystoneProject.Models.FinancialAccountReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers
{
    public class MISVoucherEntryWiseReportController : Controller
    {

        int HospitalID;
        int LocationID;
        int UserID;
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        BL_MISVoucherEntryWiseReport objbl = new BL_MISVoucherEntryWiseReport();
        MISVoucherEntryWiseReport objmodels = new MISVoucherEntryWiseReport();

        public static Nullable<DateTime> DateFrom { get; set; }
        public static Nullable<DateTime> DateTo { get; set; }

         [HttpGet]
        public ActionResult MISVoucherEntryWiseReport()
        {
            return View();
        }

         [HttpPost]
         public ActionResult MISVoucherEntryWiseReport(FormCollection collection)
         {

             objmodels.DateFrom = Convert.ToDateTime(collection["FromDate"]); 
             objmodels.DateTo = Convert.ToDateTime(collection["ToDate"]);
             objmodels.ScheduleName = collection["ScheduleName"];
             objmodels.AccountName = collection["AccountName"];
             objmodels.TransactionType = collection["TransactionType"];
             objmodels.Particular = collection["Particular"];
             objmodels.AccountID = collection["AccountID"];
             objmodels.VoucherName = collection["VoucherName"];
             objmodels.ScheduleID = collection["ScheduleID"];


             if (objmodels.ScheduleName == "" || objmodels.ScheduleName == null || objmodels.ScheduleName == "All")
             {
                 objmodels.ScheduleName = "%";

             }
             if (objmodels.AccountName == "" || objmodels.AccountName == null || objmodels.AccountName == "All")
             {
                 objmodels.AccountName = "%";

             }

             if (objmodels.VoucherName == "" || objmodels.VoucherName == null || objmodels.VoucherName == "All")
             {
                 objmodels.VoucherName = "%";

             }

             if (objmodels.TransactionType == "" || objmodels.TransactionType == null || objmodels.TransactionType == "All")
             {
                 objmodels.TransactionType = "%";

             }

             if (objmodels.ScheduleID == "" || objmodels.ScheduleID == null || objmodels.ScheduleID == "All")
             {
                 objmodels.ScheduleID = "%";

             }

             if (objmodels.AccountID == "" || objmodels.AccountID == null || objmodels.AccountID == "All")
             {
                 objmodels.AccountID = "%";

             }

             DateFrom = objmodels.DateFrom;
             DateTo = objmodels.DateTo;

             objmodels.dsPatientReport = objbl.MISVoucherEntryWiseReport(objmodels);

             return View(objmodels);
             
         }

         public JsonResult BindScheduleName(string prefix)
         {
             DataSet ds = objbl.BindSchedule(prefix);
             List<MISVoucherEntryWiseReport> searchList = new List<MISVoucherEntryWiseReport>();
             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 searchList.Add(new MISVoucherEntryWiseReport
                 {
                     ScheduleID = dr["ScheduleID"].ToString(),
                     ScheduleName = dr["ScheduleName"].ToString(),
                 });
             }
             return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }

         public JsonResult BindAccountName(string prefix)
         {
             DataSet ds = objbl.BindAccount(prefix);
             List<MISVoucherEntryWiseReport> searchList = new List<MISVoucherEntryWiseReport>();
             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 searchList.Add(new MISVoucherEntryWiseReport
                 {
                     AccountID = dr["AccountsID"].ToString(),
                     AccountName = dr["AccountName"].ToString(),
                 });
             }
             return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }


         public JsonResult BindVoucherName(string prefix)
         {
             DataSet ds = objbl.BindVoucherName(prefix);
             List<MISVoucherEntryWiseReport> searchList = new List<MISVoucherEntryWiseReport>();
             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 searchList.Add(new MISVoucherEntryWiseReport
                 {
                     VoucherID = dr["VoucherTypeID"].ToString(),
                     VoucherName = dr["VoucherTypeName"].ToString(),
                 });
             }
             return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
	}
}