using KeystoneProject.Models.Keystone;
using KeystoneProject.Models.Patient;
using KeystoneProject.Models.Report;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MISReport
{
    public class BL_MISPatientWiseCollectionDetailsReport
    {
        private SqlConnection con;

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID1 = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        public List<Users> GetUserName(string search)
        {
            Connect();

            List<Users> usernamelist = new List<Users>();
            SqlCommand cmd = new SqlCommand("select FullName,UserID,LoginName,EmailID,RoleName from Users where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and FullName like '" + search + '%' + "' and RowStatus=0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                usernamelist.Add(
                    new Users
                    {
                        FullName = Convert.ToString(dr["FullName"]),
                        UserID = Convert.ToInt32(dr["UserID"])

                    });

            }
            return usernamelist;
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
        public List<MISPatientWiseCollectionDetailsReport> MISPatientWiseCollectionDetailsReportDetail(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string PatientRegNO, string UserID, string BillType, string PatientType)
        {
            Connect();

            List<MISPatientWiseCollectionDetailsReport> MISPatientWiseCollectionDetailsReport1 = new List<MISPatientWiseCollectionDetailsReport>();
            SqlCommand cmd = new SqlCommand("ReportMISPatientWiseCollectionReportWithDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
            cmd.Parameters.AddWithValue("@BillType", BillType);

            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                MISPatientWiseCollectionDetailsReport1.Add(
                    new MISPatientWiseCollectionDetailsReport
                    {

                        PatientRegNO = Convert.ToInt32(dr["PatientRegNO"]),
                        PatientOPDIPDNo = Convert.ToInt32(dr["PatientOPDIPDNo"]),
                        PatientDetails = Convert.ToString(dr["PatientDetails"]),
                        PatientType = Convert.ToString(dr["PatientType"]),
                        BillType = Convert.ToString(dr["BillType"]),
                        BillNo = Convert.ToInt32(dr["BillNo"]),
                        PatientAccountRowID = Convert.ToInt32(dr["PatientAccountRowID"]),
                        BillsAmount = Convert.ToDecimal(dr["BillsAmount"]),
                        Discount = Convert.ToDecimal(dr["Discount"]),
                        PaidAmount = Convert.ToDecimal(dr["PaidAmount"]),
                        BalanceAmount = Convert.ToDecimal(dr["BalanceAmount"]),
                        Date = Convert.ToDateTime(dr["Date"]).ToString(),
                    });

            }
            return MISPatientWiseCollectionDetailsReport1;


        }


       

    }
}