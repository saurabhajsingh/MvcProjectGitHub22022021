using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;

namespace KeystoneProject.Controllers.MISReport.MIS_PatientReports
{
    public class MISWardWiseDetailsController : Controller
    {
        MISWardWiseDetails objWiseModel = new MISWardWiseDetails();
        BL_MISWardWiseDetails objWard = new BL_MISWardWiseDetails();
        //
        // GET: /MISWardWiseDetails/
        public ActionResult MISWardWiseDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MISWardWiseDetails(MISWardWiseDetails obj)
        {

            return View();
        }

        public JsonResult GetRptMISWardWiseDetails(DateTime FromDate, DateTime ToDate, string PatientType)
        {

            DataSet ds = new DataSet();

            List<MISWardWiseDetails> searchlist = new List<MISWardWiseDetails>();

            ds = objWard.GetRptMISWardWiseDetails(FromDate, ToDate, PatientType);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    searchlist.Add(
                        new MISWardWiseDetails
                        {
                            RegNo = Convert.ToString(dr["RegNo"]),
                            IPDNo = Convert.ToString(dr["IPDNo"]),
                            PatientName = Convert.ToString(dr["PatientName"]),
                            Age = Convert.ToString(dr["Age"]),
                            Sex = Convert.ToString(dr["Sex"]),
                            AddmissionDate = Convert.ToString(dr["AddmissionDate"]),
                            DischargeDate = Convert.ToString(dr["DischargeDate"]),
                            Days = Convert.ToString(dr["Days"]),
                            WardName = Convert.ToString(dr["WardName"]),
                            ConsultantDr = Convert.ToString(dr["ConsultantDr"]),
                            RefferedDr = Convert.ToString(dr["ReferredDr"]),
                            PatientStatus = Convert.ToString(dr["PatientStatus"]),


                        });

                }

               



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult ReportRptMISWardWiseDetails()
        {
            return View();
        }

	}
}