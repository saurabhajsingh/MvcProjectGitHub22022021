using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.MISReport.LabReport;
using KeystoneProject.Buisness_Logic.MISReport.LabReport;

namespace KeystoneProject.Controllers.MISReport.LabReport
{
    public class ReportLabTestWithDetailsController : Controller
    {
        //
        // GET: /ReportLabTestWithDetails/
        public ActionResult ReportLabTestWithDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportLabTestWithDetails(ReportLabTestWithDetails obj)
        {
            return View();
        }

        public JsonResult GetReportLabTestWithDetails(DateTime FromDate, DateTime ToDate, string PatientType)
        {


            DataSet dsPatientBillNo = new DataSet();
            List<ReportLabTestWithDetails> searchlist = new List<ReportLabTestWithDetails>();

            BL_ReportLabTestWithDetails objBL = new BL_ReportLabTestWithDetails();
            dsPatientBillNo = objBL.ReportLabTestWithDetails(FromDate, ToDate, PatientType);
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new ReportLabTestWithDetails
                    {
                        BillNo = dr["BillNo"].ToString(),
                        LabNo = dr["LabNo"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                        Patient = dr["PatientDetails"].ToString(),
                        ContactNo = dr["ContactNo"].ToString(),
                        Regno = dr["RegNo"].ToString(),
                        DoctorName = dr["DoctorName"].ToString(),
                        OPDIPDID = dr["OPDIPDID"].ToString()


                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetReportTestForPatientLabTestDetails(string LabNo, string Regno, string ID)
        {


            DataSet dsPatientBillNo = new DataSet();
            List<ReportLabTestWithDetails> searchlist = new List<ReportLabTestWithDetails>();

            BL_ReportLabTestWithDetails objBL = new BL_ReportLabTestWithDetails();
            dsPatientBillNo = objBL.ReportTestForPatientLabTestDetails(LabNo, Regno, ID);
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new ReportLabTestWithDetails
                    {
                        TestName = dr["TestName"].ToString()


                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}