using KeystoneProject.Models.FinancialAccount;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.FinancialAccount
{
    public class BL_AccountStatement
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<AccountStatement> accountstatement = new List<AccountStatement>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet GetAccountStatementDetails(AccountStatement prefix)
        {
            Connect();
            DataSet dsAccountStatement = null;

            try
            {
                SqlCommand cmd = new SqlCommand("GetAccountStatementDetails",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", prefix.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", prefix.ToDate);
                cmd.Parameters.AddWithValue("@AccountsID", prefix.AccountsID);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand =  cmd;
                dsAccountStatement = new DataSet();
                da.Fill(dsAccountStatement);
                con.Close();
                return dsAccountStatement;
            }
            catch (Exception)
            {
                return dsAccountStatement;
            }
          
        }

        public DataSet GetAccountStatementForAccountDetail(AccountStatement prefix1)
        {
            Connect();
            DataSet dsAccountStatementDetails = null;

            try
            {
                SqlCommand cmd = new SqlCommand("GetAccountStatementForAccountDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", prefix1.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", prefix1.ToDate);
                cmd.Parameters.AddWithValue("@AccountsID", prefix1.AccountsID);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsAccountStatementDetails = new DataSet();
                da.Fill(dsAccountStatementDetails);
                con.Close();
                return dsAccountStatementDetails;
            }
            catch (Exception)
            {
                return dsAccountStatementDetails; 
            }
        }

        public DataSet BindAccountStatement(String filter)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  AccountsID,AccountName from  Accounts  where AccountName like ''+@filter+'%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  AccountName asc", con);
            cmd.Parameters.AddWithValue("@filter",filter);
            DataSet ds = new DataSet ();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;
        }
    }
}