using KeystoneProject.Models.MISReport.MIS_PatientReports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports
{
    public class BL_MISWardWiseDetails
    {
        private SqlConnection con;

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }
        public DataSet GetRptMISWardWiseDetails(DateTime DateFrom, DateTime DateTo, string PatientType )
        {

            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("RptMISWardWiseDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(DateFrom));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(DateTo));
                cmd.Parameters.AddWithValue("@ConsultantDrID", "%");
                cmd.Parameters.AddWithValue("@WardID", "%");
                cmd.Parameters.AddWithValue("@PatientType", PatientType);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                HttpContext.Current.Session["RptMISWardWiseDetails"]=ds;
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        
        }
    }
}
