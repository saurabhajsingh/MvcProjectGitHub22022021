using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using KeystoneProject.Models.Master;
using System.Data;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_PatientPrefix
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<PatientPrefix> SelectAllData()
        {
            Connect();
            List<PatientPrefix> PrefixList = new List<PatientPrefix>();

            SqlCommand cmd = new SqlCommand("GetAllPatientPrefix", con);
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
                PrefixList.Add(
                    new PatientPrefix
                    {
                        PrefixID = Convert.ToInt32(dr["PrefixID"]),
                        PrefixName = Convert.ToString(dr["PrefixName"]),
                        Gender=dr["Gender"].ToString(),

                    });
            }
            return PrefixList;
        }

        public PatientPrefix GetPrefix(int PrefixID)
        {
            Connect();
            PatientPrefix prefix = new PatientPrefix();

            SqlCommand cmd = new SqlCommand("GetPrefixName", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PrefixID", PrefixID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                prefix.PrefixID = Convert.ToInt32(dr["PrefixID"]);
                prefix.PrefixName = Convert.ToString(dr["PrefixName"]);

            }

            return prefix;
        }

        public bool Save(PatientPrefix obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUPatientPrefix", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.PrefixID == 0)
            {
                cmd.Parameters.AddWithValue("@PrefixID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@PrefixID", obj.PrefixID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@PrefixName", obj.PrefixName);
            cmd.Parameters.AddWithValue("@Gender", obj.Gender);
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

        public int DeletePatientPrefix(int PrefixID)
        {
            Connect();
            int delete = 0;
            PatientPrefix search = new PatientPrefix();
            SqlCommand cmd = new SqlCommand("DeletePatientPrefix", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PrefixID", PrefixID);
            con.Open();
            delete = cmd.ExecuteNonQuery();

            return delete;
        }
        public bool CheckPatientPrefix(int PrefixID, string PrefixName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckPatientPrefix", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@PrefixID", PrefixID);
                cmd.Parameters.AddWithValue("@PrefixName", PrefixName.ToUpper());
               
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
       
    }
}