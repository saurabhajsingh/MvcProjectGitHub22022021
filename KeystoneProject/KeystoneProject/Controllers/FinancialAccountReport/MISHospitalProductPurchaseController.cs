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
    public class MISHospitalProductPurchaseController : Controller
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

        BL_MISHospitalProductPurchase objbl = new BL_MISHospitalProductPurchase();
        MISHospitalProductPurchase objmodels = new MISHospitalProductPurchase();

        public static Nullable<DateTime> DateFrom { get; set; }
        public static Nullable<DateTime> DateTo { get; set; }

        [HttpGet]
        public ActionResult MISHospitalProductPurchase()
        {
            return View();
        }

        [HttpPost]

        public ActionResult MISHospitalProductPurchase(FormCollection collection)
        {

            objmodels.DateFrom = Convert.ToDateTime(collection["DateFrom"]);
            objmodels.DateTo = Convert.ToDateTime(collection["DateTo"]);

            //string[] SupplierID = objmodels.SupplierID.Split(',').Select(sValue => sValue.Trim()).ToArray();
            objmodels.SupplierID = collection["SupplierID"];
            objmodels.ProductID = collection["ProductID"];
            objmodels.SupplierName = collection["SupplierName"];
            objmodels.ProductName = collection["ProductName"];


            if (objmodels.SupplierID == "" || objmodels.SupplierID == null || objmodels.SupplierID == "All" || objmodels.SupplierID ==",")
            {
                objmodels.SupplierID = "%";

            }
            if (objmodels.ProductID == "" || objmodels.ProductID == null || objmodels.ProductID == "All")
            {
                objmodels.ProductID = "%";

            }

            if (objmodels.SupplierName == "" || objmodels.SupplierName == null || objmodels.SupplierName == "All" || objmodels.SupplierName == ",")
            {
                objmodels.SupplierName = "%";

            }

            if (objmodels.ProductName == "" || objmodels.ProductName == null || objmodels.ProductName == "All")
            {
                objmodels.ProductName = "%";

            }

            DateFrom = objmodels.DateFrom;
            DateTo = objmodels.DateTo;

            objmodels.dsPatientReport = objbl.MISHospitalEntryWiseReport(objmodels);

            return View(objmodels);
            
        }



        public JsonResult BindSupplierName(string prefix)
        {
            DataSet ds = objbl.BindSupplierName(prefix);
            List<MISHospitalProductPurchase> searchList = new List<MISHospitalProductPurchase>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new MISHospitalProductPurchase
                {
                    SupplierID = dr["AccountsID"].ToString(),
                    SupplierName = dr["AccountName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult BindProduct(string prefix)
        {
            DataSet ds = objbl.BindProduct(prefix);
            List<MISHospitalProductPurchase> searchList = new List<MISHospitalProductPurchase>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new MISHospitalProductPurchase
                {
                    ProductID = dr["HospitalProductID"].ToString(),
                    ProductName = dr["HospitalProductName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       
	}
}