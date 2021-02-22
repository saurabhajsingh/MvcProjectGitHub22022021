using KeystoneProject.Models.Pharmacy;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Buisness_Logic.Pharmacy
{
    public class BL_Purchase
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;       
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet BindSupplier(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.Pharmacy.Purchase obj = new Models.Pharmacy.Purchase();
                Connect();
                SqlCommand cmd = new SqlCommand("select ProductSupplierID , ProductSupplierName , ShortName ,ProductSupplier.Address ,DLNo,FoodLicNo,CreditDay,ProductSupplier.CSTNo,ProductSupplier.TINDate,Accounts.AccountName ,ContactPerson from  ProductSupplier  left join Accounts On Accounts.AccountsID = ProductSupplier.AccountsID where  ProductSupplier.RowStatus = 0  and ProductSupplierName  like '" + prefix+"%"+"' and ProductSupplier.HospitalID = " + HospitalID + "  and ProductSupplier.LocationID = " + LocationID + "  order by ProductSupplierName asc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet Bind_productdetail(string productName, string productNameID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("PRODUCT_DETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductID", productNameID + "%");
            cmd.Parameters.AddWithValue("@ProductName", productName + "%");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet GetProductSupplier(string ProductSupplierID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetProductPurchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.Add(new SqlParameter("@ProductSupplierID", ProductSupplierID));
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetProductPurchaseOLDBillsNO(string ProductSupplierID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetProductPurchaseOLDBillsNO", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.Add(new SqlParameter("@ProductSupplierID", ProductSupplierID));
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet BindBatchNoOfProduct(string ProductID)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.Pharmacy.Purchase obj = new Models.Pharmacy.Purchase();
                Connect();
                SqlCommand cmd = new SqlCommand("select ProductDetailsID ,BatchNo,ExpiryDate, PurchaseRate,MRP,SalesRate,CurrentStock ,CurrentStock1,ProductDetails.BarCode  from ProductDetails left join Product  on  Product .ProductID = ProductDetails.ProductID where Product.HospitalID = " + HospitalID + " and Product.LocationID = " + LocationID + " and ProductDetails.ProductID =" + ProductID + "  and ProductDetails.RowStatus = 0 order by ExpiryDate asc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet BindBatchDetails(string productName, string productNameID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("ProductbatchDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductID", productNameID);
            cmd.Parameters.AddWithValue("@ProductName", productName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;

        }

        public DataTable GetProductBatchNo(int productNameID, string batchNumber)
        {
            DataTable dt = new DataTable();
            try
            {
                Connect();

                SqlCommand cmd = new SqlCommand("GetProductBatchNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductID", productNameID);
                cmd.Parameters.AddWithValue("@BatchNo", batchNumber);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                con.Open();
                da.Fill(dt);
                con.Close();
                return dt;

            }
            catch (Exception ex)
            {
                return dt;
            }
        }

        public DataSet GetProductPruchase1(int BillNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductPruchase1", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetProductPruchaseDetails(int BillNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductPruchaseDetails", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public bool Save(Purchase obj)
        {
            try
            {

                Connect();
                SqlCommand cmd = new SqlCommand("IUProductPurchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.ProductPurchaseID == "0" || obj.ProductPurchaseID == null || obj.ProductPurchaseID == "")
                {
                    cmd.Parameters.AddWithValue("@ProductPurchaseID", 0);
                    cmd.Parameters["@ProductPurchaseID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductPurchaseID", obj.ProductPurchaseID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                if (obj.ProductSupplierID == null)
                {
                    cmd.Parameters.AddWithValue("@ProductSupplierID", string.Empty);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductSupplierID", obj.ProductSupplierID);
                }

                cmd.Parameters.AddWithValue("@SupplierName", obj.ProductSupplierName);
                cmd.Parameters.AddWithValue("@Address", obj.Address);

                if (obj.SupplierRemark == null)
                {
                    cmd.Parameters.AddWithValue("@SupplierRemark", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SupplierRemark", obj.SupplierRemark);
                }

                if (obj.billDate == null)
                {
                    cmd.Parameters.AddWithValue("@BillDate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillDate", obj.billDate);
                }

                if (obj.Date == null)
                {
                    cmd.Parameters.AddWithValue("@Date", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Date", obj.Date);
                }

                if (obj.DueDate == null)
                {
                    cmd.Parameters.AddWithValue("@DueDate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DueDate", obj.DueDate);
                }

                if (obj.BillDiscountPercent == null)
                {
                    cmd.Parameters.AddWithValue("@BillDiscountPercent", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillDiscountPercent", 0);
                }

                if (obj.BillNo == null)
                {
                    cmd.Parameters.AddWithValue("@BillNo", "");
                }

                else
                {
                    cmd.Parameters.AddWithValue("@BillNo", obj.BillNo);
                }

                if (obj.grossTotal == null)
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", obj.grossTotal);
                }

                if (obj.discountAmt == null)
                {
                    cmd.Parameters.AddWithValue("@DiscountAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DiscountAmount", obj.discountAmt);
                }

                if (obj.taxAmount == null)
                {
                    cmd.Parameters.AddWithValue("@TaxAmount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxAmount", obj.taxAmount);
                }

                if (obj.totalAmount == null)
                {
                    cmd.Parameters.AddWithValue("@TotalAmount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TotalAmount", obj.totalAmount);
                }

                if (obj.otherAdj == null)
                {
                    cmd.Parameters.AddWithValue("@OtherAdg", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OtherAdg", obj.otherAdj);
                }

                if (obj.lessCreditDebit == null)
                {
                    cmd.Parameters.AddWithValue("@LessCrDr", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LessCrDr", obj.lessCreditDebit);
                }

                if (obj.netAmount == null)
                {
                    cmd.Parameters.AddWithValue("@NetAmount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NetAmount", obj.netAmount);
                }

                if (obj.billAmount == null)
                {
                    cmd.Parameters.AddWithValue("@BillAmount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillAmount", obj.billAmount);
                }

                //extra
                if (obj.PurchaseTaxType == null)
                {
                    cmd.Parameters.AddWithValue("@PurchaseTaxType", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PurchaseTaxType", obj.PurchaseTaxType);
                }

                //extra
                if (obj.PurchaseTax == null)
                {
                    cmd.Parameters.AddWithValue("@PurchaseTax", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PurchaseTax", obj.PurchaseTax);
                }

                if (obj.currentBalance == null)
                {
                    cmd.Parameters.AddWithValue("@CurBalance", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CurBalance", obj.currentBalance);
                }

                //extra
                if (obj.BillType == null)
                {
                    cmd.Parameters.AddWithValue("@BillType", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillType", obj.BillType);
                }

                if (obj.payment_type == null)
                {
                    cmd.Parameters.AddWithValue("@PaymentType", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PaymentType", obj.payment_type);
                }

                if (obj.cheque == null)
                {
                    cmd.Parameters.AddWithValue("@Number", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Number", obj.cheque);
                }

                if (obj.bankName == null)
                {
                    cmd.Parameters.AddWithValue("@Name", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Name", obj.bankName);
                }

                if (obj.chequeDate == null)
                {
                    cmd.Parameters.AddWithValue("@ChequeDate", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ChequeDate", obj.chequeDate);
                }

                //extra
                if (obj.Other == null)
                {
                    cmd.Parameters.AddWithValue("@Other", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Other", obj.Other);
                }

                if (obj.Remarks == null)
                {
                    cmd.Parameters.AddWithValue("@Remarks", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Remarks", obj.Remarks);
                }
               
                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                int i = cmd.ExecuteNonQuery();

                string ProductPurchaseID = cmd.Parameters["@ProductPurchaseID"].Value.ToString();
                con.Close();

                if (i > 0)
                {
                    string[] pr = obj.ProductID1.Split(',');
                    string[] prdctNm = obj.productName1.Split(',');

                    for (int k = 0; k < prdctNm.Length; k++)
                    {
                        if (prdctNm[k].Trim() != "")
                        {
                            SqlCommand cmd1 = new SqlCommand("IUProductPurchaseDetails", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                            cmd1.Parameters.AddWithValue("@ProductPurchaseDetailID", 0);
                            cmd1.Parameters.AddWithValue("@ProductPurchaseID", Convert.ToInt32(ProductPurchaseID));
                            cmd1.Parameters.AddWithValue("@ProductID", Convert.ToInt32(obj.ProductID1.ToString().Split(',')[k]));
                            cmd1.Parameters.AddWithValue("@ProductName", obj.productName1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@BatchNo", obj.batchNumber1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@ProductDetailID", obj.ProductDetailsID1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@Expiry", obj.expiry1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@HSNCode", obj.HSNSACCode1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@PurchaseRate", obj.purchaseRate1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@MRPRate", obj.mrp1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@SaleRate", obj.salesRate1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@Quantity", obj.quantity1.Split(',')[k]);

                            cmd1.Parameters.AddWithValue("@GST", obj.gst1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@SGST", obj.sgst1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@CGST", obj.cgst1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@UTGST", obj.utgst1.Split(',')[k]);

                            cmd1.Parameters.AddWithValue("@FreeProductID", 0);
                            cmd1.Parameters.AddWithValue("@FreeProductDetailID", 0);
                            cmd1.Parameters.AddWithValue("@FreeQuantity", obj.free1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@TaxRate", 0);
                            cmd1.Parameters.AddWithValue("@DiscBy", obj.discount1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@DiscountType", obj.discountSymbol1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@LessBy", obj.lessBy1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@LessByType", obj.lessBySymbol1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@TotalAmount", obj.totalamount1.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@ProductLocationID", 0);
                            cmd1.Parameters.AddWithValue("@ProductLocation", 0);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                            con.Open();
                            int j = cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return true;


        }

        public DataSet Bind_Newproductdetail(string productName, string productNameID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("PRODUCT_DETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductID", productNameID);
            cmd.Parameters.AddWithValue("@ProductName", "%");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataTable Bind_PackQtyRate(int productNameID)
        {
            DataTable dt = new DataTable();
            try
            {
                Connect();

                SqlCommand cmd = new SqlCommand("Get_Product_Detail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductID", productNameID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                con.Open();
                da.Fill(dt);
                con.Close();
                return dt;

            }
            catch (Exception ex)
            {
                return dt;
            }
        }

        public DataSet Bind_HSN(string HSNSACCode)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select distinct HSNCODE from  HSNCODENEW  where HSNCODE like '" + HSNSACCode + "%' and RowStatus=0 and COMPANYID =" + HospitalID + " and LOCATIONID =" + LocationID + "  order by  HSNCODE asc", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet Bind_HSNID(string HSNSACCode, string HSNSACCodeID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("BIND_HSNCODE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@COMPANYID", HospitalID);
            cmd.Parameters.AddWithValue("@LOCATIONID", LocationID);
            cmd.Parameters.AddWithValue("@FLAG", HSNSACCode);
            cmd.Parameters.AddWithValue("@HSNCODE", HSNSACCode + "%");
            cmd.Parameters.AddWithValue("@HSNCODEID", HSNSACCodeID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet Bind_GSTPer(string HSNcodeID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetHSNCode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@HSNCodeID", HSNcodeID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet btnImport_Click(string path)
        {
            DataSet myDataSet = new DataSet();
            DataTable dataTable = new DataTable();
            string aa = path;
            
            String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"", aa);          
            using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
            {
                OleDbCommand cmd1 = new OleDbCommand("Select * from [Exported from Keystone]", excelConnection);
                {
                    excelConnection.Open();
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM  [Sheet1$]", excelConnString);

                    dataAdapter.Fill(myDataSet, "ProductDetails");
                    dataTable = myDataSet.Tables["ProductDetails"];
                    
                    //SaveExcel(myDataSet);

                    //using (SqlBulkCopy sqlBulk = new SqlBulkCopy(con))
                    //{
                    //    excelConnection.Close();
                    //    con.Close();

                    //}
                }            
            }
            return myDataSet;
        }

        public int DeletePurchase(int ProductPurchaseID)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("Delete_Purchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductPrID", ProductPurchaseID);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();

            }
            return i;
        }

    }
}