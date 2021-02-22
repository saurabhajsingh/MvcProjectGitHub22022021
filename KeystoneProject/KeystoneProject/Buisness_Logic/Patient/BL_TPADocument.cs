using KeystoneProject.Models.Patient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_TPADocument
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationId = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<TPADocument> li_tpa = new List<TPADocument>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataTable Bind_detail(int regno)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetPatientTpaDocument", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationId);
            cmd.Parameters.AddWithValue("@PatientRegNO", regno);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataSet Bind_letter(int registrationNumber)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetAllOrganizationTemplate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationId);
            cmd.Parameters.AddWithValue("@PatientRegNO", registrationNumber);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetParaeter()
        {
                Connect();
                SqlCommand cmd = new SqlCommand("GetMasterOrganizationTemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                con.Open();
                da.Fill(ds);
                con.Close();
                return ds;
           
        }

        public bool Save(TPADocument obj)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("IUtpadocument", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);

                if (obj.DocumentID == 0)
                {
                    cmd.Parameters.AddWithValue("@TpaDocumentID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "ADD");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TpaDocumentID", obj.DocumentID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                cmd.Parameters.AddWithValue("@Naration", obj.narration);
                cmd.Parameters.AddWithValue("@PatientRegNo", obj.registrationNumber);
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TPADocument> Bind_OldDoc(int regno)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("Gettpadocument", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                cmd.Parameters.AddWithValue("@PatientRegNO", regno);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    li_tpa.Add(
                        new TPADocument
                        {
                            oldDocument = Convert.ToInt32(dr["TpaDocumentID"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return li_tpa;
        }

        public DataTable Bind_Narration(int regno)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("Gettpadocument", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationId);
            cmd.Parameters.AddWithValue("@PatientRegNO", regno);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }

    }
}