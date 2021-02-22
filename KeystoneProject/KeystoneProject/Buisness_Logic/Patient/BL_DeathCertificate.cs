using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Patient;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_DeathCertificate
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

        #region GetAllPatientDeathCertificate
        public DataSet GetAllPatientDeathCertificate()
        {
            Connect();
            DataSet ds = new DataSet();
           
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllPatientDeathCertificate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID" , HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);             
                 con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch(Exception)
            {
                return ds;     
            }
        }
        #endregion

        #region Save and Edit

        public bool Save(DeathCertificate death)
        {
            Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("IUPatientDeathCertificate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNo", death.PatientRegNo);
            if (death.PatientDeathCertificateID == 0)
            {

                cmd.Parameters.AddWithValue("@PatientDeathCertificateID", 0);
                
                cmd.Parameters["@PatientDeathCertificateID"].Direction = ParameterDirection.Output;
              
                cmd.Parameters.AddWithValue("Mode", "ADD");
            }
            else
            {
                cmd.Parameters.AddWithValue("@PatientDeathCertificateID", death.PatientDeathCertificateID);
                cmd.Parameters.AddWithValue("Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ReferenceCode", ReffrenceCode());
            cmd.Parameters.AddWithValue("@DateOfDeath",death.DateOfDeath);
            //cmd.Parameters.AddWithValue("@DateOfDeath", DateTime.Parse(death.DateOfDeath));
            cmd.Parameters.AddWithValue("@DeathType", death.DeathType);
            cmd.Parameters.AddWithValue("@ReasonOfDeath", death.ReasonOfDeath);
            cmd.Parameters.AddWithValue("@TimeOfDeath", death.TimeOfDeath);

            if (death.Discription != null)
            {
                cmd.Parameters.AddWithValue("@Discription", death.Discription);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Discription", "");
            }
           // cmd.Parameters.AddWithValue("@Address", death.Address);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            int i = cmd.ExecuteNonQuery();
            death.PatientDeathCertificateID = Convert.ToInt32(cmd.Parameters["@PatientDeathCertificateID"].Value);

            con.Close();
            if(i>0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Get PatientRegNo
        public DataSet GetPatientRegNo(int PatientRegNo)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientDeathCertificate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        #endregion

        #region Get PatientName
        public DataSet GetPatientName(string PatientName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNo, PatientName,GuardianName,Address from Patient where PatientName like '" + PatientName + '%' + "' and HospitalID = '" + HospitalID + "' and LocationID ='" + LocationID + "' and RowStatus = 0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        #endregion

        #region ReffrenceCode
        string HSNCode = "";
        public string ReffrenceCode()
        {
            HSNCode += DateTime.Now.ToString("DD");
            HSNCode += DateTime.Now.ToString("MM");
            HSNCode += DateTime.Now.ToString("hh");
            HSNCode += DateTime.Now.ToString("mm");
            HSNCode += DateTime.Now.ToString("ss");
            return HSNCode;
        }
        #endregion
        public int DeletePatientDeathCertificate(int PatientDeathCertificateID)
        {
            Connect();
            int delete = 0;
            SqlCommand cmd = new SqlCommand("DeletePatientDeathCertificate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientDeathCertificateID", PatientDeathCertificateID);
          
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
        }
        public bool CheckPatientDeathCertificate(int HospitalID, int PatientRegNo)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckPatientDeathCertificate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
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

        public DataSet GetAllFinancialYear()
        {

            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllFinancialYear", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);


                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }

        }

    }


       
}