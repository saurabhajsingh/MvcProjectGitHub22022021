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
    public class BL_Bank
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

        public bool Save(Bank obj)
        {
            Connect();
            try
            {
                string BankID = "";
                string[] BankID1 = new string[2];

               
                string[] BankName = obj.BankName.Split(',');
                if (obj.BankID == "0" || obj.BankID == null || obj.BankID == "")
                {
                    BankID = "0";
                    
                }
                else
                {
                    BankID1 = obj.BankID.Split(',');
                    BankID1 = BankID1.Where(name => !string.IsNullOrEmpty(name)).ToArray();
                     
                 
                 string.Join("", BankID1);
                 BankID = BankID1[0];
                // BankID = "1";
               
                }
               

                SqlCommand cmd = new SqlCommand("IUBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.ReferenceCode == null)
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                }
                if (BankID == "0")
                {
                    cmd.Parameters.AddWithValue("@BankID", 0);
                    cmd.Parameters["@BankID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BankID",Convert.ToInt32(BankID));
                    cmd.Parameters.AddWithValue("@Mode", "Edit");

                }
                if (obj.BankName == null)
                {
                     cmd.Parameters.AddWithValue("@BankName", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BankName", obj.BankName);
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
                    cmd.Parameters.AddWithValue("@PhoneNO", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PhoneNO", obj.PhoneNo);
                }

                if (obj.MobileNo == null)
                {
                    cmd.Parameters.AddWithValue("@MobileNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                }

                if (obj.Fax == null)
                {
                    cmd.Parameters.AddWithValue("@Fax", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Fax", obj.Fax);
                }

                if (obj.EmailID == null)
                {
                    cmd.Parameters.AddWithValue("@EmailID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                }

                if (obj.Pincode == null)
                {
                    cmd.Parameters.AddWithValue("@PinCode", 0);
                }

                else
                {
                    cmd.Parameters.AddWithValue("@PinCode", obj.Pincode);
                }

                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                obj.BankID = Convert.ToString(cmd.Parameters["@BankID"].Value);
                con.Close();
                if (i > 0)
                {
                    string[] AccountName = obj.AccountName.Split(',');
                    AccountName = AccountName.Where(name => !string.IsNullOrEmpty(name)).ToArray();
                    string[] AccountNumber = obj.AccountNumber.Split(',');
                    AccountNumber = AccountNumber.Where(name => !string.IsNullOrEmpty(name)).ToArray();


                    for (int k = 0; k < AccountName.Length; k++)
                    {
                        SqlCommand cmd1 = new SqlCommand("IUBankAccountDetail", con);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                        if (obj.BankAccountID == null || obj.BankAccountID == "")
                        {
                            cmd1.Parameters.AddWithValue("@BankAccountID", 0);

                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@BankAccountID", obj.BankAccountID);
                            cmd1.Parameters.AddWithValue("@Mode", "Edit");
                        }
                        if (obj.ReferenceCode == null)
                        {
                            cmd1.Parameters.AddWithValue("@ReferenceCode", 0);
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                        }
                        if (obj.BankID == null)
                        {
                            cmd1.Parameters.AddWithValue("@BankID", 0);
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@BankID", obj.BankID);
                        }
                        cmd1.Parameters.AddWithValue("@BankAccountNo", AccountNumber[k]);
                        cmd1.Parameters.AddWithValue("@AccountName", AccountName[k]);
                        cmd1.Parameters.AddWithValue("@CreationID", UserID);

                        con.Open();
                        int j = cmd1.ExecuteNonQuery();
                        con.Close();
                        
                    }
                }
                
            }
            catch (Exception ex)
            { }

            return true;
        }

        public DataSet GetCity(string CityName, string City)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.MasterFinacialAccounts.Bank obj = new Models.MasterFinacialAccounts.Bank();
                Connect();
                SqlCommand cmd = new SqlCommand("select  CityID,CityName from City where  CityName like '" + CityName + "%" + "' and   CityID  like  '" + City + "%" + "' and HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "' and RowStatus=0 order by  CityName asc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
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
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public List<Bank> SelectAllBank()
        {
            Connect();
            List<Bank> accounts = new List<Bank>();
            SqlCommand cmd = new SqlCommand("GetAllBank", con);
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
                    new Bank
                    {
                        BankID = Convert.ToString(dr["BankID"]),
                        BankName = Convert.ToString(dr["BankName"]),
                       
                    });
            }
            return accounts;
        }
        public bool CheckBank(string BankID, string BankName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@BankID", BankID);
                cmd.Parameters.AddWithValue("@BankName", BankName.ToUpper());
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
            catch (Exception )
            {
                return false;
            }
        }
        public DataSet SelectAllServicesbyID(int BankID)
        {
            SqlConnection con = null;

            DataSet ds = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                Connect();
                SqlCommand cmd = new SqlCommand("GetBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BankID", BankID); // i will pass zero to MobileID beacause its Primary .
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                con.Close();
            }
        }
        public int DeleteBank(int BankID)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeleteBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BankID", BankID);

                con.Open();
                i = cmd.ExecuteNonQuery();

                con.Close();

            }
            return i;
        }
      
    }
}