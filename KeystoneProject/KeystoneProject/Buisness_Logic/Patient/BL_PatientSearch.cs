using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_PatientSearch
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
 


        private SqlConnection con;

        public void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["MyCon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet GetAllFinancialYear ()
        {
            Connect();
           
            SqlCommand cmd = new SqlCommand("GetAllFinancialYear", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet dt = new DataSet();
            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;
        }
        


        public DataSet SelectAllData(string financialYear,string FName,string Lastname,string MidLename,string PatientType,string Doctor)
        {
            PatientSearch obj = new PatientSearch();
            Connect();
            List<PatientSearch> patientlist = new List<PatientSearch>();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("GetPatientSearch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);         
            cmd.Parameters.AddWithValue("@PatientName","%");

            cmd.Parameters.AddWithValue("@PatientType" ,PatientType);
            
            cmd.Parameters.AddWithValue("@FirstName", FName);
            cmd.Parameters.AddWithValue("@MiddleName", MidLename);
            cmd.Parameters.AddWithValue("@LastName",Lastname );       
            cmd.Parameters.AddWithValue("@DoctorID", Doctor);
            cmd.Parameters.AddWithValue("@FinancialYearName", financialYear);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand = cmd;
           
            con.Open();
            da.Fill(ds);
            con.Close();

            return ds;
        }


        public DataSet GetPatient(string FirstName, string Mdlename, string LAstname)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNO,PFirstName,PLastName,PMiddleName from Patient where PFirstName  like '" + FirstName + "%' and PLastName  like '" + LAstname + "%' and PMiddleName  like '" + Mdlename + "%' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

      

        public DataSet GetAllConsultantDoctor(string GetAllConsultantDoctor)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select DoctorID,DoctorPrintName from Doctor where DoctorPrintName like '" + GetAllConsultantDoctor + "%' and DoctorType ='Consultant' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetAllRefferedDoctorRecord(string GetAllRefferedDoctorRecord)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select DoctorID,DoctorPrintName from Doctor where DoctorPrintName like '" + GetAllRefferedDoctorRecord + "%' and DoctorType ='Referred' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet getFinancialYear()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select FinancialYear,FinancialYearId from FinancialYear where  HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

    }
 
}
