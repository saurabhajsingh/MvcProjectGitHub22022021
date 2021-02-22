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
    public class ReportLabTestByNameController : Controller
    {
        //
        // GET: /ReportLabTestByName/
        public ActionResult ReportLabTestByName()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportLabTestByName(ReportLabTestByName obj)
        {
            return View();
        }

        public JsonResult GetReportLabTestWithDetails(DateTime FromDate, DateTime ToDate, string PatientType, string TestID)
        {
            var id = "";

            DataSet dsPatientBillNo = new DataSet();
            List<ReportLabTestByName> searchlist = new List<ReportLabTestByName>();

            BL_ReportLabTestByName objBL = new BL_ReportLabTestByName();
            if (TestID == "0")
            {
                id = "%";
            }
            else
            {
                id = Convert.ToInt32(TestID).ToString();
            }
            dsPatientBillNo = objBL.ReportLabTestByName(FromDate, ToDate, PatientType, id);
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new ReportLabTestByName
                    {
                        IPDNO = dr["IPDNO"].ToString(),
                        Regno = dr["RegNo"].ToString(),
                        TestName = dr["TestName"].ToString(),
                        Qty = dr["Qty"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                        Patient = dr["Patient"].ToString(),
                        ContactNo = dr["ContactNo"].ToString(),
                      //  OPDIPDID = dr["OPDIPDID"].ToString()
                        

                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetReportTestForPatientLabTestDetails(string prefix)
        {


            DataSet dsPatientBillNo = new DataSet();
            List<ReportLabTestByName> searchlist = new List<ReportLabTestByName>();

            BL_ReportLabTestByName objBL = new BL_ReportLabTestByName();
            dsPatientBillNo = objBL.GetAllTestMasterForProfile();
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new ReportLabTestByName
                    {
                        TestName = dr["TestName"].ToString(),
                      TestID=dr["TestID"].ToString()

                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}