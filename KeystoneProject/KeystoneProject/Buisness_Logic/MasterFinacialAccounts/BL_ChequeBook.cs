
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
    public class BL_ChequeBook
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
        public bool Save(ChequeBook obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUChequeBook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.ChequeBookID == "0" || obj.ChequeBookID == null || obj.ChequeBookID == "")
            {
                cmd.Parameters.AddWithValue("@ChequeBookID", 0);
                cmd.Parameters["@ChequeBookID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ChequeBookID", obj.ChequeBookID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            //if (obj.ReferenceCode == null)
            //{
            //    cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            //}

            if (obj.BankID == null)
            {
                cmd.Parameters.AddWithValue("@BankID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BankID", obj.BankID);
            }

            if (obj.BankAccountID == null)
            {
                cmd.Parameters.AddWithValue("@BankAccountID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BankAccountID", obj.BankAccountID);
            }

            if (obj.ChequeLayoutID == null)
            {
                cmd.Parameters.AddWithValue("@ChequeLayoutID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ChequeLayoutID", obj.ChequeLayoutID);
            }
            if (obj.ChequeBookName == null)
            {
                cmd.Parameters.AddWithValue("@BookName", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BookName", obj.ChequeBookName);
            }
            if (obj.ChequeNoFrom == null)
            {
                cmd.Parameters.AddWithValue("@ChequeSNo", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ChequeSNo", obj.ChequeNoFrom);
            }
            if (obj.ChequeNoTo == null)
            {
                cmd.Parameters.AddWithValue("@ChequeENo", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ChequeENo", obj.ChequeNoTo);
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

        public List<ChequeBook> SelectAllChequeBook()
        {
            Connect();
            List<ChequeBook> accounts = new List<ChequeBook>();
            SqlCommand cmd = new SqlCommand("GetAllChequeBook", con);
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
                    new ChequeBook
                    {
                        ChequeBookID = Convert.ToString(dr["ChequeBookID"]),
                        ChequeBookName = Convert.ToString(dr["BookName"]),

                    });
            }
            return accounts;
        }

        public DataSet FillData(int ChequeBookID)
        {
            SqlConnection con = null;

            DataSet ds = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                Connect();
                SqlCommand cmd = new SqlCommand("GetChequeBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChequeBookID", ChequeBookID); // i will pass zero to MobileID beacause its Primary .
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

        public DataSet GetBank(string BankName, string Bank)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.MasterFinacialAccounts.ChequeBook obj = new ChequeBook();
                Connect();
                SqlCommand cmd = new SqlCommand("select  BankID,BankName from Bank where  BankName like '" + BankName + "%" + "' and   BankID  like  '" + Bank + "%" + "' and RowStatus=0 order by  BankName asc", con);
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

        public DataTable GetBankLayoutID(int BankID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetBankLayoutID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@@LocationID", @LocationID);
            cmd.Parameters.AddWithValue("@CityID", BankID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
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
        public DataSet GetAccountNameByBankID(string BankID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAccountByBankID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BankID", BankID);
                //cmd.Parameters.AddWithValue("",);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet Layout(string ChequeLayoutID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetChequeLayout", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ChequeLayoutID", ChequeLayoutID);
                //cmd.Parameters.AddWithValue("",);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public int DeleteCheckBook(int ChequeBookID)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeleteChequeBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ChequeBookID", ChequeBookID);

                con.Open();
                i = cmd.ExecuteNonQuery();

                con.Close();
            }

            return i;
        }


    }
}