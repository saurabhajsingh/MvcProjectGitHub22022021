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
    public class PatientOPDNoReportController : Controller
    {
        BL_PatientOPDNoReport objbl = new BL_PatientOPDNoReport();
        PatientOPDNoReport objmodel = new PatientOPDNoReport();

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
            List<PatientOPDNoReport> searchList = new List<PatientOPDNoReport>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new PatientOPDNoReport
                {
                    PatientName = dr["PatientName"].ToString(),
                    PatientRegNo = dr["PatientRegNo"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult PatientOPDNoReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientOPDNoReport(PatientOPDNoReport obj)
        {
            return View();
        }
        public JsonResult GetReportMISPatientOPDNoWiseReport(string PatientRegNo, string FromPatientOPDNO, string ToPatientOPDNO)
        {

            if (PatientRegNo == "")
            {
                PatientRegNo = "%";
            }
            DataSet dsPatientBillNo = new DataSet();
            List<PatientOPDNoReport> searchlist = new List<PatientOPDNoReport>();

            BL_PatientOPDNoReport objBL = new BL_PatientOPDNoReport();
            dsPatientBillNo = objBL.GetReportMISPatientOPDNoWiseReport(PatientRegNo, FromPatientOPDNO, ToPatientOPDNO);
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new PatientOPDNoReport
                    {

                        PatientRegNo = dr["PatientRegNo"].ToString(),
                        PatientOPDNO = dr["PatientOPDNO"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        Address = dr["Address"].ToString(),
                        MobileNo = dr["MobileNo"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                        PatientRegistrationDate = dr["PatientRegistrationDate"].ToString(),
                        ConsultantDr = dr["ConsultantDr"].ToString(),
                        ReferredDr = dr["ReferredDr"].ToString(),
                        TPAName = dr["TPAName"].ToString(),


                        FromOPDNo = dsPatientBillNo.Tables[1].Rows[0]["FromOPDNO"].ToString(),
                        ToOPDNo = dsPatientBillNo.Tables[1].Rows[0]["ToOPDNO"].ToString(),


                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}