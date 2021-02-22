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
    public class BL_ReportMISReferredWiseAnalyse
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;

        List<ReportMISReferredWiseAnalyse> ListReff = new List<ReportMISReferredWiseAnalyse>();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }

        public List<ReportMISReferredWiseAnalyse> BindConsultant()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  DoctorID, DoctorPrintName,DepartmentID  from  Doctor where  DoctorType ='Referred' and RowStatus =0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 order by  DoctorFName asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                ListReff.Add(
                    new ReportMISReferredWiseAnalyse
                    {
                        DoctorID = Convert.ToInt32(dr["DoctorID"]).ToString(),
                        ReffDoctor = Convert.ToString(dr["DoctorPrintName"]),


                    });
            }
            return ListReff;

        }

        public List<ReportMISReferredWiseAnalyse> ReportMISReffDoctorWiseAnalyse(DateTime DateFrom, DateTime DateTo, string PatientType, string DoctorID)
        {

            Connect();

           // List<ReportMISReferredWiseAnalyse> List = new List<ReportMISReferredWiseAnalyse>();
            SqlCommand cmd = new SqlCommand("ReportMISReferredWiseAnalyse", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //   cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@RefDocID", DoctorID);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(ds);

           

            con.Close();
            HttpContext.Current.Session["ReportMISReferredWiseAnalyse"] = ds;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListReff.Add(
                    new ReportMISReferredWiseAnalyse
                    {

                        ReffDoctor = Convert.ToString(dr["Reffered Dr."]),
                        RegNo = Convert.ToString(dr["RegNo"]),
                        PatientName = Convert.ToString(dr["Patient"]),
                        PatientType = Convert.ToString(dr["PatientType"]),
                        MobileNo = Convert.ToString(dr["Phone/MobileNo"]),
                        Address = Convert.ToString(dr["Address"]),

                    });

            }
            return ListReff;
        }


    }
}