using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KeystoneProject.Models.Patient;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_BedStatus
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private void Connect()
        {
           string constring = ConfigurationManager.ConnectionStrings["MyCon"].ToString();
           con = new SqlConnection(constring);
        }

        public DataSet GetAllBedStatus()
        {
            Connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("GetAllBedStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            con.Open();
            da.Fill(ds);
            con.Close();

            return ds;
        }

        public DataSet GetAllBedStatusDashboad()
        {
            Connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("GetAllBedStatusDashboad", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            con.Open();
            da.Fill(ds);
            con.Close();

            return ds;
        }

        //public DataSet GetAllBedStatus(int HospitalID, int LocationID)
        //{
        //    Connect();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlParameter[] para = new SqlParameter[7];
        //        para[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
        //        para[0].Value = HospitalID;
        //        para[1] = new SqlParameter("@LocationID", SqlDbType.Int);
        //        para[1].Value = LocationID;

        //        ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllBedStatus", para);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return ds;
        //}

        public DataSet GetBedStatus(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientIPDBedStatusForAllID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID" , HospitalID);
                cmd.Parameters.AddWithValue("@LocationID" , LocationID);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch(Exception ex)
            {
              return ds;
            }
        }
    }
}