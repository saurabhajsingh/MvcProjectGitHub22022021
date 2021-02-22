using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.PatientReport
{
    public class BL_PatientBillNoReport
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        public DataSet BindPatientDetails(string GetPatientDetails)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select Patient.PatientRegNo,Patient.PatientName as PatientName from PatientBills left join Patient on Patient.PatientRegNO=PatientBills.PatientRegNo  where  PatientName like ''+@GetPatientDetails+'%' and Patient.HospitalID = " + HospitalID + " and Patient.LocationID = " + LocationID + " and Patient.RowStatus= 0 group by  Patient.PatientRegNO,PatientName ORDER BY Patient.PatientName asc ", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GetPatientDetails", GetPatientDetails);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetReportMISPatientBillNoWiseReport(string PatientRegNo, string FromBillNo, string ToBillNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISPatientBillNoWiseReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNo",PatientRegNo);
                cmd.Parameters.AddWithValue("@FromBillNo", FromBillNo);
                cmd.Parameters.AddWithValue("@ToBillNo", ToBillNo);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
    }
}