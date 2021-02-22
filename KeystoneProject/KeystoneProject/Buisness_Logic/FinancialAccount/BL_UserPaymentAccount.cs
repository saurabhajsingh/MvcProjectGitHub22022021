using KeystoneProject.Models.Financial;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.FinancialAccount
{
    public class BL_UserPaymentAccount
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

        public bool Save(UserPaymentAccount obj)
        {

            Connect();

            string[] AccID = obj.AccountsID.Split(',');
           // AccountsID = AccountsID.Where(name => !string.IsNullOrEmpty(name)).ToArray();

            string[] AccName = obj.AccountName.Split(',');
           // AccountName = AccountName.Where(name => !string.IsNullOrEmpty(name)).ToArray();

            string[] UsID = obj.UserID.Split(',');
           // UserID = UserID.Where(name => !string.IsNullOrEmpty(name)).ToArray();

            string[] FName = obj.FullName.Split(',');
           // FullName = FullName.Where(name => !string.IsNullOrEmpty(name)).ToArray();

            for (int k = 0; k < FName.Length; k++)
            {
                SqlCommand cmd = new SqlCommand("IUUserPayementAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.UserPaymentAccountID == "0" || obj.UserPaymentAccountID == null || obj.UserPaymentAccountID == "")
                {
                    cmd.Parameters.AddWithValue("@UserPaymentAccountID", 0);
                    cmd.Parameters["@UserPaymentAccountID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UserPaymentAccountID", obj.UserPaymentAccountID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }
                if (obj.ReferenceCode == null || obj.ReferenceCode == "")
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                }
                if (obj.AccountsID == null || obj.AccountsID == "")
                {
                    cmd.Parameters.AddWithValue("@MasterAccountsID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MasterAccountsID", AccID[k]);
                 }

                if (obj.AccountName == null || obj.AccountName =="")
                {
                    cmd.Parameters.AddWithValue("@MasterAccountName", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MasterAccountName", AccName[k]);
                }

                if (obj.UserID == null || obj.UserID =="")
                {
                    cmd.Parameters.AddWithValue("@UserID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UserID", UsID[k]);
                }

                if (obj.FullName == null || obj.FullName =="")
                {
                    cmd.Parameters.AddWithValue("@UserName", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UserName", FName[k]);
                }

                if (obj.CurrentDate == null || obj.CurrentDate =="")
                {
                    cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CurrentDate", obj.CurrentDate);
                }

                if (obj.Narration == null || obj.Narration == "")
                {
                    cmd.Parameters.AddWithValue("@Narration", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Narration", obj.Narration);
                }

                if (obj.PaidAmount == null || obj.PaidAmount=="")
                {
                    cmd.Parameters.AddWithValue("@PaidAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PaidAmount", obj.PaidAmount);
                }

                if (obj.BalanceAmount == null || obj.BalanceAmount =="")
                {
                    cmd.Parameters.AddWithValue("@BalanceAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BalanceAmount", obj.BalanceAmount);
                }

                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                
               
            }
            return true;
           
        }

        public DataSet BindMasterAccountName(string GetAccountName)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                string account =  "Master Account";
                SqlCommand cmd = new SqlCommand("select AccountsID,  AccountName ,Schedule.ScheduleName,  OPBalance, DrAmount ,CrLimit,PrintName, CreditDays,Address,PhoneNo,MobileNo,EmailID,TinNo,Pan,TinDate   from Accounts left join Schedule on Schedule.ScheduleID = Accounts.ScheduleID where Accounts.AccountType='" + account + "' and  Accounts.RowStatus = 0 and Accounts.HospitalID = " + HospitalID + " and Accounts.LocationID = " + LocationID + " order by Accounts.AccountName asc", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GetAccountName", GetAccountName);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet BindUserName(string UserName)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("SELECT UserID,FullName,ReferenceCode,EmailID FROM Users where FullName like ''+@UserName+'%' and  Users.RowStatus = 0  order by Users.FullName asc", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public List<UserPaymentAccount> SelectAllUserPaymentAccount()
        {
            Connect();
            List<UserPaymentAccount> accounts = new List<UserPaymentAccount>();
            SqlCommand cmd = new SqlCommand("GetAllUsersPaymentAccount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                accounts.Add(
                    new UserPaymentAccount
                    {
                        AccountsID = Convert.ToString(dr["MasterAccountsID"]),
                        AccountName = Convert.ToString(dr["MasterAccountName"]),
                        UserID = Convert.ToString(dr["UserID"]),
                        FullName = Convert.ToString(dr["UserName"]),
                        CurrentDate = Convert.ToDateTime(dr["CurrentDate"]).ToString("yyyy-MM-dd"),
                        UserPaymentAccountID = Convert.ToString(dr["UserPaymentAccountID"]),
                        Narration = Convert.ToString(dr["Narration"]),
                        PaidAmount = Convert.ToString(dr["PaidAmount"]),
                        BalanceAmount = Convert.ToString(dr["BalanceAmount"]),
                        
                    });
            }
            return accounts;
        }
        public DataSet GetBalanceAmount(int UserID, string CurrentDate, string Types)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetUsersPaymentAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@CurrentDate", CurrentDate);
                cmd.Parameters.AddWithValue("@Types", Types);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }




        public DataSet GetUserStatus(string Type)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientUserStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);                
                cmd.Parameters.AddWithValue("@Type", Type);
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