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
    public class Bl_Region
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


        public bool Save(Region obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IURegion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            // cmd.Parameters.AddWithValue("@QualifictionID", 0);
            if (obj.RegionID == 0)
            {
                cmd.Parameters.AddWithValue("@RegionID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@RegionID", obj.RegionID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ReferenceCode", 1);
            cmd.Parameters.AddWithValue("@Region", obj.RegionName);

            cmd.Parameters.AddWithValue("@Area", obj.Area);


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
    

        public DataSet GetAllRegion()
        {
            Connect();

            Region search = new Region();
            SqlCommand cm = new SqlCommand("GetAllRegion", con);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@HospitalID", HospitalID);
            cm.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cm;
            DataSet ds = new DataSet();
            ad.Fill(ds);



            return ds;
        }

        public int DeleteRegion(int RegionID)
        {
            Connect();
            int delete = 0;
            Region search = new Region();
            SqlCommand cm = new SqlCommand("DeleteRegion", con);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@HospitalID", HospitalID);
            cm.Parameters.AddWithValue("@LocationID", LocationID);
            cm.Parameters.AddWithValue("@RegionID", RegionID);
            con.Open();
            delete = cm.ExecuteNonQuery();


            return delete;
        }
    }
}