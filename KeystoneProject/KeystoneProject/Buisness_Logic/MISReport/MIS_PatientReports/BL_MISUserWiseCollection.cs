using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Keystone;
using System.Data;
using KeystoneProject.Models.Report;

namespace KeystoneProject.Buisness_Logic.MISReport
{
    public class BL_MISUserWiseCollection
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
        public List<Users> GetUserName(string search)
        {
            Connect();

            List<Users> usernamelist = new List<Users>();
            SqlCommand cmd = new SqlCommand("select FullName,UserID,LoginName,EmailID,RoleName from Users where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and FullName like '" + search + '%' + "' and RowStatus=0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                usernamelist.Add(
                    new Users
                    {
                        FullName = Convert.ToString(dr["FullName"]),
                        UserID = Convert.ToInt32(dr["UserID"])
                        
                    });

            }
            return usernamelist;
        }
        public List<MISUserWiseCollection> MISUserWiseCollection(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string UserID)
        {
            Connect();

            List<MISUserWiseCollection>  MISUserWiseCollection= new List<MISUserWiseCollection>();
            SqlCommand cmd = new SqlCommand("ReportMISUserWiseCollection", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@UserId", UserID);
     
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad=new SqlDataAdapter();
            ad.SelectCommand = cmd;
             ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                MISUserWiseCollection.Add(
                    new MISUserWiseCollection
                    {
                        UserID = Convert.ToString(dr["UserID"]),
                        FullName = Convert.ToString(dr["FullName"]),
                        TotalCollection = Convert.ToString(dr["TotalCollection"]),

                    });

            }
            return MISUserWiseCollection;


        }

        public List<MISUserWiseCollection> MISUserWiseCollectionPaticular(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string UserID, string PaymentType)
        {
            Connect();

            List<MISUserWiseCollection>  MISUserWiseCollection= new List<MISUserWiseCollection>();
            SqlCommand cmd = new SqlCommand("ReportMISUserWiseCollectionPaticular", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@PaymentType", PaymentType);

            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad=new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                MISUserWiseCollection.Add(
                    new MISUserWiseCollection
                    {
                        RegNO = Convert.ToInt32(dr["RegNO"]),
                        FullName = Convert.ToString(dr["FullName"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        Particular = Convert.ToString(dr["Particular"]),
                        DrAmount = Convert.ToDecimal(dr["DrAmount"]),
                        CrAmount = Convert.ToDecimal(dr["CrAmount"]),
                        //TDSAmount = Convert.ToString(dr["TDSAmount"]),
                        //TPAOtherDeduction = Convert.ToString(dr["TPAOtherDeduction"]),
                        PaymentType = Convert.ToString(dr["PaymentType"]),
                        Date = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy")

                    });

            }
            return MISUserWiseCollection;


        }


        }
}