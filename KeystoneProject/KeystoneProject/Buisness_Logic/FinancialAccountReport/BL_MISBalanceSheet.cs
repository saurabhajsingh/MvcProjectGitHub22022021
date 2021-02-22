using KeystoneProject.Models.FinancialAccountReport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.FinancialAccountReport
{
    public class BL_MISBalanceSheet
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

        public DataSet GetLiabilitiesDetails(MISBalanceSheet DoctorWiseModel)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetLiabilitiesDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DoctorWiseModel.DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DoctorWiseModel.DateTo);
                cmd.Parameters.AddWithValue("@ScheduleID", DoctorWiseModel.ScheduleID);
                cmd.Parameters.AddWithValue("@ScheduleName", DoctorWiseModel.ScheduleName);
                cmd.Parameters.AddWithValue("@Type", DoctorWiseModel.Type);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetExpensesDetails(MISBalanceSheet DoctorWiseModel)
        {
            Connect();
            DataSet dsDoctorWiseCollection = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetExpensesDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DoctorWiseModel.DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DoctorWiseModel.DateTo);
                cmd.Parameters.AddWithValue("@ScheduleID", DoctorWiseModel.ScheduleID);
                cmd.Parameters.AddWithValue("@ScheduleName", DoctorWiseModel.ScheduleName);
                cmd.Parameters.AddWithValue("@Type", DoctorWiseModel.Type);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsDoctorWiseCollection = new DataSet();
                da.Fill(dsDoctorWiseCollection);
                con.Close();

                return dsDoctorWiseCollection;
            }
            catch (Exception)
            {
                return dsDoctorWiseCollection;
            }
        }

        public DataSet GetIncomeDetails(MISBalanceSheet DoctorWiseModel)
        {
            Connect();
            DataSet dsDoctorWiseCollection = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetIncomeDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DoctorWiseModel.DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DoctorWiseModel.DateTo);
                cmd.Parameters.AddWithValue("@ScheduleID", DoctorWiseModel.ScheduleID);
                cmd.Parameters.AddWithValue("@ScheduleName", DoctorWiseModel.ScheduleName);
                cmd.Parameters.AddWithValue("@Type", DoctorWiseModel.Type);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsDoctorWiseCollection = new DataSet();
                da.Fill(dsDoctorWiseCollection);
                con.Close();

                return dsDoctorWiseCollection;
            }
            catch (Exception)
            {
                return dsDoctorWiseCollection;
            }
        }
    }
}