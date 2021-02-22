using KeystoneProject.Models.FinancialAccountReport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.FinancialAccountReport
{
    public class BL_MISHospitalProductPurchase
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


        public DataSet MISHospitalEntryWiseReport(MISHospitalProductPurchase DoctorWiseModel)
        {
            Connect();
            DataSet dsDoctorWiseCollection = null;
            try
            {
                SqlCommand cmd = new SqlCommand("RptMISHospitalProductWisePurchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", DoctorWiseModel.DateFrom);
                cmd.Parameters.AddWithValue("@ToDate", DoctorWiseModel.DateTo);
                cmd.Parameters.AddWithValue("@SupplierID", DoctorWiseModel.SupplierID);
                cmd.Parameters.AddWithValue("@HospitalProductID", DoctorWiseModel.ProductID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsDoctorWiseCollection = new DataSet();
                da.Fill(dsDoctorWiseCollection);
                con.Close();

                return dsDoctorWiseCollection;
            }
            catch (Exception)
            {
                return dsDoctorWiseCollection;
            }
        }

        public DataSet BindSupplierName(string GetSupplierName)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Accounts.AccountsID,Accounts.AccountName from Accounts  where AccountName like ''+@GetSupplierName+'%' and Accounts.AccountType='SUPPLIER ACCOUNT' and Accounts.HospitalID=" + HospitalID + " and Accounts.LocationID=" + LocationID + " and Accounts.RowStatus=0 order by AccountName asc", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GetSupplierName", GetSupplierName);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception )
            { }
            return ds;
        }

        public DataSet BindProduct(string GetProduct)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select  HospitalProductID ,HospitalProductName from HospitalProduct where HospitalProductName like ''+@GetProduct+'%' and HospitalProduct.HospitalID= " + HospitalID + " and HospitalProduct.LocationID = " + LocationID + " and HospitalProduct.RowStatus = 0  Order by HospitalProductName asc ", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GetProduct", GetProduct);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

    }
}