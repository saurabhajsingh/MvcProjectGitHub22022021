using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace KeystoneProject.Controllers.Laboratory
{
    public class PatientDetailResultEntryController : Controller
    {
        int HospitalID;
        int LocationID;
        int CreationID;

        //
        // GET: /PatientSearch/
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
      
        //
        // GET: /PatientDetailResultEntry/
        public ActionResult PatientDetailResultEntry()
        {
            return View();
        }

      
        public ActionResult ReportLabTestByName()
        {
            List<KeystoneProject.Models.Laboratory.PatientLabBills> searchAdd = new List<Models.Laboratory.PatientLabBills>();

            KeystoneProject.Models.Laboratory.PatientLabBills obj = new Models.Laboratory.PatientLabBills();
            connection();
            HospitlLocationID();
            SqlCommand cmd=new SqlCommand("ReportLabTestByName1",con);
            cmd.CommandType = CommandType.StoredProcedure;
             cmd.Parameters.AddWithValue("@HospitalID",HospitalID);
             cmd.Parameters.AddWithValue("@LocationID",LocationID);
             cmd.Parameters.AddWithValue("@FromDate", DateTime.Now);
             cmd.Parameters.AddWithValue("@ToDate", DateTime.Now);
             SqlDataAdapter ad = new SqlDataAdapter();
             ad.SelectCommand = cmd;
             DataSet ds = new DataSet();
             ad.Fill(ds);
             string FromDate = ds.Tables[1].Rows[0]["DateFrom"].ToString();
             string ToDate = ds.Tables[1].Rows[0]["DateTo"].ToString();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                string TestStatus = "SAMPLECOLLECTION";
                int Complete = 0;
                int Approve = 0;
                DataView dv = new DataView(ds.Tables[2], "LabNo in (" + dr["BillNo"].ToString() + ") and  TestStatus='" + TestStatus + "'", "", DataViewRowState.CurrentRows);
                TestStatus = "COMPLETED";
                DataView dvComplete = new DataView(ds.Tables[2], "LabNo in (" + dr["BillNo"].ToString() + ") and  TestStatus='" + TestStatus + "'", "", DataViewRowState.CurrentRows);
                DataView dvApprove = new DataView(dvComplete.ToTable(), "LabNo in (" + dr["BillNo"].ToString() + ") and  AuthorizedID >'" + 0 + "'", "", DataViewRowState.CurrentRows);

                if (dvComplete.ToTable().Rows.Count > 0)
                {
                    Complete = dvComplete.ToTable().Rows.Count;
                }
                if (dvApprove.ToTable().Rows.Count > 0)
                {
                    Approve = dvApprove.ToTable().Rows.Count;
                }
                int incompte=  dr["TestName"].ToString().Split(',').Length-1;
                searchAdd.Add(new KeystoneProject.Models.Laboratory.PatientLabBills
                {
                    PatientName = dr["Patient"].ToString(),
                    PatientRegNo = dr["RegNo"].ToString(),
                    DoctorPrintName = dr["DoctorPrintName"].ToString(),
                    BillNo = dr["BillNo"].ToString(),
                    TestName = dr["TestName"].ToString(),
                    FromDate=FromDate,
                    ToDate=ToDate,
                    Incomplete=incompte.ToString(),
                    Completed = Complete.ToString(),
                    Approved = Approve.ToString()
                });
            }

            return new JsonResult { Data = searchAdd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public DataSet GetPatientLabBillDetails(int LabNo)
        {
            List<KeystoneProject.Models.Laboratory.PatientLabBills> searchAdd = new List<Models.Laboratory.PatientLabBills>();

            KeystoneProject.Models.Laboratory.PatientLabBills obj = new Models.Laboratory.PatientLabBills();
            connection();
            HospitlLocationID();
            SqlCommand cmd = new SqlCommand("GetPatientLabBillDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@LabNo", LabNo);
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            DataSet ds = new DataSet();
            ad.Fill(ds);
            return ds;
        }
	}
}