using KeystoneProject.Models.Master;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_SampleType
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<SampleType> sampleTypelist = new List<SampleType>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<SampleType> SelectAllSampleType()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllSampleType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                sampleTypelist.Add(
                    new SampleType
                    {
                        SampleTypeID = Convert.ToInt32(dr["SampleTypeID"]),
                        SampleTypeName = Convert.ToString(dr["SampleType"]),
                  });
            }
            return sampleTypelist;
        }

        public List<SampleType> GetSampleType(int SampleTypeID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetSampleType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@SampleTypeID", SampleTypeID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                sampleTypelist.Add(
                    new SampleType
                    {
                        SampleTypeID = Convert.ToInt32(dr["SampleTypeID"]),
                        SampleTypeName = Convert.ToString(dr["SampleType"]),           
                    });
            }
            return sampleTypelist;

        }

        public bool SampleType(SampleType obj)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("IUSampleType", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);


            if (obj.SampleTypeID == 0)
           {
               cmd.Parameters.AddWithValue("@SampleTypeID", 0);
               cmd.Parameters.AddWithValue("@Mode", "Add");
           }
           else
           {
               cmd.Parameters.AddWithValue("@SampleTypeID", obj.SampleTypeID);
               cmd.Parameters.AddWithValue("@Mode", "Edit");
           }

            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
              cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);

              cmd.Parameters.AddWithValue("@SampleType", obj.SampleTypeName);

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

        public bool CheckSampleType(int SampleTypeID, string SampleType)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckSampleType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@SampleTypeID", SampleTypeID);
                cmd.Parameters.AddWithValue("@SampleType", SampleType.ToUpper());
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

        public bool DeleteSampleType(int SampleTypeID)
        {


            Connect();
            SqlParameter[] apram = new SqlParameter[4];
            apram[0] = new SqlParameter("@SampleTypeID", SqlDbType.Int);
            apram[0].Value = SampleTypeID;
            apram[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
            apram[1].Value = HospitalID;
            apram[2] = new SqlParameter("@LocationID", SqlDbType.Int);
            apram[2].Value = LocationID;
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeleteSampleType", apram);
            con.Close();

            return true;
        }

    }
}