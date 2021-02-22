using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_FinancialYear
    {
        int HospitalID;
        int LocationID;
        int UserID;

        private SqlConnection con;

        List<FinancialYear> financialyear = new List<FinancialYear>();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public void HospitlLocationID()
        {

            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public List<FinancialYear> SelectAllData()
        {
            Connect();
            HospitlLocationID();

            SqlCommand cmd = new SqlCommand("GetAllFinancialYear", con);
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
                financialyear.Add(
                    new FinancialYear
                    {
                        FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]),
                        FinancialYears = Convert.ToString(dr["FinancialYear"]),

                    });
            }
            return financialyear;
        }

        public bool CheckFinancialYear(int FinancialYearID, string FinancialYears)
        {
            Connect();
            HospitlLocationID();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckFinancialYear", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
                cmd.Parameters.AddWithValue("@FinancialYear", FinancialYears.ToUpper());
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

        public bool Save(FinancialYear obj)
        {
            Connect();
            HospitlLocationID();
            SqlCommand cmd = new SqlCommand("IUFinancialYear", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.FinancialYearID == 0)
            {
                cmd.Parameters.AddWithValue("@FinancialYearID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@FinancialYear", obj.FinancialYears.ToUpper());


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

        public List<FinancialYear> GetFinancialYear(int FinancialYearID)
        {
            Connect();
            HospitlLocationID();
            
            SqlCommand cmd = new SqlCommand("GetFinancialYear", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
           
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                financialyear.Add(
                    new FinancialYear
                    {
                        FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]),
                        FinancialYears = Convert.ToString(dr["FinancialYear"]),

                    });
            }
            return financialyear;
        }

    }
}