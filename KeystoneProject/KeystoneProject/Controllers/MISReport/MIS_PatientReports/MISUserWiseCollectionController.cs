using KeystoneProject.Buisness_Logic.MISReport;
using KeystoneProject.Models.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Report
{
    public class MISUserWiseCollectionController : Controller
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
        public ActionResult MISUserWiseCollection()
        {
            return View();
        }

        MISUserWiseCollection objUserWiseModel=new MISUserWiseCollection();
        BL_MISUserWiseCollection BL_MISUserWiseCollection =new BL_MISUserWiseCollection();


        public ActionResult MISUserWiseCollection(string DateFrom, string DateTo, string UserID)
        {


            BL_MISUserWiseCollection BlReport1 = new BL_MISUserWiseCollection();
            HospitalLocation();


            if(UserID=="")
            {
                UserID = "%";
            }
           
            return new JsonResult { Data = BlReport1.MISUserWiseCollection( HospitalID,LocationID,Convert.ToDateTime( DateFrom),Convert.ToDateTime( DateTo), UserID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult MISUserWiseCollectionPaticular(string DateFrom, string DateTo, string UserID,string PaymentType)
        {


            BL_MISUserWiseCollection BlReport1 = new BL_MISUserWiseCollection();
            HospitalLocation();

           
            if (UserID == "")
            {
                UserID = "%";
            }
            if (PaymentType == "" && PaymentType=="All")
            {
                PaymentType = "%";
            }
           

            return new JsonResult { Data = BlReport1.MISUserWiseCollectionPaticular(HospitalID, LocationID, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), UserID,PaymentType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetUserNameValue(string prefix)
        {
            BL_MISUserWiseCollection BlReport = new BL_MISUserWiseCollection();

            return new JsonResult { Data = BlReport.GetUserName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetClick(string ServiceID)
        {
            BL_MISUserWiseCollection BL_obj = new BL_MISUserWiseCollection();
            MISUserWiseCollection obj=new MISUserWiseCollection();
         

            obj.UserID = Session["UserID"].ToString();
            obj.DateFrom = Convert.ToDateTime(Session["FromDate"]);
            obj.DateTo = Convert.ToDateTime(Session["ToDate"]);
            //obj.PatientType = Session["PatientType"].ToString();

            // HospitlLocationID();

            //DataSet ds = BL_obj.MISPatientIPDDischargeSummary(HospitalID, LocationID, obj.FromDate, obj.ToDate, obj.PatientType, obj.SearchName, obj.EndResult);
            List<MISUserWiseCollection> searchList = new List<MISUserWiseCollection>();

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}