using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Buisness_Logic.Laboratory;
using KeystoneProject.Models.Laboratory;
namespace KeystoneProject.Buisness_Logic.Laboratory
{
    public class BL_PatientLabReportPrint
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<PatientLabReportPrint> PatientLabReportList = new List<PatientLabReportPrint>();
        PatientLabReportPrint obj = new PatientLabReportPrint();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet GetCategory(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select   CategoryID  , CategoryName  from category   where CategoryName like ''+@prefix+'%' and   HospitalID = " + HospitalID + " and LocationID =  " + LocationID + " and RowStatus = 0 Order by CategoryName  asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception )
            { }
            return ds;
        }


        public DataSet GetTestName(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select   TestID  , TestName  from TestMaster  where  TestName like ''+@prefix+'%' and   HospitalID = " + HospitalID + " and LocationID =  " + LocationID + " and RowStatus = 0 Order by TestName  asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetDepartment(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select   DepartmentID  , DepartmentName  from Department  where  DepartmentName like ''+@prefix+'%' and   HospitalID = " + HospitalID + " and LocationID =  " + LocationID + " and RowStatus = 0 Order by DepartmentName  asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetDoctor(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select   DoctorID  , DoctorPrintName  from Doctor  where  DoctorPrintName like ''+@prefix+'%' and   HospitalID = " + HospitalID + " and LocationID =  " + LocationID + " and RowStatus = 0 and DoctorType = 'Consultant' Order by DoctorPrintName  asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetPatient(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select   PatientRegNO  , PatientName  from patient  where  PatientName like ''+@prefix+'%' and   HospitalID = " + HospitalID + " and LocationID =  " + LocationID + " and RowStatus = 0   Order by PatientName  asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetAllPatientLabReports(string fromdate, string todate,string PatientRegNo, string LabNo,string DoctorID)
        {
            Connect();
            DataSet ds = null;
        //    fromdate = obj.FromDate;
         
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllPatientLabReports", con);
                cmd.CommandType = CommandType.StoredProcedure;
              
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                //cmd.Parameters.AddWithValue("@PatientName", PatientName);
                cmd.Parameters.AddWithValue("@DateFrom", fromdate);
                cmd.Parameters.AddWithValue("@DateTo", todate);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                cmd.Parameters.AddWithValue("@DepartmentID", "");
                cmd.Parameters.AddWithValue("@CategoryID", "");
                cmd.Parameters.AddWithValue("@TestID","");
                cmd.Parameters.AddWithValue("@LabNo", LabNo);


                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ds = new DataSet();
                ad.Fill(ds);
                con.Close();
                return ds;

 
                
            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        public DataSet SendMail(string Lab, string testid)
        {
            Connect();
            DataSet ds = null;
            //    fromdate = obj.FromDate;

            try
            {
                SqlCommand cmd = new SqlCommand("RptPatientLabReports", con);
                cmd.CommandType = CommandType.StoredProcedure;


              

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@LabNo", Lab);
                cmd.Parameters.AddWithValue("@TestID", testid);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ds = new DataSet();
                ad.Fill(ds);
                con.Close();
                return ds;



            }
            catch (Exception ex)
            {
                return ds;
            }
        }


        public DataSet GetPatientLabBillDetails(string LabNo)
        {
            Connect();
            DataSet ds = null;
            //    fromdate = obj.FromDate;

            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientLabBillDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@LabNo", LabNo);
              


                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ds = new DataSet();
                ad.Fill(ds);
                con.Close();
                return ds;



            }
            catch (Exception ex)
            {
                return ds;
            }

        }

        public DataSet GetRptPatientLabReports(string LabNo, string TestID)
        {
            Connect();
            DataSet ds = null;
            //    fromdate = obj.FromDate;

            try
            {
                SqlCommand cmd = new SqlCommand("RptPatientLabReports", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@LabNo", LabNo);
                cmd.Parameters.AddWithValue("@TestID", TestID);



                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ds = new DataSet();
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