using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.PatientReport;
using KeystoneProject.Models.MISReport.PatientReport;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KeystoneProject.Controllers.PatientReport
{
    public class MISReportMonthlyConsultancyChargesController : Controller
    {
        //
        // GET: /MISReportMonthyConsultancyCharges/
        public ActionResult MISReportMonthlyConsultancyCharges()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MISReportMonthlyConsultancyCharges(MISReportMonthlyConsultancyCharges obj,FormCollection fc)
        {
            return View();
        }

        public JsonResult GetReportMISMonthlyConsultancyCharges(DateTime FromDate, DateTime ToDate, string PatientType,string DoctorID)
        {


            DataSet dsPatientBillNo = new DataSet();
            List<MISReportMonthlyConsultancyCharges> searchlist = new List<MISReportMonthlyConsultancyCharges>();

            BL_Report objBL = new BL_Report();
            var Cat = "";


            if (DoctorID == "0")
            {
                Cat = "%";
            }
            else
            {
                Cat = Convert.ToInt32(DoctorID).ToString();
            }
            dsPatientBillNo = objBL.GetReportMISMonthlyConsultancyCharges(FromDate, ToDate, PatientType,Cat);
            var PaidAmt = Convert.ToString(dsPatientBillNo.Tables[0].Compute("Sum(PaidAmount)", String.Empty));

            var BillAmt = Convert.ToString(dsPatientBillNo.Tables[0].Compute("Sum(BillAmount)", String.Empty));
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new MISReportMonthlyConsultancyCharges
                    {
                        Regno = dr["RegNo"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                        DoctorPrintName = dr["DoctorPrintName"].ToString(),
                        BillDate = dr["BillDate"].ToString(),
                        BillAmount = dr["BillAmount"].ToString(),
                        ConsAmt = dr["ConsAmt"].ToString(),
                        Discount = dr["Discount"].ToString(),
                        PaidAmount = dr["PaidAmount"].ToString(),
                       PaidAmt=PaidAmt,
            BillAmt=BillAmt
                    });
                }
               


            }
            Session["ReportMISMonthlyConsultancyCharges"] = dsPatientBillNo;
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllConsultantDr(string prefix)
        {
            BL_Report obj = new BL_Report();
            List<MISReportMonthlyConsultancyCharges> searchlist = new List<MISReportMonthlyConsultancyCharges>();
            DataSet ds = obj.GetAllConsultantDr();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISReportMonthlyConsultancyCharges
                {
                    DoctorID = dr["DoctorID"].ToString(),
                    DoctorName = dr["DoctorName"].ToString()
                });
            }


            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult RptReportMISMonthlyConsultancyCharges()
        {
            return View();
        }

	}
}