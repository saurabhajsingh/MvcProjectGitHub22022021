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
    public class BL_Drug
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

        public bool Save(Drug obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUDrug", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            // cmd.Parameters.AddWithValue("@QualifictionID", 0);
            if (obj.DrugID == 0)
            {
                cmd.Parameters.AddWithValue("@DrugID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@DrugID", obj.DrugID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ReferenceCode", 1);
            cmd.Parameters.AddWithValue("@DrugName", obj.DrugName);

            cmd.Parameters.AddWithValue("@DrugDescription", obj.DrugDescription);


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
        public DataSet GetAllDrug(int QualifictionID)
        {
            SqlConnection con = null;
            DataSet ds = null;
            try
            {
                Connect();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("GetAllDrug", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .       
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
    }
}