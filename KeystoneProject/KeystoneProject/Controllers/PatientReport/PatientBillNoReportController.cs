using KeystoneProject.Buisness_Logic.PatientReport;
using KeystoneProject.Models.PatientReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.PatientReport
{
    public class PatientBillNoReportController : Controller
    {

        BL_PatientBillNoReport objbl = new BL_PatientBillNoReport();
        PatientBillNoReport objmodel = new PatientBillNoReport();

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }



        public JsonResult BindPatientName(string prefix)
        {
            DataSet ds = objbl.BindPatientDetails(prefix);
            List<PatientBillNoReport> searchList = new List<PatientBillNoReport>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new PatientBillNoReport
                {
                    PatientName = dr["PatientName"].ToString(),
                    PatientRegNo = dr["PatientRegNo"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
       
        //
        // GET: /PatientBillNoReport/
        public ActionResult PatientBillNoReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientBillNoReport(PatientBillNoReport obj)
        {
            return View();
        }

        public JsonResult GetReportMISPatientBillNoWiseReport(string PatientRegNo, string FromBillNo, string ToBillNo)
        {

            if (PatientRegNo == "")
            {
                PatientRegNo = "%";
            }
            DataSet dsPatientBillNo = new DataSet();
            List<PatientBillNoReport> searchlist = new List<PatientBillNoReport>();

            BL_PatientBillNoReport objBL = new BL_PatientBillNoReport();
            dsPatientBillNo = objBL.GetReportMISPatientBillNoWiseReport(PatientRegNo, FromBillNo, ToBillNo);
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new PatientBillNoReport
                    {
                        BillNo = dr["BillNo"].ToString(),
                        PatientRegNo = dr["RegNo"].ToString(),
                        OPDIPDNo = dr["OPDIPDNo"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        BillType = dr["BillType"].ToString(),
                        BillDate = dr["BillDate"].ToString(),
                        GrossAmount = dr["GrossAmount"].ToString(),
                        DisAmount = dr["DisAmount"].ToString(),
                        TaxAmount = dr["TaxAmount"].ToString(),
                        BillAmount = dr["BillAmount"].ToString(),
                        PaymentType = dr["PaymentType"].ToString(),
                        FinancialYear = dr["FinancialYear"].ToString(),
                       

                        FromBillNo = dsPatientBillNo.Tables[1].Rows[0]["FromBillNo"].ToString(),
                        ToBillNo = dsPatientBillNo.Tables[1].Rows[0]["ToBillNo"].ToString(),

                        BillAmount1 = Convert.ToString(dsPatientBillNo.Tables[0].Compute("sum(BillAmount)", string.Empty).ToString()),
                        GrossAmount1 = Convert.ToString(dsPatientBillNo.Tables[0].Compute("sum(GrossAmount)", string.Empty).ToString()),
                        TaxAmount1 = Convert.ToString(dsPatientBillNo.Tables[0].Compute("sum(TaxAmount)", string.Empty).ToString()),
                        DisAmount1 = Convert.ToString(dsPatientBillNo.Tables[0].Compute("sum(DisAmount)", string.Empty).ToString()),

                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}