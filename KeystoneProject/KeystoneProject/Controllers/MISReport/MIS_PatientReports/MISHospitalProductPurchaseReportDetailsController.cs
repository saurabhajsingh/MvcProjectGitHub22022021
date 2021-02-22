using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;

using System.Data;
using System.IO;

namespace KeystoneProject.Controllers.MISReport.MIS_PatientReports
{
    public class MISHospitalProductPurchaseReportDetailsController : Controller
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
        BL_MISHospitalProductPurchaseReportDetails objPur = new BL_MISHospitalProductPurchaseReportDetails();
        MISHospitalProductPurchaseReportDetails objMod = new MISHospitalProductPurchaseReportDetails();
        //
        // GET: /MISHospitalProductPurchaseReport/
        public ActionResult MISHospitalProductPurchaseReportDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MISHospitalProductPurchaseReportDetails(MISHospitalProductPurchaseReportDetails obj)
        {
            return RedirectToAction("MISHospitalProductPurchaseReport", "MISHospitalProductPurchaseReport");
        }

        public JsonResult BindSupplierName(string prefix)
        {
            //DataSet ds = objPur.BindSupplierName(prefix);
            List<MISHospitalProductPurchaseReportDetails> searchList = new List<MISHospitalProductPurchaseReportDetails>();

            return new JsonResult { Data = objPur.BindSupplierName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ReportMISHospitalProductPurchaseReport(DateTime DateFrom, DateTime DateTo, string SupplierID, string InvoiceNo, string Billno)
        {
            var bill = "";

            var invoice = "";
            var supply = "";
            if (Billno == null)
            {
                bill = "";
            }
            else
            {
                bill = Billno;
            }
            if (InvoiceNo=="0")
            {
                invoice = "";
            }
            else
            {
                invoice = InvoiceNo;
            }
            if (SupplierID == "0")
            {
                supply = "%";
            }
            else
            {
                supply = SupplierID;
            }
            var PaidAmt = "";
            var BillAmt = "";
            DataSet ds = new DataSet();
            List<MISHospitalProductPurchaseReportDetails> searchlist = new List<MISHospitalProductPurchaseReportDetails>();
            List<MISHospitalProductPurchaseReportDetails> data = new List<MISHospitalProductPurchaseReportDetails>();

            ds = objPur.ReportMISHospitalProductPurchaseReport(DateFrom, DateTo, supply, invoice, bill);

            if (ds.Tables[0].Rows.Count > 0)
            {
                PaidAmt = Convert.ToString(ds.Tables[0].Compute("Sum(PaidAmount)", String.Empty).ToString());
                BillAmt = Convert.ToString(ds.Tables[0].Compute("Sum(NetAmount)", String.Empty).ToString());

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    searchlist.Add(new MISHospitalProductPurchaseReportDetails
                    {
                        SupplierName = dr["SupplierName"].ToString(),
                        BillDate = Convert.ToDateTime(dr["Bill Date"]).ToString("dd-MM-yyyy"),
                        BillNo = dr["Bill No"].ToString(),
                        InvoiceNo = dr["InvoiceNumber"].ToString(),
                        NetAmount = dr["NetAmount"].ToString(),
                        PaidAmount = dr["PaidAmount"].ToString(),
                        PaymentType = dr["PaymentType"].ToString(),
                        TotalPaid = PaidAmt,
                        TotalNet = BillAmt

                    });
                }
            }
                if(ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        data.Add(new MISHospitalProductPurchaseReportDetails
                        {
                            SupplierName = dr["SupplierName"].ToString(),
                            BillDate = Convert.ToDateTime(dr["Bill Date"]).ToString("dd-MM-yyyy"),
                            BillNo = dr["Bill No"].ToString(),
                            InvoiceNo = dr["InvoiceNumber"].ToString(),
                            ItemName = dr["ItemName"].ToString(),
                            Rate = dr["Rate"].ToString(),
                            Quantity = dr["Quantity"].ToString(),
                            TotalAmount = dr["TotalAmount"].ToString()
                            //BillNo = Convert.ToInt32(dr["BillNo"])

                        });
                    }
                }





                return Json(new { searchlist = searchlist, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RptMISHospitalProductPurchaseReportDetails(string Type)
        {
            Session["Type"] = Type;
            return View();
        }
	}
}