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
    public class MISGovernmentRecordReportController : Controller
    {
        //
        // GET: /MISGovernmentRecordReport/
        public ActionResult MISGovernmentRecordReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MISGovernmentRecordReport(MISGovernmentRecordReport obj,FormCollection fc)
        {
            return View();
        }

        public JsonResult GetAllPerformedby(string prefix)
        {
            BL_Report obj = new BL_Report();
            List<MISGovernmentRecordReport> searchlist = new List<MISGovernmentRecordReport>();
            DataSet ds = obj.GetAllPerformedby(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISGovernmentRecordReport
                {
                    DoctorID = dr["DoctorID"].ToString(),
                    DoctorPrintName = dr["DoctorPrintName"].ToString()
                });
            }


            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult GetAllOpinionBy(string prefix)
        {
            BL_Report obj = new BL_Report();
            List<MISGovernmentRecordReport> searchlist = new List<MISGovernmentRecordReport>();
            DataSet ds = obj.GetAllOpinionBy(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISGovernmentRecordReport
                {
                    DoctorID = dr["DoctorID"].ToString(),
                    DoctorPrintName = dr["DoctorPrintName"].ToString()
                });
            }


            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult GetAllProblem(string prefix)
        {
            BL_Report obj = new BL_Report();
            List<MISGovernmentRecordReport> searchlist = new List<MISGovernmentRecordReport>();
            DataSet ds = obj.GetAllProblem();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISGovernmentRecordReport
                {
                    ProblemID = dr["ProblemID"].ToString(),
                    Problem = dr["Problem"].ToString()
                });
            }


            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult GetReportRptMISGovernmentRecords(DateTime FromDate, DateTime ToDate, string PatientType, string ProblemID, string OpinionID, string PerformedID)
        {


            DataSet dsPatientBillNo = new DataSet();
            List<MISGovernmentRecordReport> searchlist = new List<MISGovernmentRecordReport>();

            BL_Report objBL = new BL_Report();
            var type="";
            var Opn = "";
            var Pro = "";
            var Per = "";
            if (PatientType == "All")
            {
                type = "%";
            }
            else
            {
                type = Convert.ToInt32(PatientType).ToString();
            }
            if (ProblemID == "0")
            {
                Pro = "%";
            }
            else
            {
                Pro = Convert.ToInt32(ProblemID).ToString();
            }
            if (PerformedID == "0")
            {
                Per = "%";
            }
            else
            {
                Per = Convert.ToInt32(PerformedID).ToString();
            }
            if (OpinionID == "0")
            {
                Opn = "%";
            }
            else
            {
                Opn = Convert.ToInt32(OpinionID).ToString();
            }
            dsPatientBillNo = objBL.RptMISGovernmentRecords(FromDate, ToDate, type, Pro, Opn, Per);
            
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new MISGovernmentRecordReport
                    {
                        PatientRegNo = dr["PatientRegNo"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        GuardianName = dr["GuardianName"].ToString(),
                        Problem = dr["Problem"].ToString(),
                        OPDIPDID = dr["OPDIPDID"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        Age = dr["Age"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                        MaritalStatus = dr["MaritalStatus"].ToString(),
                        ReligionName = dr["ReligionName"].ToString(),
                        IndicationOfTermination = dr["IndicationOfTermination"].ToString(),
                        Duration = dr["Duration"].ToString(),
                        Remark = dr["Remark"].ToString(),
                        OpinionBy = dr["OpinionBy"].ToString(),
                        PerformedBy = dr["PerformedBy"].ToString(),
                        AddmissionDate = dr["AddmissionDate"].ToString(),
                        DischargeDate = dr["DischargeDate"].ToString(),
                        Address = dr["Address"].ToString(),
                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

	}
}