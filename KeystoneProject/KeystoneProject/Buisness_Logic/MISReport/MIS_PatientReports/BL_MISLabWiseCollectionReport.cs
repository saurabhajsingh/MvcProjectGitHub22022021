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
    public class BL_MISLabWiseCollectionReport
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
        public List<Profile> GetTestName(string search)
        {
            Connect();

            List<Profile> testnamelist = new List<Profile>();
             SqlCommand cmd = new SqlCommand("select TestID ,upper(TestName) as TestName from TestMaster   where TestName like '" + search + '%' + "'  and  RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "  order by  TestName asc", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                testnamelist.Add(
                    new Profile
                    {
                        TestID = dr["TestID"].ToString(),
                        TestName = dr["TestName"].ToString(),

                    });

            }
            return testnamelist;
        }


        public List<ServiceGroup> GetServiceGroupName(string search)
        {
            Connect();

            List<ServiceGroup> ServiceGroupnamelist = new List<ServiceGroup>();
            SqlCommand cmd = new SqlCommand("select ServiceGroupID,upper(ServiceGroupName) as ServiceGroupName  from ServiceGroup where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and ServiceGroupName like '" + search + '%' + "' and RowStatus=0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                ServiceGroupnamelist.Add(
                    new ServiceGroup
                    {
                        ServiceGroupName = Convert.ToString(dr["ServiceGroupName"]),
                        ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"])
                    });

            }
            return ServiceGroupnamelist;
        }
        public List<MISLabWiseCollectionReport> MISLabWiseCollectionReport(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string TestID, string CategoryID, string PatientType)
        {
            Connect();

            List<MISLabWiseCollectionReport> MISLabWiseCollectionReport = new List<MISLabWiseCollectionReport>();
            SqlCommand cmd = new SqlCommand("ReportMISLabWiseCollection", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@TestID", TestID);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
    
           

            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                MISLabWiseCollectionReport.Add(
                    new MISLabWiseCollectionReport
                    {
                        SrNo = Convert.ToString(dr["SrNo"]),
                        ServiceGroupName = Convert.ToString(dr["ServiceGroupName"]),
                      //  PatientName = Convert.ToString(dr["PatientName"]),
                        TestID   = Convert.ToInt32(dr["TestID"]),
                        TestName = Convert.ToString(dr["TestName"]),
                        TotalAmount = Convert.ToDecimal(dr["Total Amount"]),
                     
                    });

            }
            return MISLabWiseCollectionReport;


        }

        public List<MISLabWiseCollectionReport> MISLabWiseCollectionReportDgv1(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string TestID, string CategoryID, string PatientType)
        {
            Connect();
           
            List<MISLabWiseCollectionReport> MISLabWiseCollectionReport = new List<MISLabWiseCollectionReport>();
            SqlCommand cmd = new SqlCommand("ReportMISLabWiseCollection", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@TestID", TestID);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);

            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand  = cmd;
            ad.Fill(dt);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            HttpContext.Current.Session["ReportMISLabWiseCollection"] = ds;
            con.Close();
    
            foreach (DataRow dr in ds.Tables[2].Rows)

            {
                 MISLabWiseCollectionReport.Add(
                    new MISLabWiseCollectionReport
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNo"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        TestName = Convert.ToString(dr["TestName"]),
                        TotalAmount = Convert.ToDecimal(dr["Total Amount"]),

                    });

            }
            return MISLabWiseCollectionReport;


        }
  
    }
}