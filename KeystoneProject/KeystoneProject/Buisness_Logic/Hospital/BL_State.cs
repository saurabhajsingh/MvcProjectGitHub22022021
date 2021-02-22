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
    public class BL_State
    {
        int HospitalID = 0;
        int LocationID = 0;

        int UserID = 0;
        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        }

        public bool Save(State obj)
        {
            Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("IUState", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.StateID == 0)
            {
                cmd.Parameters.AddWithValue("StateID", 0);
                cmd.Parameters.AddWithValue("Mode", "ADD");
            }
            else
            {
                cmd.Parameters.AddWithValue("StateID", obj.StateID);
                cmd.Parameters.AddWithValue("Mode", "Edit");
            }
            if (obj.StateName != null)
            {
                cmd.Parameters.AddWithValue("@StateName", obj.StateName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@StateName", "");
            }
            cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
            cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
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

        public DataSet SelectAllData()
        {

            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllState", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }

        public DataSet GetCountry(string GetCountry)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select CountryID,CountryName,ReferenceCode from Country where CountryName  like '" + @GetCountry + "%' and  HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "'", con);
            cmd.Parameters.AddWithValue("@GetCountry", GetCountry);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetState(int StateID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetState", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@StateID", StateID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteState(int StateID)
        {
            Connect();
            int delete = 0;
            SqlCommand cmd = new SqlCommand("DeleteState", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@StateID", StateID);
            cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
            cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
        }

        public bool CheckState(int StateID, string StateName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckState", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@StateID", StateID);
                cmd.Parameters.AddWithValue("@StateName", StateName);
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