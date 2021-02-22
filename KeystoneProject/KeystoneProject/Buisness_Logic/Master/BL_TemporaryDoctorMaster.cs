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
    public class BL_TemporaryDoctorMaster
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        List<TemporaryDoctorMaster> PatientLabBillsList = new List<TemporaryDoctorMaster>();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }


        public DataSet GetPatientForTemporaryDoctorMaster(int HospitalID, int LocationID, int RegNO)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            { 
                SqlCommand cmd = new SqlCommand("GetPatientForTemporaryDoctorMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNO", RegNO);



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
        public DataSet UpdateDoctorID(TemporaryDoctorMaster obj1)
        {
            SqlDataAdapter dt = new SqlDataAdapter();
            DataSet ds = new DataSet();

            if (obj1.PatientType == "OPD")
            {
                if (obj1.ConsultantDoctorID != "")
                {
                    Connect();
                    SqlCommand da = new SqlCommand("update PatientOPDDetails set DoctorID = " + obj1.ConsultantDoctorID + " ,DepartmentID = " + obj1.DepartMentID + ",TemporaryDoctor = " +  "''"+ ",RefferedTemporaryDoctorStatus = " + 0 + ",TemporaryDoctorStatus = " + 0 + " where PatientOPDNO = " + obj1.PatientOPDIPDNO + " ", con); 
                    dt.SelectCommand = da;
                    con.Open();
                    dt.Fill(ds);
                    con.Close();

                }

                if (obj1.ReferredByDoctorID !="")
                {
                    Connect();
                    SqlCommand da1 = new SqlCommand("update PatientOPDDetails set ReferredByDoctorID = " + obj1.ReferredByDoctorID + ",RefferedTemporaryDoctor = " + "''" + "  where PatientOPDNO = " + obj1.PatientOPDIPDNO + " ", con);
                    dt.SelectCommand = da1;
                    con.Open();
                    dt.Fill(ds);
                    con.Close();
                }
            }

            if (obj1.PatientType == "IPD")
            {
                if (obj1.ConsultantDoctorID != "")
                {
                    Connect();
                    SqlCommand da2 = new SqlCommand("update PatientIPDDetails set ConsultantDrID = " + obj1.ConsultantDoctorID + ",DepartmentID = " + obj1.DepartMentID + " ,TemporaryDoctor = " + "''" + ",RefferedTemporaryDoctorStatus = " + 0 + " ,TemporaryDoctorStatus = " + 0 + " where PatientIPDNO = " + obj1.PatientOPDIPDNO + " ", con);
                    dt.SelectCommand = da2;
                    con.Open();
                    dt.Fill(ds);
                    con.Close();
                }
              
          
            if (obj1.ReferredByDoctorID != "")
            {
                Connect();
                SqlCommand da3 = new SqlCommand("update PatientIPDDetails set ReferredByDoctorID = " + obj1.ReferredByDoctorID + " ,RefferedTemporaryDoctor = " + "''" + "  where PatientIPDNO = " + obj1.PatientOPDIPDNO + " ", con);
                dt.SelectCommand = da3;
                con.Open();
                dt.Fill(ds);
                con.Close();
             }
         }
            return ds;
        }

        public List<TemporaryDoctorMaster> ShowAllTemporaryDoctorStatus()
        {
            Connect();
            List<TemporaryDoctorMaster> accounts = new List<TemporaryDoctorMaster>();
            SqlCommand cmd = new SqlCommand("ShowAllTemporaryDoctorStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                accounts.Add(
                    new TemporaryDoctorMaster
                    {
                        PatientRegNo = Convert.ToString(dr["UHID No"]),
                        patientname = Convert.ToString(dr["PatientName"]),
                        TemporaryDoctor = Convert.ToString(dr["TemporaryDoctor"]),
                        RefferedTemporaryDoctor = Convert.ToString(dr["RefferedTemporaryDoctor"]),

                    });
            }
            return accounts;
        }
    }
}