using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Report;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using KeystoneProject.Buisness_Logic.MISReport;
using KeystoneProject.Models.Patient;

namespace KeystoneProject.Controllers.Report
{
    public class MISServiceWiseCollectionReportController : Controller
    {
        int   HospitalID;
        int   LocationID;
        int   UserID;
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID     = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        [HttpGet]
        public ActionResult MISServiceWiseCollectionReport()
        {
            MISServiceWiseCollectionReport obj = new MISServiceWiseCollectionReport();
            return View();
        }

        BL_MISServiceWiseCollectionReport BlReport = new BL_MISServiceWiseCollectionReport();

        [HttpPost]
        public ActionResult MISServiceWiseCollectionReport(MISServiceWiseCollectionReport Model_obj)
        {
            BL_MISServiceWiseCollectionReport BlReport1 = new BL_MISServiceWiseCollectionReport();

            Model_obj.FromDate = Convert.ToDateTime(Request.Form["Fromdate"]);
            Model_obj.ToDate = Convert.ToDateTime(Request.Form["Todate"]);
            Model_obj.PatientType = Request.Form["patient_type"];
            Model_obj.ServiceGroupName = Request.Form["ServiceGroupID"];
            Model_obj.ServiceName = Request.Form["ServiceID"];
            Model_obj.PatientRegNo = Request.Form["PatientRegNo"];

            if (Model_obj.PatientType == "")
            {
                Model_obj.PatientType = "All";
            }
            if (Model_obj.ServiceGroupName == "")
            {
                Model_obj.ServiceGroupName = "%";
            }
            if (Model_obj.ServiceName == "")
            {
                Model_obj.ServiceName = "%";
            }

            HospitlLocationID();
            Session["ServiceGroupName"] = Model_obj.ServiceGroupName;
            Session["Fromdate"] = Model_obj.FromDate;
            Session["Todate"] = Model_obj.ToDate;
            Model_obj.ds = BlReport1.ReportMISServicesWiseCollection(HospitalID, LocationID, Model_obj.FromDate, Model_obj.ToDate, Model_obj.ServiceName, Model_obj.ServiceGroupName, Model_obj.PatientType,Model_obj.PatientRegNo);

            Session["ServiceID"] = Model_obj.ServiceName;
            Session["ServiesGroupID"] = Model_obj.ServiceGroupName;
            if (Session["PatientType"] == "")
            {
                Session["PatientType"] = "All";
            }
            else
            {
                Session["PatientType"] = Model_obj.PatientType;
            }

            foreach (DataRow dr in Model_obj.ds.Tables[0].Rows)
            {
               
                if (Model_obj.PatientType == "UnDischarge")
                {

                }
            }

            return View(Model_obj);
        }

        public JsonResult GetQuestionHeadRecord(string prefix)
        {


            DataSet dsQuestionhead = new DataSet();
            dsQuestionhead = BlReport.BindServiceGroupName(prefix);

            List<MISServiceWiseCollectionReport> serch = new List<MISServiceWiseCollectionReport>();

            DataView dv = new DataView(dsQuestionhead.Tables[0], "ServiceGroupName like '" + prefix + "%" + "'", "", DataViewRowState.CurrentRows);
            foreach (DataRow dr in dv.ToTable().Rows)
            {
                serch.Add(new MISServiceWiseCollectionReport
                {
                    Name = dr["ServiceGroupName"].ToString(),
                    ID = dr["ServiceGroupID"].ToString()
                    //  DoctorPrintName = dr["DoctorPrintName"].ToString()
                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetQuestionRecord(string prefix)
        {


            DataSet dsQuestionhead = new DataSet();
            dsQuestionhead = BlReport.BindSeviceName(prefix);

            List<MISServiceWiseCollectionReport> serch = new List<MISServiceWiseCollectionReport>();

            DataView dv = new DataView(dsQuestionhead.Tables[0], "ServiceName like '" + prefix + "%" + "'", "", DataViewRowState.CurrentRows);

            foreach (DataRow dr in dv.ToTable().Rows)
            {
                serch.Add(new MISServiceWiseCollectionReport
                {
                    Name = dr["ServiceName"].ToString(),
                    ID = dr["ServiceID"].ToString()
                    
                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPatientNameValue(string prefix)
        {
            BL_MISServiceWiseCollectionReport BlReport = new BL_MISServiceWiseCollectionReport();

            return new JsonResult { Data = BlReport.GetPatientName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetClick(string ServiceID)
        {
            BL_MISServiceWiseCollectionReport BL_obj = new BL_MISServiceWiseCollectionReport();

            MISServiceWiseCollectionReport obj = new MISServiceWiseCollectionReport();

            obj.ServiceGroupName = Session["ServiceGroupName"].ToString();

            obj.FromDate = Convert.ToDateTime(Session["FromDate"]);
            obj.ToDate = Convert.ToDateTime(Session["ToDate"]);
            //obj.PatientType = Session["PatientType"].ToString();

            HospitlLocationID();

            DataSet ds = BL_obj.ReportMISServicesWiseCollection(HospitalID, LocationID, obj.FromDate, obj.ToDate, ServiceID, obj.ServiceGroupName, "","");
            List<MISServiceWiseCollectionReport> searchList = new List<MISServiceWiseCollectionReport>();

           

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                searchList.Add(new MISServiceWiseCollectionReport
                {
                    PatientRegNo = dr["PatientRegNo"].ToString(),
                    PatientName = dr["PatientName"].ToString(),

                    ServiceName = dr["ServiceName"].ToString(),
                    Qty = dr["QTY"].ToString(),
                    TotalAmount = dr["Total Amount"].ToString(),

                    TotalAmt1 = Convert.ToString(ds.Tables[2].Compute("sum([Total Amount])", string.Empty).ToString())
                    //  DoctorPrintName = dr["DoctorPrintName"].ToString()
                });
            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult RptServicepatientwise()
        {

            return View();
        }

        public ActionResult RptMISServicesWiseCollectionNew()
        {

            return View();
        }


       
    }
}