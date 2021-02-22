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
    public class BL_ChiefHistory
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<ChiefHistory> ChiefHistoryList = new List<ChiefHistory>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }


        public List<ChiefHistory> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllChiefHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                ChiefHistoryList.Add(
                    new ChiefHistory
                    {
                        ChiefHistoryID = Convert.ToInt32(dr["ChiefHistoryID"]),
                        ChiefHistoryName = Convert.ToString(dr["ChiefHistoryName"]),
                        ChiefHistoryDescription = Convert.ToString(dr["ChiefHistoryDescription"]),
                    });
            }
            return ChiefHistoryList;
        }


        public List<ChiefHistory> GetChiefHistory(int ChiefHistoryID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetChiefHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ChiefHistoryID", ChiefHistoryID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                ChiefHistoryList.Add(
                    new ChiefHistory
                    {
                        ChiefHistoryID = Convert.ToInt32(dr["ChiefHistoryID"]),
                        ChiefHistoryName = Convert.ToString(dr["ChiefHistoryName"]),
                        ChiefHistoryDescription = Convert.ToString(dr["ChiefHistoryDescription"]),
                    });
            }
            return ChiefHistoryList;
        }



        public bool Save(ChiefHistory obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUChiefHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.ChiefHistoryID == 0)
            {
                cmd.Parameters.AddWithValue("@ChiefHistoryID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ChiefHistoryID", obj.ChiefHistoryID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ChiefHistoryName", obj.ChiefHistoryName);
            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            if (obj.ChiefHistoryDescription == null)
                cmd.Parameters.AddWithValue("@ChiefHistoryDescription", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ChiefHistoryDescription", obj.ChiefHistoryDescription);
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


        public bool CheckChiefHistory(int ChiefHistoryID, string ChiefHistoryName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckChiefHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ChiefHistoryID", ChiefHistoryID);
                cmd.Parameters.AddWithValue("@ChiefHistoryName", ChiefHistoryName.ToUpper());
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


        public string Delete(int ChiefHistoryID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteChiefHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChiefHistoryID", ChiefHistoryID);

                con.Open();
                int i = cmd.ExecuteNonQuery();

                con.Close();
                if (i > 0)
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
                return Table;
            }
        }
    }
}