using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports
{
    public class BL_MISHospitalExpencesReport
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public DataSet GetServiceGroupID(string prefix)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllServiceGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }


        public DataSet GetHospitalExpences(DateTime DateFrom, DateTime DateTo, string GroupID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetHospitalExpences", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DateTo);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                DataSet dscolumn = new DataSet();
                dscolumn.Tables.Add(new DataTable());
                int row=0;
                //foreach(DataRow dr in ds.Tables[0].Rows)
                //{
                //    dscolumn.Tables[0].Columns.Add(dr["HospitalExpencesName"].ToString());

                //       row++;
                //}

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