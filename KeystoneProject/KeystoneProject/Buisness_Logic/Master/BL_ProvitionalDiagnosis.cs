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
    public class BL_ProvitionalDiagnosis
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<ProvitionalDiagnosis> ProvitionalDiagnosisList = new List<ProvitionalDiagnosis>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }


        public List<ProvitionalDiagnosis> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllProvitionalDiagnosis", con);
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
                ProvitionalDiagnosisList.Add(
                    new ProvitionalDiagnosis
                    {
                        ProvitionalDiagnosisID = Convert.ToInt32(dr["ProvitionalDiagnosisID"]),
                        ProvitionalDiagnosisName = Convert.ToString(dr["ProvitionalDiagnosisName"]),
                        ProvitionalDiagnosisDescription = Convert.ToString(dr["ProvitionalDiagnosisDescription"]),
                    });
            }
            return ProvitionalDiagnosisList;
        }


        public List<ProvitionalDiagnosis> GetProvitionalDiagnosis(int ProvitionalDiagnosisID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetProvitionalDiagnosis", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProvitionalDiagnosisID", ProvitionalDiagnosisID));
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                ProvitionalDiagnosisList.Add(
                    new ProvitionalDiagnosis
                    {
                        ProvitionalDiagnosisID = Convert.ToInt32(dr["ProvitionalDiagnosisID"]),
                        ProvitionalDiagnosisName = Convert.ToString(dr["ProvitionalDiagnosisName"]),
                        ProvitionalDiagnosisDescription = Convert.ToString(dr["ProvitionalDiagnosisDescription"]),
                    });
            }
            return ProvitionalDiagnosisList;
        }



        public bool Save(ProvitionalDiagnosis obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUProvitionalDiagnosis", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.ProvitionalDiagnosisID == 0)
            {
                cmd.Parameters.AddWithValue("@ProvitionalDiagnosisID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ProvitionalDiagnosisID", obj.ProvitionalDiagnosisID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ProvitionalDiagnosisName", obj.ProvitionalDiagnosisName);
            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            if (obj.ProvitionalDiagnosisDescription == null)
                cmd.Parameters.AddWithValue("@ProvitionalDiagnosisDescription", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ProvitionalDiagnosisDescription", obj.ProvitionalDiagnosisDescription);
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


        public bool CheckProvitionalDiagnosis(int ProvitionalDiagnosisID, string ProvitionalDiagnosisName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckProvitionalDiagnosis", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ProvitionalDiagnosisID", ProvitionalDiagnosisID);
                cmd.Parameters.AddWithValue("@ProvitionalDiagnosisName", ProvitionalDiagnosisName.ToUpper());
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


        public string Delete(int ProvitionalDiagnosisID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteProvitionalDiagnosis", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProvitionalDiagnosisID", ProvitionalDiagnosisID);

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