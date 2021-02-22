using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.Master;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data.SqlClient;
//using System.Data;
//using KeystoneProject.Models.Patient;
//using System.Configuration;
//using Microsoft.ApplicationBlocks.Data;
//using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_CustomTranslation
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<CustomTranslation> CustomTranslationList = new List<CustomTranslation>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public bool Save(CustomTranslation obj)
        {
            Connect();
            try
            {

                SqlCommand cmd = new SqlCommand("IUCustomTranslation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                if (obj.TextID == 0)
                {
                    cmd.Parameters.AddWithValue("@TextID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TextID", obj.TextID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }
                if (obj.Text ==null)
                {
                    cmd.Parameters.AddWithValue("@Text", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Text", obj.Text);
                }

                if (obj.Hindi ==null)
                {
                    cmd.Parameters.AddWithValue("@Hindi", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Hindi", obj.Hindi);
                }
                if (obj.Marathi ==null)
                {
                    cmd.Parameters.AddWithValue("@Marathi", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Marathi", obj.Marathi);
                }
                if (obj.Tamil == null)
                {
                    cmd.Parameters.AddWithValue("@Tamil", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Tamil",obj.Tamil);
                }
                if (obj.Bengali == null) 
                {
                    cmd.Parameters.AddWithValue("@Bengali", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Bengali", obj.Bengali);
                }
                if (obj.Gujarati == null) 
                {
                    cmd.Parameters.AddWithValue("@Gujarati", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Gujarati", obj.Gujarati);
                }

                if (obj.Urdu == null)
                {
                    cmd.Parameters.AddWithValue("@Urdu", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Urdu", obj.Urdu);
                }

                if (obj.Telugu == null)
                {
                    cmd.Parameters.AddWithValue("@Telugu", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Telugu", obj.Telugu);
                }

                if (obj.Kannada == null)
                {
                    cmd.Parameters.AddWithValue("@Kannada", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Kannada", obj.Kannada);
                }

                if (obj.Malayalam == null)
                {
                    cmd.Parameters.AddWithValue("@Malayalam", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Malayalam", obj.Malayalam);
                }
                if (obj.Arabic == null)
                {
                    cmd.Parameters.AddWithValue("@Arabic", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Arabic", obj.Arabic);
                }

                if (obj.Odia == null)
                {
                    cmd.Parameters.AddWithValue("@Odia", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Odia", obj.Odia);
                }
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
            catch (Exception)
            {
                return false;
            }

        }

        public List<CustomTranslation> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllCustomTranslation", con);
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
                CustomTranslationList.Add(
                    new CustomTranslation
                    {
                        TextID = Convert.ToInt32(dr["TextID"]),
                        Text = Convert.ToString(dr["Text"]),
                        Hindi = Convert.ToString(dr["Hindi"]),
                        Marathi = Convert.ToString(dr["Marathi"]),
                        Gujarati = Convert.ToString(dr["Gujarati"]),                   
                        Tamil = Convert.ToString(dr["Tamil"]),
                        Bengali = Convert.ToString(dr["Bengali"]),
                        Telugu = Convert.ToString(dr["Telugu"]),                       
                        Urdu = Convert.ToString(dr["Urdu"]),
                        Arabic = Convert.ToString(dr["Arabic"]),
                    });
            }
            return CustomTranslationList;
        }

        public bool CheckCustomTranslation(CustomTranslation objCustomTranslation)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckCustomTranslation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@TextID",objCustomTranslation. TextID);
                cmd.Parameters.AddWithValue("@Text",objCustomTranslation.Text.ToUpper());
          
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

     
        public List<CustomTranslation> GetPatientPrivilegeCard(int TextID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetCustomTranslation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TextID", TextID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                CustomTranslationList.Add(
                    new CustomTranslation
                    {
                        // PatientPrivilegeCardID = Convert.ToInt32(dr["PatientPrivilegeCardID"]),
                        // PatientName = Convert.ToString(dr["PatientName"]),
                        //  AdviceDescription = Convert.ToString(dr["AdviceDescription"]),
                        TextID = Convert.ToInt32(dr["TextID"]),
                        Text = Convert.ToString(dr["Text"]),
                        Hindi = Convert.ToString(dr["Hindi"]),
                        Marathi = Convert.ToString(dr["Marathi"]),
                        Gujarati = Convert.ToString(dr["Gujarati"]),
                        Tamil = Convert.ToString(dr["Tamil"]),
                        Bengali = Convert.ToString(dr["Bengali"]),
                        Telugu = Convert.ToString(dr["Telugu"]),
                        Urdu = Convert.ToString(dr["Urdu"]),
                        Arabic = Convert.ToString(dr["Arabic"]),


                    });
            }
            return CustomTranslationList;
        }

        public bool Delete(int TextID)
        {
            Connect();
            SqlParameter[] apram = new SqlParameter[2];
            apram[0] = new SqlParameter("@TextID", SqlDbType.Int);
            apram[0].Value = TextID;
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeleteCustomTranslation", apram);
            con.Close();

            return true;

        }
    }
}