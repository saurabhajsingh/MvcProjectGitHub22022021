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
    public class ReportLabTestController : Controller
    {
        //
        // GET: /ReportLabTest/
        public ActionResult ReportLabTest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportLabTest(ReportLabTest obj)
        {
            return View();
        }


        public JsonResult GetReportLabTestDetails(DateTime FromDate, DateTime ToDate, string PatientType)
        {
           
           
            DataSet dsPatientBillNo = new DataSet();
            List<ReportLabTest> searchlist = new List<ReportLabTest>();

            BL_ReportLabTest objBL = new BL_ReportLabTest();
            dsPatientBillNo = objBL.GetReportLabTest(FromDate, ToDate, PatientType);
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new ReportLabTest
                    {
                        BillNo=dr["BillNo"].ToString(),
                        LabNo=dr["BillNo"].ToString(),
                        PatientType=dr["PatientType"].ToString(),
                        Patient=dr["Patient"].ToString(),
                        ContactNo=dr["ContactNo"].ToString(),
                        Address=dr["Address"].ToString(),
                        DoctorName=dr["DoctorName"].ToString()
                        //BillNoDate = dr["BillNo&Date"].ToString(),
                        //BillNo = Convert.ToInt32(dr["BillNo"])

                    });
                }

               
                
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

	}
}