using KeystoneProject.Models.MISReport.MIS_PatientReports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports
{
    public class BL_ReportMISConsultantDoctorWiseAnalyse
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;

        List<ReportMISConsultantDoctorWiseAnalyse> ReportMISConsultantDoctorWiseAnalyseList = new List<ReportMISConsultantDoctorWiseAnalyse>();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }

        public List<ReportMISConsultantDoctorWiseAnalyse> BindConsultant()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  DoctorID, DoctorPrintName,DepartmentID  from  Doctor where  DoctorType ='Consultant' and RowStatus =0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 order by  DoctorFName asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                ReportMISConsultantDoctorWiseAnalyseList.Add(
                    new ReportMISConsultantDoctorWiseAnalyse
                    {
                        DoctorID = Convert.ToInt32(dr["DoctorID"]).ToString(),
                        ConsultantDoctor = Convert.ToString(dr["DoctorPrintName"]),
                       

                    });
            }
            return ReportMISConsultantDoctorWiseAnalyseList;

        }

        public List<ReportMISConsultantDoctorWiseAnalyse> ReportMISConsultantDoctorWiseAnalyse(DateTime DateFrom, DateTime DateTo, string PatientType, string DoctorID)
        {

            Connect();

            List<ReportMISConsultantDoctorWiseAnalyse> ReportMISConsultantDoctorWiseAnalyse = new List<ReportMISConsultantDoctorWiseAnalyse>();
            SqlCommand cmd = new SqlCommand("ReportMISConsultantDoctorWiseAnalyse", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //   cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@ConsDrID", DoctorID);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(ds);

            HttpContext.Current.Session["ReportMISConsultantDoctorWiseAnalyse"] = ds;
             
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ReportMISConsultantDoctorWiseAnalyse.Add(
                    new ReportMISConsultantDoctorWiseAnalyse
                    {

                        ConsultantDoctor = Convert.ToString(dr["Consultant Dr"]),
                        RegNo = Convert.ToString(dr["RegNo"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientType = Convert.ToString(dr["Patient Type"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        Address = Convert.ToString(dr["Address"]),

                    });

            }
            return ReportMISConsultantDoctorWiseAnalyse;
        }

    }
}