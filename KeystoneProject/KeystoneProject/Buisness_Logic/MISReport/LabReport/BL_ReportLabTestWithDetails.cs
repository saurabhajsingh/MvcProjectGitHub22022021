using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.MISReport.LabReport;
using KeystoneProject.Controllers.MISReport.LabReport;

namespace KeystoneProject.Buisness_Logic.MISReport.LabReport
{
    public class BL_ReportLabTestWithDetails
    {

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }

        public DataSet ReportTestForPatientLabTestDetails(string LabNo, string Regno, string ID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportTestForPatientLabTestDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@LabNo", LabNo);
                cmd.Parameters.AddWithValue("@PatientRegNo", Regno);
                cmd.Parameters.AddWithValue("@OPDIPDID", ID);

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

        public DataSet ReportLabTestWithDetails(DateTime FromDate, DateTime ToDate, string PatientType)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportLabTestWithDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PatientType", PatientType);

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