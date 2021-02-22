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
    public class BL_TPADocumentMaster
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationId = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<TPADocumentMaster> LiTPA = new List<TPADocumentMaster>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<TPADocumentMaster> Getdata()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetMasterOrganizationTemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    LiTPA.Add(
                        new TPADocumentMaster
                        {
                            ParameterName = Convert.ToString(dr["PARAMETER"]),                            
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return LiTPA;
        }

        public List<TPADocumentMaster> Add_Parameter(string parameter)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetMedicalAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                cmd.Parameters.AddWithValue("@AccountsID", parameter);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    LiTPA.Add(
                        new TPADocumentMaster
                        {
                            ParameterName = Convert.ToString(dr["PARAMETER"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return LiTPA;
        }

        public List<TPADocumentMaster> GetTpaLetter()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllOrganizationTemplateDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    LiTPA.Add(
                        new TPADocumentMaster
                        {
                            DocumentName = Convert.ToString(dr["Naration"]),
                            LetterID = Convert.ToInt32(dr["OrganizationTemplateID"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return LiTPA;
        }        

        public bool Save(TPADocumentMaster obj)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("IUOrganizationTemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);

                if (obj.LetterID == 0)
                {
                    cmd.Parameters.AddWithValue("@OrganizationTemplateID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OrganizationTemplateID", obj.LetterID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                cmd.Parameters.AddWithValue("@Naration", obj.DocumentID);
                cmd.Parameters.AddWithValue("@Naration1", "");
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

        public int Deletedata(int LetterID)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeleteOrganizationTemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                cmd.Parameters.AddWithValue("@OrganizationTemplateID", LetterID);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();

            }
            return i;
        }


    }
}