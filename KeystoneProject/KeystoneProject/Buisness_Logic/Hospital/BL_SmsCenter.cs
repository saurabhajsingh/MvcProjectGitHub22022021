using KeystoneProject.Models.Keystone;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Hospital
{
    public class BL_SmsCenter
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<SmsCenter> categorylist = new List<SmsCenter>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<SmsCenter> SelectAllSmsCenter()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetSmsCenterForAllData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@Designation", "%");
            cmd.Parameters.AddWithValue("@DoctorType", "%");
            cmd.Parameters.AddWithValue("@PatientType", "%");
          

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                categorylist.Add(
                    new SmsCenter
                    {

                        Select = Convert.ToString(dr["Select"]),
                        Name = Convert.ToString(dr["Name"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        EmailID = Convert.ToString(dr["EmailID"]),
                      
                    });
            }
            return categorylist;


        }

        public List<SmsCenter> FillReferredDoctor( string ReferredDoctor)
        {
            Connect();

            List<SmsCenter> categorylist = new List<SmsCenter>();
            SqlCommand cmd = new SqlCommand("GetSmsCenterForAllData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@Designation", "%");
            cmd.Parameters.AddWithValue("@DoctorType", ReferredDoctor);
            cmd.Parameters.AddWithValue("@PatientType", "%");


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
           
            foreach (DataRow dr in dt.Rows)
            {
                categorylist.Add(
                    new SmsCenter
                    {

                        Select = Convert.ToString(dr["Select"]),
                        Name = Convert.ToString(dr["Name"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        EmailID = Convert.ToString(dr["EmailID"]),

                    });
            }
            return categorylist;


        }

        public List<SmsCenter> FillEmployee()
        {
            Connect();

            List<SmsCenter> categorylist = new List<SmsCenter>();
            SqlCommand cmd = new SqlCommand("GetSmsCenterForBindPatient", con);
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
                categorylist.Add(
                    new SmsCenter
                    {

                        Designation = Convert.ToString(dr["Designation"]),
                       

                    });
            }
            return categorylist;


        }

        public List<SmsCenter> FillPatientType()
        {
            Connect();

            List<SmsCenter> categorylist = new List<SmsCenter>();
            SqlCommand cmd = new SqlCommand("GetSmsCenterForBindPatient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);



            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Tables[1].Rows)
            {
                categorylist.Add(
                    new SmsCenter
                    {

                        PatientType = Convert.ToString(dr["PatientType"]),


                    });
            }
            return categorylist;


        }

        public List<SmsCenter> FillEmployeeDesignation(string Designation)
        {
            Connect();

            List<SmsCenter> categorylist = new List<SmsCenter>();
            SqlCommand cmd = new SqlCommand("GetEmployeeDesignation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@Designation", Designation);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                categorylist.Add(
                    new SmsCenter
                    {

                        Select = Convert.ToString(dr["Select"]),
                        Name = Convert.ToString(dr["Name"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        EmailID = Convert.ToString(dr["EmailID"]),


                    });
            }
            return categorylist;


        }

        public List<SmsCenter> FillPatientOPDandIPD(string PatientType)
        {
            Connect();

            List<SmsCenter> categorylist = new List<SmsCenter>();
            SqlCommand cmd = new SqlCommand("GetPatientTypeForSms", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                categorylist.Add(
                    new SmsCenter
                    {

                        Select = Convert.ToString(dr["Select"]),
                        Name = Convert.ToString(dr["Name"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        EmailID = Convert.ToString(dr["EmailID"]),


                    });
            }
            return categorylist;


        }

        public DataSet GetMasterSettingData()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetMasterSetting", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetAllSmsSetting()
        {
            string Designation = "%";
            string DoctorType = "%";
            string PatientType = "%";
            Connect();
         
            SqlCommand cmd = new SqlCommand("GetSmsCenterForAllData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@Designation", Designation);
            cmd.Parameters.AddWithValue("@DoctorType", DoctorType);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;    
        }
    }
}