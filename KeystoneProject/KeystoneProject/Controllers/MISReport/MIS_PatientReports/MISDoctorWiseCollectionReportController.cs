using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Report;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.MISReport;
using System.Data;

namespace KeystoneProject.Controllers.Report
{
    public class MISDoctorWiseCollectionReportController : Controller
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

        BL_MISDoctorWiseCollectionReport objbl = new BL_MISDoctorWiseCollectionReport();
        MISDoctorWiseCollectionReport objmodels = new MISDoctorWiseCollectionReport();

        [HttpGet]
        public ActionResult MISDoctorWiseCollectionReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MISDoctorWiseCollectionReport(FormCollection collection)
        {

            objmodels.DateFrom = Convert.ToDateTime(collection["Fromdate"]);
            objmodels.DateTo = Convert.ToDateTime(collection["Todate"]);
            objmodels.DoctorType = collection["doctor_type"];
            objmodels.DoctorID = collection["DoctorID"];
            objmodels.PatientType = collection["patient_type"];

            Session["DateFrom"] = objmodels.DateFrom;
            Session["DateTo"] = objmodels.DateTo;
            

            if (objmodels.DoctorType == "" || objmodels.DoctorType == null || objmodels.DoctorType == "All")
            {
                objmodels.DoctorType = "%";
            }


            if (objmodels.DoctorID == "" || objmodels.DoctorID == null || objmodels.DoctorID == "All")
            {
                objmodels.DoctorID = "%";

            }

            if (objmodels.PatientType == "" || objmodels.PatientType == null || objmodels.PatientType == "All")
            {
                objmodels.PatientType = "%";
            }


            //objbl.ReportMISDoctorWiseCollection(1, 1, objDoctor.DateFrom, objDoctor.DateTo, DoctorType, DoctorID, PatientType);   
            objmodels.dsDoctor = objbl.ReportMISDoctorWiseCollection(objmodels);

            return View(objmodels);

        }


        public JsonResult BindDoctor(string prefix, string Type)
        {
            List<MISDoctorWiseCollectionReport> searchList = new List<MISDoctorWiseCollectionReport>();
            MISDoctorWiseCollectionReport doctor = new MISDoctorWiseCollectionReport();

            if (Type == "Consultant")
            {
                DataSet ds = objbl.DoctorType();

                DataView dv = new DataView(ds.Tables[0], "Doctorname like '" + prefix + "%" + "'", "", DataViewRowState.CurrentRows);

                foreach (DataRow dr in dv.ToTable().Rows)
                {
                    searchList.Add(new MISDoctorWiseCollectionReport
                    {

                        DoctorID = dr["DoctorID"].ToString(),
                        DoctorName = dr["Doctorname"].ToString()

                    });
                }

            }

            else
            {
                if (Type == "Referred")
                {

                    DataSet ds = objbl.DoctorType();
                    DataView dv = new DataView(ds.Tables[1], "Doctorname like'" + prefix + "%" + "'", "", DataViewRowState.CurrentRows);

                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        searchList.Add(new MISDoctorWiseCollectionReport
                        {

                            DoctorID = dr["DoctorID"].ToString(),
                            DoctorName = dr["Doctorname"].ToString()


                        });
                    }
                }


            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult detDobleClick(string DoctorID, string doctor_type,string PatientType)
        {
            List<MISDoctorWiseCollectionReport> searchList = new List<MISDoctorWiseCollectionReport>();

            if (PatientType == "All")
            {
                PatientType = "%";
            }
            objmodels.DoctorID = DoctorID;
            objmodels.DoctorType = doctor_type;
            objmodels.PatientType = PatientType;
            objmodels.DateFrom = Convert.ToDateTime(Session["DateFrom"]);
            objmodels.DateTo = Convert.ToDateTime(Session["DateTo"]);
            Session["DoctorID"] = DoctorID;
            Session["DoctorType"] = doctor_type;
            Session["PatientType"] = PatientType;
            DataSet ds = objbl.ReportMISDoctorWiseCollectionForParticular(objmodels);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new MISDoctorWiseCollectionReport
                {
                    
                   DoctorID = dr["PatientName"].ToString(),
                    BillNo = dr["BillNo"].ToString(),
                    Username = dr["UserName"].ToString(),
                    TotalAmount = dr["TotalAmount"].ToString(),

                    TotalAmount1 = Convert.ToString(ds.Tables[0].Compute("sum(TotalAmount)", string.Empty).ToString()),

                });

            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public DataSet GetMisDoctorWiseCollectionReport()
        {
            objmodels.DoctorID = Session["DoctorID"].ToString();
            objmodels.DoctorType = Session["DoctorType"].ToString();
            objmodels.DateFrom = Convert.ToDateTime(Session["DateFrom"]);
            objmodels.DateTo = Convert.ToDateTime(Session["DateTo"]);
            objmodels.PatientType = Session["PatientType"].ToString();
            DataSet ds = objbl.ReportMISDoctorWiseCollectionForParticular(objmodels);

            return ds;
        }
        public ActionResult RptMISDoctorWiseCollection()
        {

            return new JsonResult { Data = "Report", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult RptMISDoctorWiseCollectionReport()
        {

            return View();

        }
	}
}