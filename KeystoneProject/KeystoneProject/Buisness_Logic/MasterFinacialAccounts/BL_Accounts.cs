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
    public class BL_Accounts
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

        public bool Save(Accounts obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUAccounts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.AccountsID == "0" || obj.AccountsID == null || obj.AccountsID == "")
            {
                cmd.Parameters.AddWithValue("@AccountsID", 0);
                cmd.Parameters["@AccountsID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@AccountsID", obj.AccountsID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }

            if (obj.ReferenceCode == null)
            {
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            }

            cmd.Parameters.AddWithValue("@AccountName", obj.AccountName);

            if (obj.ScheduleID == null)
            {
                cmd.Parameters.AddWithValue("@ScheduleID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ScheduleID", obj.ScheduleID);
            }

            if (obj.OpeningBalance == null)
            {
                cmd.Parameters.AddWithValue("@OPBalance", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@OPBalance", obj.OpeningBalance);
            }
            if (obj.OpeningBalance == "Credit")
            {
                cmd.Parameters.AddWithValue("@CrAmount", obj.CrAmount);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CrAmount", 0.00);
            }


            if (obj.OpeningBalance == "Debit")
            {
                cmd.Parameters.AddWithValue("@DrAmount", obj.CrAmount);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DrAmount", 0.00);

            }
            if (obj.CreditLimit == null)
            {
                cmd.Parameters.AddWithValue("@CrLimit", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CrLimit", obj.CreditLimit);
            }
            if (obj.PrintName == null)
            {
                cmd.Parameters.AddWithValue("@PrintName", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PrintName", obj.PrintName);
            }
            if (obj.CreditDays == null)
            {
                cmd.Parameters.AddWithValue("@CreditDays", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreditDays", obj.CreditDays);
            }
            if (obj.AccountType == null)
            {
                cmd.Parameters.AddWithValue("@AccountType", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@AccountType", obj.AccountType);
            }
            if (obj.Address == null)
            {
                cmd.Parameters.AddWithValue("@Address", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Address", obj.Address);
            }
            if (obj.CityID == null)
            {
                cmd.Parameters.AddWithValue("@CityID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CityID", obj.CityID);
            }
            if (obj.PinCode == null)
            {
                cmd.Parameters.AddWithValue("@PinCode", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
            }

            if (obj.StateID == null)
            {
                cmd.Parameters.AddWithValue("@StateID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@StateID", obj.StateID);
            }
            if (obj.CountryID == null)
            {
                cmd.Parameters.AddWithValue("@CountryID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
            }
            if (obj.PhoneNo == null)
            {
                cmd.Parameters.AddWithValue("@PhoneNo", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNo);
            }
           
            if (obj.MobileNo == null)
            {
                cmd.Parameters.AddWithValue("@MobileNo", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
            }
            if (obj.EmailID == null)
            {
                cmd.Parameters.AddWithValue("@EmailID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
            }
            if (obj.Remark == null)
            {
                cmd.Parameters.AddWithValue("@Remark", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Remark", obj.Remark);
            }
            if (obj.Fax == null)
            {
                cmd.Parameters.AddWithValue("@FAX", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FAX", obj.Fax);
            }
            if (obj.TIN == null)
            {
                cmd.Parameters.AddWithValue("@TinNo", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TinNo", obj.TIN);
            }
            if (obj.PanNo == null)
            {
                cmd.Parameters.AddWithValue("@Pan", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Pan", obj.PanNo);
            }
            if (obj.TinDate == null)
            {
                cmd.Parameters.AddWithValue("@TinDate", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TinDate", obj.TinDate);
            }
            if (obj.GstNo == null)
            {
                cmd.Parameters.AddWithValue("@CSTNO", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CSTNO", obj.GstNo);
            }
           
            cmd.Parameters.AddWithValue("@CreationID", UserID);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
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

        public DataSet GetCity(string CityName, string City)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
                Connect();
                SqlCommand cmd = new SqlCommand("select  CityID,CityName from City where  CityName like '" + CityName + "%" + "' and   CityID  like  '" + City + "%" + "' and  HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "' and RowStatus=0 order by  CityName asc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception )
            {

            }
            return ds;
        }

        public DataTable GetCountryStateID(int CityID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("getCountryStateID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", @LocationID);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public List<Accounts> SelectAllAccounts()
        {
            Connect();
            List<Accounts> accounts = new List<Accounts>();
            SqlCommand cmd = new SqlCommand("GetAllAccounts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();
           

            foreach (DataRow dr in dt.Rows)
            {
                accounts.Add(
                    new Accounts
                    {
                        AccountsID = Convert.ToString(dr["AccountsID"]),
                        AccountName = Convert.ToString(dr["AccountName"]),
                        Schedule = Convert.ToString(dr["ScheduleName"]),
                        DrAmount = Convert.ToString(dr["DrAmount"]),
                        CrAmount = Convert.ToString(dr["CrAmount"]),
                        CrLimit = Convert.ToString(dr["CrLimit"]),
                        PrintName = Convert.ToString(dr["PrintName"]),
                        Locality = Convert.ToString(dr["Locality"]),
                        CreditDays = Convert.ToString(dr["CreditDays"]),
                        Address = Convert.ToString(dr["Address"]),
                        PinCode = Convert.ToString(dr["PinCode"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        EmailID = Convert.ToString(dr["EmailID"]),
                        Remark = Convert.ToString(dr["Remark"]),
                        Fax = Convert.ToString(dr["FAX"]),
                        TIN = Convert.ToString(dr["TinNo"]),
                        PanNo = Convert.ToString(dr["Pan"]),
                        TinDate = Convert.ToDateTime(dr["TinDate"]).ToString("yyyy-MM-dd"),
                        GstNo = Convert.ToString(dr["CSTNO"]),
                        OpeningBalance = Convert.ToString(dr["OPBalance"]),
                        CityID = Convert.ToString(dr["CityID"]),
                        StateID = Convert.ToString(dr["StateID"]),
                        CountryID = Convert.ToString(dr["CountryID"]),
                        City = Convert.ToString(dr["CityName"]),
                        State = Convert.ToString(dr["StateName"]),
                        Country = Convert.ToString(dr["CountryName"]),
                        ScheduleID = Convert.ToString(dr["ScheduleID"]),
                    });
            }
            return accounts;
        }
        public int DeleteAccounts(int AccountsID)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeleteAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@AccountsID", AccountsID);

                con.Open();
                i = cmd.ExecuteNonQuery();

                con.Close();

            }
            return i;
        }

        public bool CheckAccounts(string AccountID, string AccountName)
        {
            string t = "";
            if (AccountID == null || AccountID == "")
            {
                t = "0";
            }
            else
            {
                t = AccountID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@AccountsID", t);
                cmd.Parameters.AddWithValue("@AccountName", AccountName.ToUpper());
                cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@NameExists"].Value;
                if (Convert.ToInt32(Table) == 1)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}