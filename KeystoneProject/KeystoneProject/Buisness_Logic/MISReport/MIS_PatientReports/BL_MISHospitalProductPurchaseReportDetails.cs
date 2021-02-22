using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using KeystoneProject.Controllers.MISReport.MIS_PatientReports;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports
{
    public class BL_MISHospitalProductPurchaseReportDetails
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

        public List<MISHospitalProductPurchaseReportDetails>BindSupplierName(string prefix)
        {
            List<MISHospitalProductPurchaseReportDetails> supplierList = new List<MISHospitalProductPurchaseReportDetails>();
            Connect();
            SqlCommand cmd = new SqlCommand("select HospitalProductPurchase.SupplierID,HospitalProductPurchase.SupplierName from HospitalProductPurchase where HospitalProductPurchase.HospitalID = " + HospitalID + " and HospitalProductPurchase.LocationID = " + LocationID + " and HospitalProductPurchase.RowStatus = 0 and HospitalProductPurchase.SupplierName like '" + prefix + '%' + "' group by HospitalProductPurchase.SupplierID,HospitalProductPurchase.SupplierName order by SupplierName asc ", con);
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ds.Fill(dt);
            con.Close();
            foreach(DataRow dr in dt.Rows)
            {
                supplierList.Add(
                    new MISHospitalProductPurchaseReportDetails
                    {
                        SupplierName = dr["SupplierName"].ToString(),
                        SupplierID = dr["SupplierID"].ToString()

                    });
          
            }

            return supplierList;
        }

        public DataSet GetReportLabTest(DateTime FromDate, DateTime ToDate, string PatientType)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportLabTest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PatientType", PatientType);

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
        public DataSet ReportMISHospitalProductPurchaseReport(DateTime fromDate, DateTime todate, string SupplierId, string InvoiceNo, string Billno)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {

         
           
          
            SqlCommand cmd = new SqlCommand("ReportMISHospitalProductPurchaseReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DateFrom", fromDate);
            cmd.Parameters.AddWithValue("@DateTo",todate);
            cmd.Parameters.AddWithValue("@HospitalProductPurchaseID", Billno);
            cmd.Parameters.AddWithValue("@SupplierID", SupplierId);
            cmd.Parameters.AddWithValue("@InvoiceNumber", InvoiceNo);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            sd.SelectCommand = cmd;
           
            con.Open();

            sd.Fill(ds);
            con.Close();
            HttpContext.Current.Session["ReportMISHospitalProductPurchaseReport"] = ds;
            return ds;
              }
            catch(Exception ex)
            {
                return ds;
            }
            
        }

    }
}