using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Report;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.MISReport;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace KeystoneProject.Controllers.Report
{
    public class MISPatientWiseCollectionReportController : Controller
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

        MISPatientWiseCollectionReport misPatientWiseCollection = new MISPatientWiseCollectionReport();
                
        private SqlConnection con;
        string Constring = "";
        public static Nullable<DateTime> DateFrom { get; set; }
        public static Nullable<DateTime> DateTo { get; set; }
        private void Connect()
        {
            Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        [HttpGet]
        public ActionResult MISPatientWiseCollectionReport()
        {
             return View();
        }

        BL_MISPatientWiseCollectionReport BlReport = new BL_MISPatientWiseCollectionReport();

        [HttpPost]
        public ActionResult MISPatientWiseCollectionReport(FormCollection form)
        
        {
            try
            {

                misPatientWiseCollection.DateFrom = Convert.ToDateTime(form["FromDate"]);
                misPatientWiseCollection.DateTo = Convert.ToDateTime(form["ToDate"]);
                DateFrom = misPatientWiseCollection.DateFrom;
                DateTo = misPatientWiseCollection.DateTo;

                misPatientWiseCollection.PatientType = form["PatientType"];
                misPatientWiseCollection.PatientRegNo = form["PatientRegNo"];
                misPatientWiseCollection.BillType = form["PatientType"];

                if (misPatientWiseCollection.PatientRegNo == "" || misPatientWiseCollection.PatientRegNo == null)
                {
                    misPatientWiseCollection.PatientRegNo = "%";
                }
                misPatientWiseCollection.dsPatientReport = misPatientReport(misPatientWiseCollection);


                return View(misPatientWiseCollection);
            }
            catch (Exception ex)
            {
                return View(misPatientWiseCollection);

            }
        }

        public DataSet misPatientReport(MISPatientWiseCollectionReport misPatientWiseCollection)
        {
            DataSet ds = new DataSet();

            Connect();
            try
            {
                HospitlLocationID();
                SqlCommand cmd = new SqlCommand("ReportMISPatientWiseCollectionReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", misPatientWiseCollection.DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", misPatientWiseCollection.DateTo);
                cmd.Parameters.AddWithValue("@PatientType", misPatientWiseCollection.PatientType);
                cmd.Parameters.AddWithValue("@PatientRegNo", misPatientWiseCollection.PatientRegNo);
                cmd.Parameters.AddWithValue("@BillType", misPatientWiseCollection.PatientType);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;

            }
        }

        public JsonResult GetPatientWiseReportWithDetails(string PatientRegNo, string BillType)
        {
            List<MISPatientWiseCollectionReport> searchList = new List<MISPatientWiseCollectionReport>();
            Connect();
            try
            {
                HospitlLocationID();

                Session["DateFrom"] = DateFrom;
                Session["DateTo"] = DateTo;
                Session["PatientRegNo"] = PatientRegNo;
                SqlCommand cmd = new SqlCommand("ReportMISPatientWiseCollectionReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DateTo);
                cmd.Parameters.AddWithValue("@PatientType", "%");
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@BillType", BillType);

                DataSet ds = new DataSet();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                Session["ReportMISPatientWiseCollectionReport"] = ds;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        MISPatientWiseCollectionReport rpt = new MISPatientWiseCollectionReport();
                        rpt.PatientRegNo = dr["PatientRegNo"].ToString();
                        rpt.PatientOPDIPDNo = dr["PatientOPDIPDNo"].ToString();
                        rpt.PatientDetails = dr["P.(Name/Date of Birth,Address )"].ToString();
                        rpt.PatientType = dr["PatinetType"].ToString();
                        rpt.BillType = dr["BillType"].ToString();
                        rpt.BillNo = dr["BillNo"].ToString();
                        rpt.TotalAmount = dr["TotalAmount"].ToString();
                        rpt.DateFrom = DateFrom;

                        rpt.DateTo = DateTo;
                        searchList.Add(rpt);
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           
        }
        public ActionResult RptMISPatientWiseCollectionReport()
        {
            return View();
        }
	}
}
