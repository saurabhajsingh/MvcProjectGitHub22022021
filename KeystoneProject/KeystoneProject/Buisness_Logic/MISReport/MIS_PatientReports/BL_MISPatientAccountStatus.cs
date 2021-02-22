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
    public class BL_MISPatientAccountStatus
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        List<MISPatientAccountStatus> SearchList = new List<MISPatientAccountStatus>();
        private SqlConnection con;

        private void Connect()
        {
            string conString = ConfigurationManager.ConnectionStrings["MyCon"].ToString();
            con = new SqlConnection(conString);
        }

        public List<MISPatientAccountStatus> MISPatientAccountStatus(DateTime DateFrom, DateTime DateTo, string PatientType, string DisplayType, string PatientRegNo, string BillNo, string OPDIPDID)
        {
            Connect();

            List<MISPatientAccountStatus> MISPatientAccountStatus1 = new List<MISPatientAccountStatus>();
            SqlCommand cmd = new SqlCommand("ReportMISPatientAccountStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
            cmd.Parameters.AddWithValue("@DisplayType", DisplayType);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@BillNo", BillNo);
            cmd.Parameters.AddWithValue("@OPDIPDID", OPDIPDID);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                if (dt.Rows.Count.ToString() != null)
                {
                    if (dr["PatientType"].ToString() == "OPD")
                    {
                        MISPatientAccountStatus1.Add(
                       new MISPatientAccountStatus
                       {
                           PatientRegNo = Convert.ToInt32(dr["PatientRegNo"]),
                           PatientName = Convert.ToString(dr["PatientName"]),
                           PatientType = Convert.ToString(dr["PatientType"]),
                           OPDIPDID = dr["OPDIPDID"].ToString(),
                           BillAmount = Convert.ToDecimal(dr["BillAmount"]),
                           Discount = Convert.ToDecimal(dr["Discount"]),
                           PaidAmount = Convert.ToDecimal(dr["PaidAmount"]),
                           BalanceAmount = Convert.ToDecimal(dr["BalanceAmount"]),

                       });
                    }
                    else
                    {


                        MISPatientAccountStatus1.Add(
                            new MISPatientAccountStatus
                            {
                                PatientRegNo = Convert.ToInt32(dr["PatientRegNo"]),
                                PatientName = Convert.ToString(dr["PatientName"]),
                                PatientType = Convert.ToString(dr["PatientType"]),
                                OPDIPDID = dr["OPDIPDID"].ToString(),
                                AddmissionDate = Convert.ToString(dr["AddmissionDate"]),
                                OrganizationName = Convert.ToString(dr["OrganizationName"]),
                                Dischargedate = Convert.ToString(dr["Dischargedate"]),
                                TPA_CASH = Convert.ToString(dr["TPA_CASH"]),
                                BillAmount = Convert.ToDecimal(dr["BillAmount"]),
                                TDSAmount = Convert.ToDecimal(dr["TDSAmount"]),
                                TPAOtherDeduction = Convert.ToDecimal(dr["TPAOtherDeduction"]),
                                Discount = Convert.ToDecimal(dr["Discount"]),
                                PaidAmount = Convert.ToDecimal(dr["PaidAmount"]),
                                BalanceAmount = Convert.ToDecimal(dr["BalanceAmount"]),

                            });
                    }
                }
            }
            return MISPatientAccountStatus1;
        }

 public List<MISPatientAccountStatus> MISPatientAccountStatusDgv1(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string PatientType, string DisplayType, string PatientRegNo, string BillNo, string OPDIPDID)
        {
            Connect();

            List<MISPatientAccountStatus> MISPatientAccountStatus = new List<MISPatientAccountStatus>();
            SqlCommand cmd = new SqlCommand("ReportMISPatientAccountStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
            cmd.Parameters.AddWithValue("@DisplayType", DisplayType);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@BillNo", BillNo);
            cmd.Parameters.AddWithValue("@OPDIPDID", OPDIPDID);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                MISPatientAccountStatus.Add(
                    new MISPatientAccountStatus
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNo"]),
                        PatientOPDIPDNo = Convert.ToInt32(dr["PatientOPDIPDNo"]),
                        BillNo = Convert.ToInt32(dr["BillNo"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientAccountRowID = Convert.ToInt32(dr["PatientAccountRowID"]),
                        BillType = Convert.ToString(dr["BillType"]),
                        DrAmount = Convert.ToDecimal(dr["DrAmount"]),
                        CrAmount = Convert.ToDecimal(dr["CrAmount"]),
                        PaymentType = Convert.ToString(dr["PaymentType"]),
                                              
                    });

            }
            return MISPatientAccountStatus;




        }

        public List<MISPatientAccountStatus> MISPatientAccountStatusDgv2(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string PatientType, string DisplayType, string PatientRegNo, string BillNo, string OPDIPDID)
        {
            Connect();

            List<MISPatientAccountStatus> MISPatientAccountStatus = new List<MISPatientAccountStatus>();
            SqlCommand cmd = new SqlCommand("ReportMISPatientAccountStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
            cmd.Parameters.AddWithValue("@DisplayType", DisplayType);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@BillNo", BillNo);
            cmd.Parameters.AddWithValue("@OPDIPDID", OPDIPDID);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            con.Close();
            HttpContext.Current.Session["ReportMISPatientAccountStatus"] = ds;
            foreach (DataRow dr in dt.Rows)
            {
                MISPatientAccountStatus.Add(
                    new MISPatientAccountStatus
                    {
                       
                        BillNo = Convert.ToInt32(dr["BillNo"]),
                        ServiceName = Convert.ToString(dr["ServiceName"]),
                        Quantity = Convert.ToDecimal(dr["Quantity"]),
                        Rate = Convert.ToDecimal(dr["Rate"]),
                        BillAmount = Convert.ToDecimal(dr["BillAmount"]),
                       
                    });

            }
            return MISPatientAccountStatus;




        }

    }
}