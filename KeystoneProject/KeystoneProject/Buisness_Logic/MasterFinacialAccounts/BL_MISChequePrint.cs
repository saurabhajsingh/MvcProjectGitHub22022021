using KeystoneProject.Models.MasterFinacialAccounts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MasterFinacialAccounts
{
    public class BL_MISChequePrint
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
        public DataSet GetAllBank()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetAllChequeBook()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllChequeBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetAllChequeNumber()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetChequeNumber", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetAllPayeeName()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetChequeNumber", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetAllAccountName()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        //public List<MISChequePrint> SelectAllChequePrint()
        //{
        //    Connect();
        //    List<MISChequePrint> accounts = new List<MISChequePrint>();
        //    SqlCommand cmd = new SqlCommand("GetChequePrint", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
        //    cmd.Parameters.AddWithValue("@LocationID", LocationID);

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    con.Close();
        //    da.Fill(dt);
        //    con.Close();


        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        accounts.Add(
        //            new MISChequePrint
        //            {
        //                BankName = Convert.ToString(dr["BankName"]),
        //                PayeeName = Convert.ToString(dr["PayeeName"]),
        //                BookName = Convert.ToString(dr["BookName"]),
        //                ChequeNo = Convert.ToString(dr["ChequeNo"]),
        //                TDS = Convert.ToString(dr["TDS"]),
        //                Amount = Convert.ToString(dr["Amount"]),
        //                ChequeAmount = Convert.ToString(dr["ChequeAmount"]),
        //                AccountName = Convert.ToString(dr["AccountName"]),
        //                Narration = Convert.ToString(dr["Narration"]),
        //                ChequeStatus = Convert.ToString(dr["ChequeStatus"]),
        //            });
        //    }
        //    return accounts;
        //}

    }
}