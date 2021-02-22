using KeystoneProject.Models.Keystone;
using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Hospital
{
    public class BL_Country
    {
       
        private SqlConnection Con;
        int HospitalID = 0;
        int LocationID = 0;

        int UserID = 0;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            Con = new SqlConnection(constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public bool Save(Country obj)
        {
            Connect();
            Con.Open();
            SqlCommand cmd = new SqlCommand("IUCountry", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.CountryID == 0)
            {
                cmd.Parameters.AddWithValue("@CountryID" ,0);
                cmd.Parameters.AddWithValue("Mode" ,"ADD");
            }
            else
            {
                cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
                cmd.Parameters.AddWithValue("Mode","Edit" );
            }
            if(obj.CountryName != null)
            {
                cmd.Parameters.AddWithValue("@CountryName", obj.CountryName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CountryName", "");
            }
            cmd.Parameters.AddWithValue("@ReferenceCode", ReffrenceCode());
            cmd.Parameters.AddWithValue("@ISDCode", obj.ISDCode);
            cmd.Parameters.AddWithValue("@DateFormat", "");
            cmd.Parameters.AddWithValue("@DateTime", "");
            cmd.Parameters.AddWithValue("@DateSeparator", "");
            cmd.Parameters.AddWithValue("@Century", "");
            cmd.Parameters.AddWithValue("@TimeSeparator", "");
            cmd.Parameters.AddWithValue("@Seconds", "");
            cmd.Parameters.AddWithValue("@BDName", "");
            cmd.Parameters.AddWithValue("@BDAbbreviation", "");
            cmd.Parameters.AddWithValue("@ADName", "");
            cmd.Parameters.AddWithValue("@ADAbbreviation", "");
            cmd.Parameters.AddWithValue("@LacsMillion", "");
            cmd.Parameters.AddWithValue("@NumericSeparator", "");
            cmd.Parameters.AddWithValue("@DecimalSeparator", "");
            cmd.Parameters.AddWithValue("@DecimalDigits", "");
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            int i = cmd.ExecuteNonQuery();
            Con.Close();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet SelectAllData()
        {

            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllCountry", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                Con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                Con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }
        
        public DataSet GetCountry(int CountryID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetCountry", Con);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@CountryID", CountryID);

                Con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                Con.Close();
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        string CN = "";
        public string ReffrenceCode()
        {
            CN += DateTime.Now.ToString("DD");
            CN += DateTime.Now.ToString("MM");
            CN += DateTime.Now.ToString("hh");
            CN += DateTime.Now.ToString("mm");
            CN += DateTime.Now.ToString("ss");
            return CN;
        }


        public int DeleteCountry(int CountryID)
        {
            Connect();
            int delete = 0;
            SqlCommand cmd = new SqlCommand("DeleteCountry", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
            cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
            Con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
        }

        public bool CheckCountry(int CountryID, string CountryName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckCountry", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@CountryID", CountryID);
                cmd.Parameters.AddWithValue("@CountryName", CountryName);
                cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                Con.Open();
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