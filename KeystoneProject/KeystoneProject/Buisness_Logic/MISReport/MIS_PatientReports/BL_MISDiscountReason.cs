using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using KeystoneProject.Controllers.MISReport.MIS_PatientReports;
using KeystoneProject.Buisness_Logic.MISReport.MISPatientReport;

namespace KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports
{
    public class BL_MISDiscountReason
    {

        int HospitalID;
        int LocationID;
        int UserID;

        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }
        public DataSet GETallDiscountReson(DateTime DateFrom, DateTime DateTo, string PatientType)
        {

            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISPatientDiscountReasons", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
                cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
                cmd.Parameters.AddWithValue("@PatientType", PatientType);
              

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                HttpContext.Current.Session["MISDiscountReason"] = ds;
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }

        }
    }
}