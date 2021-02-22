using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.PharmacyMaster;
using System.Data;

namespace KeystoneProject.Buisness_Logic.PharmacyMaster
{
    public class BL_GenericInformation
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);

        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //int CreationID = Convert.ToInt32(HttpContext.Current.Session["CreationID"]);

        List<GenericInformation> GenericInformationlist= new List<GenericInformation>();



        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        public List<GenericInformation> SelectAllData()
        {
             Connect();


            SqlCommand cmd = new SqlCommand("GetAllGenericInformation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID",LocationID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                GenericInformationlist.Add(
                    new GenericInformation
                    {
                        GenericName = (dr["GenericName"].ToString()),
                        GenericID = Convert.ToInt32(dr["GenericID"]),

                    });
            }
            return GenericInformationlist;
        }
        public List<GenericInformation> GetGenericInformation(string GenericID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetGenericInformation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@GenericID", GenericID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                GenericInformationlist.Add(
                    new GenericInformation
                    {
                        GenericName = (dr["GenericName"].ToString()),
                        GenericID = Convert.ToInt32(dr["GenericID"]),

                    });
            }
            return GenericInformationlist;
        }
        public bool Save(GenericInformation obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUGenericInformation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
          

            if (obj.GenericID == 0)
            {
                cmd.Parameters.AddWithValue("@GenericID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@GenericID", obj.GenericID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@GenericName", obj.GenericName);
            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
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
        public bool CheckGenericInformation(int GenericID, string GenericName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckGenericInformation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@GenericID", GenericID);
                cmd.Parameters.AddWithValue("@GenericName", GenericName.ToUpper());

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
            catch (Exception ex)
            {
                return false;
            }
        }
        public int DeleteGenericInformation(int GenericID)
        {
            Connect();
            int delete = 0;
            SqlCommand cmd = new SqlCommand("DeleteGenericInformation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@GenericID", GenericID);
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
           
        }


    }
}