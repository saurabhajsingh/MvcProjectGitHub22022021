using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Master;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class Bl_Qualification
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }
        public bool Save(Qualification obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUQualification", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            // cmd.Parameters.AddWithValue("@QualifictionID", 0);
            if (obj.QualifictionID == 0)
            {
                cmd.Parameters.AddWithValue("@QualifictionID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@QualifictionID", obj.QualifictionID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ReferenceCode", 1);
            cmd.Parameters.AddWithValue("@QualifictionName", obj.QualifictionName);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            //  cmd.Parameters.AddWithValue("@Mode", "Add");
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
        public DataSet SelectAllDataQualification()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllQualification", con);
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
        public string DeleteQualification(int QualifictionID)
        {
            string Table = string.Empty;

            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteQualification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QualifictionID", QualifictionID);
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 500);
                cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@TableName"].Value;
                con.Close();
                if (i >= 1)
                {
                    return Table;
                }
                else
                {
                    return Table;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataSet SelectByEditMode(int QualifictionID)
        {
            SqlConnection con = null;
            DataSet ds = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("GetQualificationName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QualifictionID", QualifictionID); // i will pass zero to MobileID beacause its Primary .       
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
        public string UpdateData(Qualification ADocM)
        {
            SqlConnection con = null;
            string result = "";
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("IUQualification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@QualifictionID", ADocM.QualifictionID);
                cmd.Parameters.AddWithValue("@QualifictionName", ADocM.QualifictionName);
                cmd.Parameters.AddWithValue("@ReferenceCode", 1);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
                con.Open();
                cmd.ExecuteNonQuery();
                result = cmd.ToString();
                return result;
            }
            catch
            {
                return result = "";
            }
            finally
            {
                con.Close();
            }
        }

        public bool CheckQualification(string QualifictionName, int QualifictionID)
        {

            Connect();
            bool Table;
            bool flag;
            try
            {
                SqlCommand checkQualification = new SqlCommand("CheckQualification", con);
                checkQualification.CommandType = CommandType.StoredProcedure;
                checkQualification.Parameters.AddWithValue("@HospitalID", HospitalID);
                checkQualification.Parameters.AddWithValue("@LocationID", LocationID);
                checkQualification.Parameters.AddWithValue("@QualifictionID", QualifictionID);
                checkQualification.Parameters.AddWithValue("@QualifictionName", QualifictionName.Trim());
                checkQualification.Parameters.Add("@NameExists", SqlDbType.Bit, 50);
                checkQualification.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = checkQualification.ExecuteNonQuery();
                Table = (bool)checkQualification.Parameters["@NameExists"].Value;
                con.Close();
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