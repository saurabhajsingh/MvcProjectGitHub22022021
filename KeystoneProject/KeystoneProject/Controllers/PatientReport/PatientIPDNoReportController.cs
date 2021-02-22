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
    public class PatientIPDNoReportController : Controller
    {

        BL_PatientIPDNoReport objbl = new BL_PatientIPDNoReport();
        PatientIPDNoReport objmodel = new PatientIPDNoReport();

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
            List<PatientIPDNoReport> searchList = new List<PatientIPDNoReport>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new PatientIPDNoReport
                {
                    PatientName = dr["PatientName"].ToString(),
                    PatientRegNo = dr["PatientRegNo"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]

        public ActionResult PatientIPDNoReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientIPDNoReport(PatientIPDNoReport Obj)
        {
            return View();
        }

        public JsonResult GetReportMISPatientIPDNoWiseReport(string PatientRegNo, string FromPatientIPDNO, string ToPatientIPDNO)
        {

            if (PatientRegNo == "")
            {
                PatientRegNo = "%";
            }
            DataSet dsPatientBillNo = new DataSet();
            List<PatientIPDNoReport> searchlist = new List<PatientIPDNoReport>();

            BL_PatientIPDNoReport objBL = new BL_PatientIPDNoReport();
            dsPatientBillNo = objBL.GetReportMISPatientIPDNoWiseReport(PatientRegNo, FromPatientIPDNO, ToPatientIPDNO);
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new PatientIPDNoReport
                    {

                        PatientRegNo = dr["PatientRegNo"].ToString(),
                        PatientIPDNO = dr["PatientIPDNO"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        Address = dr["Address"].ToString(),
                        MobileNo = dr["MobileNo"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                        AddmissionDate = dr["AddmissionDate"].ToString(),
                        ConsultantDr = dr["ConsultantDr"].ToString(),
                        ReferredDr = dr["ReferredDr"].ToString(),
                        TPAName = dr["TPAName"].ToString(),


                        FromIPDNo = dsPatientBillNo.Tables[1].Rows[0]["FromIPDNO"].ToString(),
                        ToIPDNo = dsPatientBillNo.Tables[1].Rows[0]["ToIPDNO"].ToString(),


                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}