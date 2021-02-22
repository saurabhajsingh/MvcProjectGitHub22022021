using KeystoneProject.Models.Report;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MISReport
{
    public class BL_MISDoctorWiseCollectionReport
    {
        private SqlConnection con;

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public DataSet DoctorType()
        {
            Connect();
            DataSet dsDoctorWise = null;

            try
            {
                SqlCommand cmd = new SqlCommand("GetDoctorForCommission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsDoctorWise = new DataSet();
                da.Fill(dsDoctorWise);
                con.Close();
                return dsDoctorWise;
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public DataSet ReportMISDoctorWiseCollection(MISDoctorWiseCollectionReport DoctorWiseModel)
        {
            Connect();
            DataSet dsDoctorWiseCollection = null;
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISDoctorWiseCollection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DoctorWiseModel.DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DoctorWiseModel.DateTo);
                cmd.Parameters.AddWithValue("@DoctorType", DoctorWiseModel.DoctorType);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorWiseModel.DoctorID);
                cmd.Parameters.AddWithValue("@PatientType", DoctorWiseModel.PatientType);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsDoctorWiseCollection = new DataSet();
                da.Fill(dsDoctorWiseCollection);
                con.Close();

                return dsDoctorWiseCollection;
            }
            catch (Exception)
            {
                return dsDoctorWiseCollection;
            }
        }

        public DataSet ReportMISDoctorWiseCollectionForParticular(MISDoctorWiseCollectionReport DoctorDetailParticular)
        {
            Connect();
            DataSet dsDoctorwiseParticular = null;
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISDoctorWiseCollectionForParticular", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DoctorDetailParticular.DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DoctorDetailParticular.DateTo);
                cmd.Parameters.AddWithValue("@DoctorType", DoctorDetailParticular.DoctorType);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorDetailParticular.DoctorID);
                cmd.Parameters.AddWithValue("@PatientType", DoctorDetailParticular.PatientType);
                cmd.Parameters.AddWithValue("@UserID", "%");
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsDoctorwiseParticular = new DataSet();
                da.Fill(dsDoctorwiseParticular);
                con.Close();

                return dsDoctorwiseParticular;
            }
            catch (Exception)
            {
                return dsDoctorwiseParticular;
            }
        }
    }
}