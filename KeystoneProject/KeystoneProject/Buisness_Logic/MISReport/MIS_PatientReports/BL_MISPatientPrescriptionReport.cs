using KeystoneProject.Models.Master;
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
    public class BL_MISPatientPrescriptionReport
    {
        private SqlConnection con;

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID1 = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }


        public List<DoctorCommissionSetting> GetDoctor(string DoctorID)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
            SqlCommand cmd = new SqlCommand("GetDoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serach.Add(new Models.Master.DoctorCommissionSetting
                {
                    Department = dr["DepartmentName"].ToString(),
                    DoctorType = dr["DoctorType"].ToString()

                });
            }
            return serach;
        }

        public List<MISPatientPrescriptionReport> MISPatientPrescriptionReport(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string ConsultantDoctore, string PatientType, string PatientPrescriptionID)
        {
            Connect();

            List<MISPatientPrescriptionReport> MISPatientPrescriptionReport = new List<MISPatientPrescriptionReport>();
            SqlCommand cmd = new SqlCommand("RptMISPatientPrescription", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@ConsultantDoctore", ConsultantDoctore);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
            cmd.Parameters.AddWithValue("@PatientPrescriptionID", PatientPrescriptionID);
          

            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {  
                MISPatientPrescriptionReport.Add(
                    new MISPatientPrescriptionReport
                    {

                        PatientPrescriptionID = Convert.ToInt32(dr["PatientPrescriptionID"]),
                        PatientRegNO = Convert.ToInt32(dr["PatientRegNO"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                        ChiefComplaint = Convert.ToString(dr["ChiefComplaint"]),
                        Days = Convert.ToString(dr["Days"]),
                        DiagnosisName = Convert.ToString(dr["DiagnosisName"]),
                        InvestigationName = Convert.ToString(dr["InvestigationName"]),
                        DrugName = Convert.ToString(dr["DrugName"]),
                        
                    });

            }
            return MISPatientPrescriptionReport;


        }

    }
}