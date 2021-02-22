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
    public class BL_City
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
        public bool Save(City obj)
        {
            Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("IUCity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.CityID == 0)
            {
                cmd.Parameters.AddWithValue("@CityID", 0);
                cmd.Parameters.AddWithValue("Mode", "ADD");
            }
            else
            {
                cmd.Parameters.AddWithValue("@CityID", obj.CityID);
                cmd.Parameters.AddWithValue("Mode","Edit");
            }
            if(obj.CityName!= null)
            {
                cmd.Parameters.AddWithValue("@CityName", obj.CityName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CityName","");
            }
            if (obj.StateID != null)
            {
                cmd.Parameters.AddWithValue("@StateID", obj.StateID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@StateID", "");
            }
            if (obj.CountryID != null)
            {
                cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CountryID", "");
            }
           
            cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if(i>0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet SelectAllCity()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Select CityID,CityName, ReferenceCode  from City where  RowStatus = 0 and HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "'", con);
              //  cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch(Exception)
            {
                return ds;
            }
        }



        public DataSet GetState(string StateName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select State.StateID,State.StateName,Country.CountryID, Country.CountryName from State inner join Country on Country.CountryID = State.CountryID where  StateName like ''+@StateName+'%' and State.HospitalID = '" + HospitalID + "' and State.LocationID = '" + LocationID + "' and State.RowStatus= 0", con);
            cmd.Parameters.AddWithValue("@StateName", StateName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetCity(int CityID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            { 
                SqlCommand cmd = new SqlCommand("GetCity",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@CityID", CityID);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public int DeleteCity(int CityID)
        {
            Connect();
            int delete = 0;
            SqlCommand cmd = new SqlCommand("DeleteCity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CityID", CityID);

            cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
            cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
        }

        public bool CheckCity(int CityID , string CityName)
        {
            Connect();
            string Table;
            bool Flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckCity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@CityID",CityID);
                cmd.Parameters.AddWithValue("@CityName", CityName);

                cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@NameExists"].Value;
                if(Convert.ToInt32(Table) ==1)
                {
                    Flag = false;
                }
                else
                {
                    Flag = true;
                }

                return Flag;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}