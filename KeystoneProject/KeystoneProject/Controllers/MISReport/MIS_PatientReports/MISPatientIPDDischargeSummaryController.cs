using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Report;
using KeystoneProject.Buisness_Logic.MISReport;
using System.Data;

namespace KeystoneProject.Controllers.Report
{
    public class MISPatientIPDDischargeSummaryController : Controller
    {
       
        int   HospitalID;
        int   LocationID;
        int   UserID;

        public void HospitalLocation()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

         [HttpGet]

        public ActionResult MISPatientIPDDischargeSummary()
        {
            MISPatientIPDDischargeSummary obj = new MISPatientIPDDischargeSummary();
            return View();
        }
         BL_MISPatientIPDDischargeSummary BL_MISPatientIPDDischargeSummary= new BL_MISPatientIPDDischargeSummary();

         [HttpPost]
         public ActionResult MISPatientIPDDischargeSummary(DateTime DateFrom, DateTime DateTo, string PatientType, string SearchName, string EndResult)
         {
   
             if(SearchName=="")
             {
                 SearchName = "%";
             }
             if (EndResult == "" || EndResult == null)
             {
                 EndResult = "%";
             }
             BL_MISPatientIPDDischargeSummary BlReport1 = new BL_MISPatientIPDDischargeSummary();
             HospitalLocation();
      
             return new JsonResult { Data = BlReport1.MISPatientIPDDischargeSummary(HospitalID,LocationID,DateFrom, DateTo, PatientType, SearchName, EndResult), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
         public JsonResult GetPatientNameValue(string prefix)
         {
             BL_MISPatientIPDDischargeSummary BlReport = new BL_MISPatientIPDDischargeSummary();

             return new JsonResult { Data = BlReport.GetPatientName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }

         public JsonResult GetClick(string ServiceID)
         {
             BL_MISPatientIPDDischargeSummary BL_obj = new BL_MISPatientIPDDischargeSummary();

             MISPatientIPDDischargeSummary obj = new MISPatientIPDDischargeSummary();

             obj.PatientType = Session["PatientType"].ToString();
             obj.SearchName = Session["SearchName"].ToString();
             obj.EndResult = Session["EndResult"].ToString();

             obj.FromDate = Convert.ToDateTime(Session["FromDate"]);
             obj.ToDate = Convert.ToDateTime(Session["ToDate"]);
             //obj.PatientType = Session["PatientType"].ToString();

            // HospitlLocationID();

             //DataSet ds = BL_obj.MISPatientIPDDischargeSummary(HospitalID, LocationID, obj.FromDate, obj.ToDate, obj.PatientType, obj.SearchName, obj.EndResult);
             List<MISPatientIPDDischargeSummary> searchList = new List<MISPatientIPDDischargeSummary>();



             //foreach (DataRow dr in ds.Tables[2].Rows)
             //{
             //    searchList.Add(new MISPatientIPDDischargeSummary
             //    {
             //        PatientRegNo = dr["PatientRegNo"].ToString(),
             //        PatientName = dr["PatientName"].ToString(),

             //        ServiceName = dr["ServiceName"].ToString(),
             //        Qty = dr["QTY"].ToString(),
             //        TotalAmount = dr["Total Amount"].ToString(),

             //        TotalAmt1 = Convert.ToString(ds.Tables[2].Compute("sum([Total Amount])", string.Empty).ToString())
             //        //  DoctorPrintName = dr["DoctorPrintName"].ToString()
             //    });
             //}

             return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
	}
}