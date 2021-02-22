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
    public class BL_MISVoucherEntryWiseReport
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

        public DataSet MISVoucherEntryWiseReport(MISVoucherEntryWiseReport DoctorWiseModel)
        {
            Connect();
            DataSet dsDoctorWiseCollection = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetVoucherEntryWiseReportForAccountDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", DoctorWiseModel.DateFrom);
                cmd.Parameters.AddWithValue("@ToDate", DoctorWiseModel.DateTo);
                cmd.Parameters.AddWithValue("@ScheduleID", DoctorWiseModel.ScheduleID);
                cmd.Parameters.AddWithValue("@AccountsID", DoctorWiseModel.AccountID);
                cmd.Parameters.AddWithValue("@TransactionType", DoctorWiseModel.TransactionType);
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
        public DataSet BindSchedule(string GetScheduleName)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ScheduleID , ScheduleName  FROM Schedule where  ScheduleName like ''+@GetScheduleName+'%' and  HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "' and RowStatus = 0 order by  ScheduleName  asc", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GetScheduleName", GetScheduleName);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

         public DataSet BindAccount(string GetAccount)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select AccountsID,AccountName from Accounts where AccountName like ''+@GetAccount+'%' and  RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " order by AccountName asc", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GetAccount", GetAccount);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }


         public DataSet BindVoucherName(string GetVoucherName)
         {
             Connect();
             DataSet ds = new DataSet();
             try
             {
                 SqlCommand cmd = new SqlCommand("select VoucherTypeID,VoucherTypeName from VoucherType where VoucherTypeName like ''+@GetVoucherName+'%' and  RowStatus = 0 and HospitalID =  " + HospitalID + " and LocationID = " + LocationID + " order by VoucherTypeName asc", con);
                 cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                 cmd.Parameters.AddWithValue("@LocationID", LocationID);
                 cmd.Parameters.AddWithValue("@GetVoucherName", GetVoucherName);
                 con.Open();
                 SqlDataAdapter ad = new SqlDataAdapter(cmd);
                 con.Close();
                 ad.Fill(ds);
             }
             catch (Exception ex)
             { }
             return ds;
         }
    }
}


