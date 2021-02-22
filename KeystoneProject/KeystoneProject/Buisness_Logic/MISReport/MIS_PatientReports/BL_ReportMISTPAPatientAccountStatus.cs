using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports
{
    public class BL_ReportMISTPAPatientAccountStatus
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

        public DataSet ReportMISTPAPatientAccountStatus(DateTime FromDate,DateTime ToDate,string PatientType)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISTPAWisePatientAccountStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", FromDate);
                cmd.Parameters.AddWithValue("@DateTo", ToDate);
                cmd.Parameters.AddWithValue("@PatientType", PatientType);
                cmd.Parameters.AddWithValue("@DisplayType", "PatientWise");
                cmd.Parameters.AddWithValue("@PatientRegNo", "%");
                cmd.Parameters.AddWithValue("@BillNo", "%");
                cmd.Parameters.AddWithValue("@TPAType", "%");
                cmd.Parameters.AddWithValue("@PrintIPDNO", "%");
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                HttpContext.Current.Session["ReportMISTPAWisePatientAccountStatus"] = ds;
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet ReportMISTPAPatientAccountStatusINDetail(DateTime DateFrom, DateTime DateTo, string PatientRegNO, string PatientType, string Type)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISTPAPatientAccountStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DateTo);
                cmd.Parameters.AddWithValue("@PatientType", PatientType);
                cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                cmd.Parameters.AddWithValue("@Type", "DETAILS");
               // cmd.Parameters.AddWithValue("@TPANAME", TPANAME);
             
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                HttpContext.Current.Session["ReportMISTPAPatientAccountStatus"] = ds;
                return ds;
               
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                //throw ex;
                return ds;
            }
           
        }

    }
}