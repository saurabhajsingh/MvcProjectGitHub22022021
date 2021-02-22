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
    public class BL_MISHospitalDayBookReport
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

        public List<MISHospitalDayBookReport> ReportMISHospitalDayBook(DateTime DateFrom, string Type1)
        {
            HttpContext.Current.Session["DateFrom"] = DateFrom;
            HttpContext.Current.Session["Type1"] = Type1;


            Connect();
            List<MISHospitalDayBookReport> MISReportMISDoctorFees = new List<MISHospitalDayBookReport>();
         
            return MISReportMISDoctorFees;

        }



    }
}
