using System;
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
    public class BL_DiscountReason
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

        public DataSet SelectAllData()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllDiscountReason", con);
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

        public bool CheckDiscountReason(int DiscountReasonID, string DiscountReason)
        {
            Connect();
            string Table;
            bool flag;
            SqlCommand cmd = new SqlCommand("CheckDiscountReason", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DiscountReasonID", DiscountReasonID);
            cmd.Parameters.AddWithValue("@DiscountReason", DiscountReason.ToUpper().ToString());
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

        public string save(DiscountReason obj)
        {
            Connect();
            try
            { 
            SqlCommand cmd = new SqlCommand("IUDiscountReason", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.DiscountReasonID==0)
            {
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@DiscountReasonID", obj.DiscountReasonID);
            cmd.Parameters.AddWithValue("@DiscountReason", obj.DiscountReasonName);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
           
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                obj.Message = "Save Successfully";
            }
            }
               catch(Exception ex)
              {
                  obj.Message = ex.Message;
              }
            return obj.Message;
        }

        public DataSet SelectDataByID(int DiscountReasonID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetDiscountReason", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@HospitalID", 1);
                //cmd.Parameters.AddWithValue("@LocationID", 1);
                cmd.Parameters.AddWithValue("@DiscountReasonID", DiscountReasonID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }

        public bool Edit(DiscountReason obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUDiscountReason", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DiscountReasonID", obj.DiscountReasonID);
            cmd.Parameters.AddWithValue("@DiscountReason", obj.DiscountReasonName.ToUpper());
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            cmd.Parameters.AddWithValue("@Mode", "Edit");
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

        public string Delete(int DiscountReasonID)
        {
            Connect();
            string Result = null;
            try
            {
                SqlCommand cmd = new SqlCommand("DeleteDiscountReason", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DiscountReasonID", DiscountReasonID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                Result = cmd.ExecuteNonQuery().ToString();
                con.Close();
                return Result;
            }
            catch (Exception)
            {
                return Result;
            }

        }
    }
}