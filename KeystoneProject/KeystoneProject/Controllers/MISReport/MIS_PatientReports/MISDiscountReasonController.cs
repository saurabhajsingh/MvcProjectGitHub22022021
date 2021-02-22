using System;
using System.Collections.Generic;

using System.Web.Mvc;
using KeystoneProject.Models.Report;
using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using KeystoneProject.Buisness_Logic.MISReport.MISPatientReport;



namespace KeystoneProject.Controllers.PatientReport
{
    public class MISDiscountReasonController : Controller
    {
        BL_MISDiscountReason obj = new BL_MISDiscountReason();
 
  
        
 

        int HospitalID;
        int LocationID;
        int UserID;

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        MISDiscountReason misdiscount = new MISDiscountReason();
        private SqlConnection con;
        string Constring = "";

        public static Nullable<DateTime> DateFrom { get; set; }
        public static Nullable<DateTime> DateTo { get; set; }
        private void Connect()
        {
            Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        [HttpGet]
        public ActionResult MISDiscountReason()
        {
            MISDiscountReason obj1 = new MISDiscountReason();
            return View();
        }

        [HttpPost]
       // public ActionResult GETDiscountReason(FormCollection collection)
        //{

        //    try
        //    {

        //        misdiscount.DateFrom = Convert.ToDateTime(collection["FromDate"]);
        //        misdiscount.DateTo = Convert.ToDateTime(collection["ToDate"]);
        //        DateFrom = misdiscount.DateFrom;
        //        DateTo = misdiscount.DateTo;

        //        misdiscount.PatientType = collection["PatientType"];

                
                
        //        misdiscount.BillType = collection["PatientType"];

        //        if (misdiscount.PatientRegNo == "" || misdiscount.PatientRegNo == null)
        //        {
        //            misdiscount.PatientRegNo = "%";
        //        }
        //        misdiscount.dsPatientReport = MisDiscountReson(misdiscount);


        //        return View(misdiscount);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View(misdiscount);

        //    }
        //}

        public JsonResult GETDiscountReson(DateTime FromDate, DateTime ToDate, string PatientType)
        {

            DataSet ds = new DataSet();

            List<MISDiscountReason> searchlist = new List<MISDiscountReason>();

            if (PatientType == "All")
            {
                PatientType = "%";
            }

            ds = obj.GETallDiscountReson(FromDate, ToDate, PatientType);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    searchlist.Add(
                        new MISDiscountReason
                        {
                            SrNo = Convert.ToString(dr["SrNo"]),
                            RegNo = Convert.ToString(dr["RegNo"]),
                            PatientName = Convert.ToString(dr["PatientName"]),
                            PatientType = Convert.ToString(dr["PatientType"]),
                            OPDIPDNo = Convert.ToString(dr["OPDIPDNo"]),
                            BillAmount = Convert.ToString(dr["BillAmount"]),
                            Discount = Convert.ToString(dr["Discount"]),
                            DiscountReason = Convert.ToString(dr["DiscountReason"]),
                            TaxAmount = Convert.ToString(dr["TaxAmount"]),
                            PaidAmount = Convert.ToString(dr["PaidAmount"]),
                            BalanceAmount = Convert.ToString(dr["BalanceAmount"]),

                           BillAmount1  = Convert.ToString(ds.Tables[0].Compute("sum(BillAmount)", string.Empty).ToString()),
                           PaidAmount1 = Convert.ToString(ds.Tables[0].Compute("sum(PaidAmount)", string.Empty).ToString()),
                           BalanceAmount1 = Convert.ToString(ds.Tables[0].Compute("sum(BalanceAmount)", string.Empty).ToString()),
                           DiscountAmount= Convert.ToString(ds.Tables[0].Compute("sum(Discount)", string.Empty).ToString()),
                        });

                }





            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult ReportMISPatientDiscountReasons()
        {
            return View();
        }

    }
}