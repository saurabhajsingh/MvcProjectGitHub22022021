using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.Pharmacy;

namespace KeystoneProject.Buisness_Logic.Pharmacy
{
    public class BL_StockStatus
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<StockStatus> searchList = new List<StockStatus>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet BindProduct(string prefix)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select ProductID,ProductName from Product where HospitalID =" + HospitalID + " and  LocationID =" + LocationID + " and RowStatus=0 and ProductName like '" + prefix + "%' order by ProductName", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataTable Bindbatch(int ProductID)
        {
            DataTable dt = new DataTable();

            Connect();
            SqlCommand cmd = new SqlCommand("select BatchNo from  ProductDetails where HospitalID =" + HospitalID + " and  LocationID =" + LocationID + " and ProductID=" + ProductID + " and RowStatus= 0", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public List<StockStatus> Getdata()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllStockStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["A"].ToString() == "B")
                    {                        
                        dr["ProductName"] = "";
                    }

                    searchList.Add(
                        new StockStatus
                        {
                            ProductID = Convert.ToInt32(dr["ProductID"]),
                            productName = Convert.ToString(dr["ProductName"]),
                            productBatch = Convert.ToString(dr["BatchNo"]),
                            PurchaseRate = Convert.ToString(dr["PurchaseRate"]),
                            CurrentStock = Convert.ToString(dr["CurrentStock"]),
                            CurrentStock1 = Convert.ToString(dr["CurrentStock1"]),
                            column = Convert.ToString(dr["A"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return searchList;
        }

        public List<StockStatus> GetStockStatusData(string ProductID, string batch)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetStockStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductID", ProductID);
                cmd.Parameters.AddWithValue("@ProductDetailsID", batch);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["A"].ToString() == "B")
                    {
                        dr["ProductName"] = "";
                    }

                    searchList.Add(
                        new StockStatus
                        {
                            ProductID = Convert.ToInt32(dr["ProductID"]),
                            productName = Convert.ToString(dr["ProductName"]),
                            productBatch = Convert.ToString(dr["BatchNo"]),
                            PurchaseRate = "",
                            CurrentStock = Convert.ToString(dr["CurrentStock"]),
                            CurrentStock1 = Convert.ToString(dr["CurrentStock1"]),
                            column = Convert.ToString(dr["A"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return searchList;
        }
    }
}