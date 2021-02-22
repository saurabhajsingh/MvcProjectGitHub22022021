using KeystoneProject.Buisness_Logic.FinancialAccountReport;
using KeystoneProject.Models.FinancialAccountReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.FinancialAccountReport
{
    public class MISHospitalProductPurchaseReportController : Controller
    {
        //
        // GET: /MISHospitalProductPurchaseReport/

        int HospitalID;
        int LocationID;
        int UserID;
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        BL_MISHospitalProductPurchaseReport objbl = new BL_MISHospitalProductPurchaseReport();
        MISHospitalProductPurchaseReport objmodels = new MISHospitalProductPurchaseReport();
        public static Nullable<DateTime> DateFrom { get; set; }
        public static Nullable<DateTime> DateTo { get; set; }

        [HttpGet]
        
        public ActionResult MISHospitalProductPurchaseReport()
        {
            return View();
        }

        [HttpPost]

        public ActionResult MISHospitalProductPurchaseReport(FormCollection collection)
        {

            objmodels.DateFrom = Convert.ToDateTime(collection["DateFrom"]);
            objmodels.DateTo = Convert.ToDateTime(collection["DateTo"]);

           
            objmodels.SupplierID = collection["SupplierID"];
           
            objmodels.SupplierName = collection["SupplierName"];
           


            if (objmodels.SupplierID == "" || objmodels.SupplierID == null || objmodels.SupplierID == "All" || objmodels.SupplierID == ",")
            {
                objmodels.SupplierID = "%";

            }
            
            if (objmodels.SupplierName == "" || objmodels.SupplierName == null || objmodels.SupplierName == "All" || objmodels.SupplierName == ",")
            {
                objmodels.SupplierName = "%";

            }

            
           
            DateFrom = objmodels.DateFrom;
            DateTo = objmodels.DateTo;

            objmodels.dsPatientReport = objbl.MISHospitalEntryWiseReport(objmodels);

            return View(objmodels);

        }



        public JsonResult BindSupplierName(string prefix)
        {
            DataSet ds = objbl.BindSupplierName(prefix);
            List<MISHospitalProductPurchaseReport> searchList = new List<MISHospitalProductPurchaseReport>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new MISHospitalProductPurchaseReport
                {
                    SupplierID = dr["AccountsID"].ToString(),
                    SupplierName = dr["AccountName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


	}
}