using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using KeystoneProject.Controllers.MISReport.MIS_PatientReports;

namespace KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports
{
    public class BL_MISDailyIncomeExpensesReport
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
        public List<MISDailyIncomeExpensesReport> BindUserName(string prefix)
        {
            List<MISDailyIncomeExpensesReport> supplierList = new List<MISDailyIncomeExpensesReport>();
            Connect();
            SqlCommand cmd = new SqlCommand("select UserID,LoginName from Users where Users.HospitalID = " + HospitalID + " and Users.LocationID = " + LocationID + " and Users.RowStatus = 0 and Users.LoginName like '" + prefix + '%' + "'  order by LoginName asc ", con);
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ds.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                supplierList.Add(
                    new MISDailyIncomeExpensesReport
                    {
                        UserName = dr["LoginName"].ToString(),
                        UserID = dr["UserID"].ToString()

                    });

            }

            return supplierList;
        }

        public DataSet RptReportDailyEncomeExpences(DateTime FromDate, DateTime ToDate, string UserID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("RptReportDailyEncomeExpences", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@UserID", UserID);

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