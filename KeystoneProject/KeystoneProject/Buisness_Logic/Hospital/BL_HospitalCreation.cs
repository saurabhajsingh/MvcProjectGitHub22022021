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
    public class BL_HospitalCreation
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);     
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        int HospitalName = Convert.ToInt32(HttpContext.Current.Session["HospitalName"]);
        int ReferenceCode = Convert.ToInt32(HttpContext.Current.Session["ReferenceCode"]);  
        private SqlConnection con;

        public void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["MyCon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet SelectAllData()
       {

            Connect();
            DataSet ds = null;
            try
            {
             SqlCommand cmd = new SqlCommand("GetAllHospital", con); 
             cmd.CommandType = CommandType.StoredProcedure;
             con.Open();
             SqlDataAdapter da = new SqlDataAdapter();
             da.SelectCommand = cmd;
             ds = new DataSet();
             da.Fill(ds);
             con.Close();
             return ds;
            }
            catch(Exception )
            {
                return ds;
            }    
        }
        public bool Save(HospitalCreation obj)
        {
            Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("IUHospital", con);
            cmd.CommandType = CommandType.StoredProcedure;
            
            if (obj.HospitalID == 0)
            {
                cmd.Parameters.AddWithValue("@HospitalID", 0);
                cmd.Parameters.AddWithValue("@Mode", "ADD");
            }
            else
            {
                cmd.Parameters.AddWithValue("@HospitalID", obj.HospitalID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            if (obj.HospitalName != null)
            {
                cmd.Parameters.AddWithValue("@HospitalName", obj.HospitalName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@HospitalName","");
            }
           
            if (obj.GroupName == null)
            {
                cmd.Parameters.AddWithValue("@GroupName", "");
            }
            else
            {
                cmd.Parameters.AddWithValue("@GroupName", obj.GroupName);
            }
             if(obj.ReferenceCode == null)
             {
              cmd.Parameters.AddWithValue("@ReferenceCode", "");
             }
            else
             {
                 cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
             }         
            if (obj.ManagingBody!= null)
            {
             cmd.Parameters.AddWithValue("@ManagingBody", obj.ManagingBody);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ManagingBody", "");
            }
            if (obj.Adminstrator != null)
           {
                cmd.Parameters.AddWithValue("@Adminstrator", obj.Adminstrator);
            }
            else
           {
               cmd.Parameters.AddWithValue("@Adminstrator", "");
           }
            if (obj.Address!= null)
            {
                cmd.Parameters.AddWithValue("@Address", obj.Address);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Address", "");
            }
            if (obj.Logo == null)
            {
                SqlParameter imageParameter = new SqlParameter("@Logo", SqlDbType.Image);
                imageParameter.Value = DBNull.Value;
                cmd.Parameters.Add(imageParameter);
              
            }
            else 
            {
                cmd.Parameters.AddWithValue("@Logo", obj.Logo);
            }        
            if(obj.CityID == null)
            {
                cmd.Parameters.AddWithValue("@CityID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CityID", obj.CityID);
            }
            if(obj.CountryID == null)
            {
                cmd.Parameters.AddWithValue("@CountryID", 0);
            }
            else
            {
               cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
            }
            if(obj.StateID== null)
            {
                cmd.Parameters.AddWithValue("@StateID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@StateID", obj.StateID);
            }
            if (obj.PinCode != null)
            {
                cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PinCode", "");
            }
            if (obj.PhoneNo != null)
            {
               cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNo);
            }
            else
            {
               cmd.Parameters.AddWithValue("@PhoneNo", "");
            }
            if (obj.PhoneNo1!= null)
            {
             cmd.Parameters.AddWithValue("@PhoneNo1", obj.PhoneNo1);
            }
            else
            {
             cmd.Parameters.AddWithValue("@PhoneNo1", "");
            }
            if (obj.FaxNo != null)
           {
                cmd.Parameters.AddWithValue("@FaxNo", obj.FaxNo);
           }
            else
           {
                cmd.Parameters.AddWithValue("@FaxNo", "");
           }
            if (obj.MobileNo != null)
           {
               cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
           }
            else
           {
               cmd.Parameters.AddWithValue("@MobileNo", "");
           }
            if (obj.EmailID != null)
            {
                  cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
            }
            else
            {
                  cmd.Parameters.AddWithValue("@EmailID", "");
            }
            if (obj.URL != null)
             {
              cmd.Parameters.AddWithValue("@URL", obj.URL);
             }
            else
             {
              cmd.Parameters.AddWithValue("@URL", "");
             }
            if (UserID != null)
           {
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            }
            else
           {
           
           }
        
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
        //public DataSet GetCountry(string GetCountry)
        //{
        //    Connect();
        //    SqlCommand cmd = new SqlCommand("Select CountryID,CountryName from Country where CountryName  like '"+@GetCountry+"%'", con);
        //    cmd.Parameters.AddWithValue("@GetCountry", GetCountry);
        //    DataSet ds = new DataSet();
        //    con.Open();
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(ds);
        //    con.Close();
        //    return ds;
        //}
        //public DataSet GetState(string GetState)
        //{
        //    Connect();
        //    SqlCommand cmd = new SqlCommand("Select StateID ,StateName from State where StateName like '"+@GetState+"%' " , con);
        //    cmd.Parameters.AddWithValue("@GetState", GetState);
        //    DataSet ds = new DataSet();
        //    con.Open();
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(ds);
        //    con.Close();
        //    return ds;
        //}
        public DataSet GetCity(string CityName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select City.CityID,City.CityName ,State.StateID ,State.StateName,Country.CountryID,Country.CountryName from City inner join State on State.StateID=City.StateID inner join Country on Country.CountryID=State.CountryID where  CityName like ''+@CityName+'%'", con);
           // SqlCommand cmd = new SqlCommand("Select CityID, CityName from City where CityName like '"+@GetCity+"%'  ", con);
            cmd.Parameters.AddWithValue("@CityName", CityName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetHospital(int HospitalID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetHospital", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
           catch(Exception)
            {
                throw;
            }
        }
        public bool CheckHospital(int HospitalID, string HospitalName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckHospital", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@HospitalName", HospitalName.ToUpper());
                cmd.Parameters.Add("@Exists", SqlDbType.NVarChar, 50);
                cmd.Parameters["@Exists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@Exists"].Value;
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
            catch(Exception)
            {
                return false;
            }
        }

        public int DeleteHospital(int HospitalID)
        {
           
                Connect();
                int delete = 0;
                SqlCommand cmd = new SqlCommand("DeleteHospital", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
                cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
                con.Open();             
                delete = cmd.ExecuteNonQuery();
                return delete;
            
        }
    }
}