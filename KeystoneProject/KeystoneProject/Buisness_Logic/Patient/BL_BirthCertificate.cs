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
    public class BL_BirthCertificate
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


        #region Get PatientRegNo
        public DataSet GetPatientRegNo(int PatientRegNo)
        {
            Connect();
            
            SqlCommand cmd = new SqlCommand("GetPatientBirthCertificate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            //cmd.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet Fill(int CertificateNo)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetPatientBirthCertificateDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CertificateNo", CertificateNo);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }


        #endregion


        public List<BirthCertificate> SelectAllData()
        {

            List<BirthCertificate> BirthCert = new List<BirthCertificate>();

            Connect();


            SqlCommand cmd = new SqlCommand("GetAllPatientBirthCertificate", con);
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
                BirthCert.Add(
                    new BirthCertificate
                    {
                        CertificateNo = Convert.ToInt32(dr["CertificateNo"]),
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNo"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                    });
            }
            return BirthCert;
        }
        #region Get PatientName
        public DataSet GetPatientName(string PatientName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PrintRegNO, PatientRegNo, PatientName,address from Patient where PatientName like '" + PatientName + '%' + "' and PatientType like 'IPD' and HospitalID = '" + HospitalID + "' and LocationID ='" + LocationID + "' and RowStatus = 0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        #endregion

        #region Save & Edit
        public bool Save(BirthCertificate birth)
        {
            Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("IUPatientBirthCertificate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (birth.CertificateNo == 0)
            {
                cmd.Parameters.AddWithValue("@CertificateNo", 0);

                cmd.Parameters["@CertificateNo"].Direction = ParameterDirection.Output;

                cmd.Parameters.AddWithValue("Mode", "ADD");

               
            }
            else
            {
                cmd.Parameters.AddWithValue("@CertificateNo", birth.CertificateNo);
                cmd.Parameters.AddWithValue("Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ReferenceCode", ReffrenceCode());
            cmd.Parameters.AddWithValue("@PatientRegNo", birth.PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientName", birth.PatientName);
            if (birth.Cast != null)
            {
                cmd.Parameters.AddWithValue("@Cast", birth.Cast);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Cast", "");
            }
            if (birth.Date != null)
            {
                cmd.Parameters.AddWithValue("@Date", birth.Date);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Date", "");
            }
            if (birth.MotherName != null)
            {
                cmd.Parameters.AddWithValue("@MotherName", birth.MotherName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MotherName", "");
            }
            if (birth.FatherName != null)
            {
                cmd.Parameters.AddWithValue("@FatherName", birth.FatherName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FatherName", "");
            }
           if (birth.Address != null)
            {
                cmd.Parameters.AddWithValue("@Address", birth.Address);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Address", "");
            }
           if (birth.MAge != null)
           {
               cmd.Parameters.AddWithValue("@MAge", birth.MAge);
           }
           else
           {
               cmd.Parameters.AddWithValue("@MAge", "");
           }
           if (birth.MNationality != null)
           {
               cmd.Parameters.AddWithValue("@MNationality", birth.MNationality);
           }
           else
           {
               cmd.Parameters.AddWithValue("@MNationality", "");
           }
           if (birth.MReligion != null)
           {
               cmd.Parameters.AddWithValue("@MReligion", birth.MReligion);
           }
           else
           {
               cmd.Parameters.AddWithValue("@MReligion", "");
           }
           if (birth.MQualification != null)
           {
               cmd.Parameters.AddWithValue("@MQualification", birth.MQualification);
           }
           else
           {
               cmd.Parameters.AddWithValue("@MQualification", "");
           }

           if (birth.MOccuption != null)
           {
               cmd.Parameters.AddWithValue("@MOccuption", birth.MOccuption);
           }
           else
           {
               cmd.Parameters.AddWithValue("@MOccuption", "");
           }
           if (birth.FAge != null)
           {
               cmd.Parameters.AddWithValue("@FAge", birth.FAge);
           }
           else
           {
               cmd.Parameters.AddWithValue("@FAge", "");
           }
           if (birth.FNationality != null)
           {
               cmd.Parameters.AddWithValue("@FNationality", birth.FNationality);
           }
           else
           {
               cmd.Parameters.AddWithValue("@FNationality", "");
           }
           if (birth.FReligion != null)
           {
               cmd.Parameters.AddWithValue("@FReligion", birth.FReligion);
           }
           else
           {
               cmd.Parameters.AddWithValue("@FReligion", "");
           }
           if (birth.FQualification != null)
           {
               cmd.Parameters.AddWithValue("@FQualification", birth.FQualification);
           }
           else
           {
               cmd.Parameters.AddWithValue("@FQualification", "");
           }
           if (birth.FOccuption != null)
           {
               cmd.Parameters.AddWithValue("@FOccuption", birth.FOccuption);
           }
           else
           {
               cmd.Parameters.AddWithValue("@FOccuption", "");
           }
           if (birth.DOB1 != null)
           {
               cmd.Parameters.AddWithValue("@DOB1", birth.DOB1);
           }
           else
           {
               cmd.Parameters.AddWithValue("@DOB1", "");
           }
           if (birth.DOB2 != null)
           {
               cmd.Parameters.AddWithValue("@DOB2", birth.DOB2);
           }
           else
           {
               cmd.Parameters.AddWithValue("@DOB2", DBNull.Value);
           }
           if (birth.DOB3 != null)
           {
               cmd.Parameters.AddWithValue("@DOB3", birth.DOB3);
           }
           else
           {
               cmd.Parameters.AddWithValue("@DOB3", DBNull.Value);
           }
           if (birth.DOB4 != null)
           {
               cmd.Parameters.AddWithValue("@DOB4", birth.DOB4);
           }
           else
           {
               cmd.Parameters.AddWithValue("@DOB4", DBNull.Value);
           }
           if (birth.TOB1 != null)
           {
               cmd.Parameters.AddWithValue("@TOB1", birth.TOB1);
           }
           else
           {
               cmd.Parameters.AddWithValue("@TOB1", DBNull.Value);
           }
           if (birth.TOB2 != null)
           {
               cmd.Parameters.AddWithValue("@TOB2", birth.TOB2);
           }
           else
           {
               cmd.Parameters.AddWithValue("@TOB2", DBNull.Value);
           }
           if (birth.TOB3 != null)
           {
               cmd.Parameters.AddWithValue("@TOB3", birth.TOB3);
           }
           else
           {
               cmd.Parameters.AddWithValue("@TOB3", DBNull.Value);
           }
           if (birth.TOB4 != null)
           {
               cmd.Parameters.AddWithValue("@TOB4", birth.TOB4);
           }
           else
           {
               cmd.Parameters.AddWithValue("@TOB4", DBNull.Value);
           }
           if (birth.Weight1 != null)
           {
               cmd.Parameters.AddWithValue("@Weight1", birth.Weight1);
           }
           else
           {
               cmd.Parameters.AddWithValue("@Weight1", "");
           }
           if (birth.Weight2 != null)
           {
               cmd.Parameters.AddWithValue("@Weight2", birth.Weight2);
           }
           else
           {
               cmd.Parameters.AddWithValue("@Weight2", "");
           }
           if (birth.Weight3 != null)
           {
               cmd.Parameters.AddWithValue("@Weight3", birth.Weight3);
           }
           else
           {
               cmd.Parameters.AddWithValue("@Weight3", "");
           }
           if (birth.Weight4 != null)
           {
               cmd.Parameters.AddWithValue("@Weight4", birth.Weight4);
           }
           else
           {
               cmd.Parameters.AddWithValue("@Weight4", "");
           }

           if (birth.Sex1 != null)
           {
               cmd.Parameters.AddWithValue("@Sex1", birth.Sex1);
           }
           else
           {
               cmd.Parameters.AddWithValue("@Sex1", "");
           }
           if (birth.Sex2 != null)
           {
               cmd.Parameters.AddWithValue("@Sex2", birth.Sex2);
           }
           else
           {
               cmd.Parameters.AddWithValue("@Sex2", "");
           }
           if (birth.Sex3 != null)
           {
               cmd.Parameters.AddWithValue("@Sex3", birth.Sex3);
           }
           else
           {
               cmd.Parameters.AddWithValue("@Sex3", "");
           }
           if (birth.Sex4 != null)
           {
               cmd.Parameters.AddWithValue("@Sex4", birth.Sex4);
           }
           else
           {
               cmd.Parameters.AddWithValue("@Sex4", "");
           }
           if (birth.BirthPlace!= null)
            {
                 cmd.Parameters.AddWithValue("@BirthPlace", birth.BirthPlace);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BirthPlace", "");
            }          
            if (birth.DeliveryType != null)
            {
                cmd.Parameters.AddWithValue("@DeliveryType", birth.DeliveryType);
            }
            else
            {
                 cmd.Parameters.AddWithValue("@DeliveryType", "");
            }
            if (birth.HODone != null)
            {
                cmd.Parameters.AddWithValue("@HODone", birth.HODone);
            }
            else
            {
                cmd.Parameters.AddWithValue("@HODone", "");
            }
            if (birth.Remarks != null)
            {
             cmd.Parameters.AddWithValue("@Remarks", birth.Remarks);
            }
             else
            {
                 cmd.Parameters.AddWithValue("@Remarks", "");
            }
            if (birth.NoOfChild!= null)
            {
                 cmd.Parameters.AddWithValue("@NoOfChild", birth.NoOfChild);
            }
              else
            {
                 cmd.Parameters.AddWithValue("@NoOfChild", "");
            }
            cmd.Parameters.AddWithValue("@CreationID", UserID);          
            int i = cmd.ExecuteNonQuery();
            birth.CertificateNo = Convert.ToInt32(cmd.Parameters["@CertificateNo"].Value);

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

        #region GetDataForPatientBirthCertificate
        public DataSet GetDataForPatientBirthCertificate(int CertificateNo)
        {
            Connect();
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("GetDataForPatientBirthCertificate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@CertificateNo", CertificateNo);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }

        }
        #endregion
        public int DeletePatientBirthCertificate(int CertificateNo)
        {
            Connect();
            int delete = 0;
            SqlCommand cmd = new SqlCommand("DeletePatientBirthCertificate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CertificateNo", CertificateNo);
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
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