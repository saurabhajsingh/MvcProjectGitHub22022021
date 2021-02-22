using KeystoneProject.Models.MasterLaboratory;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MasterLaboratory
{
    public class BL_Header
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<Header> headerlist = new List<Header>();
        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        public List<Header> SelectAllHeader()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllHeader", con);
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
                headerlist.Add(
                    new Header
                    {
                        HeaderID = Convert.ToString(dr["HeaderID"]),
                        HeaderName = Convert.ToString(dr["HeaderName"])
                       
                    });
            }
            return headerlist;


        }

        public List<Header> GetHeader(int HeaderID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetHeader", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HeaderID", HeaderID));
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                headerlist.Add(
                    new Header
                    {
                        HeaderID =dr["HeaderID"].ToString(),
                        HeaderName = Convert.ToString(dr["HeaderName"])
                       
                    });
            }
            return headerlist;

        }
        public bool Header(Header obj)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("IUHeader", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.HeaderID == "0" || obj.HeaderID== null)
            {
                cmd.Parameters.AddWithValue("@HeaderID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@HeaderID", obj.HeaderID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }


            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            
            cmd.Parameters.AddWithValue("@HeaderName", obj.HeaderName);

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


        public bool CheckHeader(string HeaderID, string HeaderName)
        {
            string t = "";
            if (HeaderID == null || HeaderID == "")
            {
                t = "0";
            }
            else
            {
                t = HeaderID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckHeader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@HeaderID", t);
                cmd.Parameters.AddWithValue("@HeaderName", HeaderName.ToUpper());
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
        public bool DeleteHeader(int HeaderID)
        {

            Connect();
            SqlParameter[] apram = new SqlParameter[4];
            apram[0] = new SqlParameter("@HeaderID", SqlDbType.Int);
            apram[0].Value = HeaderID;
            apram[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
            apram[1].Value = HospitalID;
            apram[2] = new SqlParameter("@LocationID", SqlDbType.Int);
            apram[2].Value = LocationID;
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeleteHeader", apram);
            con.Close();

            return true;
        }

    }
}