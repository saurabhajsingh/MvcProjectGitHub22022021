using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeystoneProject.Models.Report;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Patient;
using System.Data;

namespace KeystoneProject.Buisness_Logic.MISReport
{
    public class BL_MISPatientIPDDischargeSummary
    {
        int HospitalID= Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID =Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        List<MISPatientIPDDischargeSummary> SearchList =new List<MISPatientIPDDischargeSummary>();
        private SqlConnection con;

        private void Connect()
        {
            string conString= ConfigurationManager.ConnectionStrings["MyCon"].ToString();
            con = new SqlConnection(conString);
        }

        public List<Patient1> GetPatientName(string search)
        {
            Connect();

            List<Patient1> patientnamelist = new List<Patient1>();
            SqlCommand cmd = new SqlCommand("select PatientName,PatientRegNO,MobileNo,EmailID,Address from Patient where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and PatientName like '" + search + '%' + "' and RowStatus=0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                patientnamelist.Add(
                    new Patient1
                    {
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientRegNO = Convert.ToString(dr["PatientRegNO"])
                    });

            }
            return patientnamelist;
        }

        public List<MISPatientIPDDischargeSummary> MISPatientIPDDischargeSummary(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string PatientType, string SearchName, string EndResult)
        {
            Connect();

            List<MISPatientIPDDischargeSummary>  MISPatientIPDDischargeSummary= new List<MISPatientIPDDischargeSummary>();
            SqlCommand cmd = new SqlCommand("ReportMISPatientIPDDischargeSummary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime( DateFrom));
            cmd.Parameters.AddWithValue("@DateTo",  Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
            cmd.Parameters.AddWithValue("@SearchName", SearchName);
            cmd.Parameters.AddWithValue("@EndResult", EndResult);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad=new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                MISPatientIPDDischargeSummary.Add(
                    new MISPatientIPDDischargeSummary
                    {
                        PatientName = Convert.ToString(dr["PatientDetails"]),
                        PatientRegNo = Convert.ToString(dr["RegNO"]),
                        EndResult = Convert.ToString(dr["EndResult"]),
                        Age= Convert.ToString(dr["Age"]),
                        IPDID = Convert.ToString(dr["IPDID"]),
                        Ward = Convert.ToString(dr["Ward"]),
                        FinalDiagnosis = Convert.ToString(dr["FinalDiagnosis"]),
                        ConsultantDr = Convert.ToString(dr["ConsultantDr"]),
                        ReffDocName = Convert.ToString(dr["ReffDocName"]),
                        AddmissionDate = Convert.ToString(dr["AddmissionDate"]),
                        Dischargedate = Convert.ToString(dr["Dischargedate"]),
                        PatientStatus = Convert.ToString(dr["PatientStatus"]),
                        BillsAmount = Convert.ToString(dr["BillsAmount"]),
                        PaidAmount = Convert.ToString(dr["PaidAmount"]),
                        BalanceAmount = Convert.ToString(dr["BalanceAmount"]),

                    });

            }
            return MISPatientIPDDischargeSummary;

            

           
        }
    }
}