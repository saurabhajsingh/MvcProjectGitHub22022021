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
    public class BL_PurchaseReturn
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;

        private void Connect()
        {
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        public bool Save(PurchaseReturn obj)
        {
            try
            {

                Connect();
                SqlCommand cmd = new SqlCommand("IUProductPurchaseReturn", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.oldBillNumber == "0" || obj.oldBillNumber == null)
                {
                    cmd.Parameters.AddWithValue("@ProductPurchaseReturnID", 0);
                    cmd.Parameters["@ProductPurchaseReturnID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductPurchaseReturnID", obj.oldBillNumber);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                if (obj.ProductSupplierID == null)
                {
                    cmd.Parameters.AddWithValue("@SupplierID", string.Empty);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SupplierID", obj.ProductSupplierID);
                }

                cmd.Parameters.AddWithValue("@SupplierName", obj.ProductSupplierName);

                cmd.Parameters.AddWithValue("@Address", obj.Address);
                if (obj.remark == null)
                {
                    cmd.Parameters.AddWithValue("@Remark", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Remark", obj.remark);
                }

                if (obj.refBillNumber == null)
                {
                    cmd.Parameters.AddWithValue("@ReffNo", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReffNo", obj.refBillNumber);
                }
                if (obj.taxType == "postingInAccounts")
                {
                    cmd.Parameters.AddWithValue("@IsPostingAccount", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IsPostingAccount", false);
                }

                if (obj.taxType == "adjustmentInInvoice")
                {
                    cmd.Parameters.AddWithValue("@AdjustmentInInvoice", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AdjustmentInInvoice", false);
                }

                if (obj.taxType == "rateInclusiveTax")
                {
                    cmd.Parameters.AddWithValue("@RateInclusiveTax", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RateInclusiveTax", false);
                }

                if (obj.returnDate == null)
                {
                    cmd.Parameters.AddWithValue("@ReturnDate", "");
                }

                else
                {
                    cmd.Parameters.AddWithValue("@ReturnDate", obj.returnDate);
                }

                if (obj.refBillDate == null)
                {
                    cmd.Parameters.AddWithValue("@ReffBillDate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReffBillDate", obj.refBillDate);
                }

                if (obj.grossTotal == null)
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", obj.grossTotal);
                }

                if (obj.otherAdj == null)
                {
                    cmd.Parameters.AddWithValue("@OtherLess", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OtherLess", obj.otherAdj);
                }
                if (obj.netAmount == null)
                {
                    cmd.Parameters.AddWithValue("@NetAmount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NetAmount", obj.netAmount);
                }
                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                int i = cmd.ExecuteNonQuery();

                string ProductPurchaseReturnID = cmd.Parameters["@ProductPurchaseReturnID"].Value.ToString();
                con.Close();



                if (i > 0)
                {
                    string[] pr = obj.ProductID.Split(',');

                    for (int k = 0; k < pr.Length; k++)
                    {
                        string[] nm = obj.UserRate.Split(',');
                        string dateExpary = obj.ExpiryDate.Split(',')[k];

                        SqlCommand cmd1 = new SqlCommand("IUProductPurchaseReturnDetails", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd1.Parameters.AddWithValue("@ProductPurchaseReturnID", Convert.ToInt32(ProductPurchaseReturnID));
                        cmd1.Parameters.AddWithValue("@ProductPurchaseReturnDetailID", 0);
                        cmd1.Parameters.AddWithValue("@Mode", "Add");


                        cmd1.Parameters.AddWithValue("@UseRate", obj.UserRate.Split(',')[k]);


                        cmd1.Parameters.AddWithValue("@Category", obj.Category.Split(',')[k]);

                        cmd1.Parameters.AddWithValue("@Barcode", obj.Barcode);


                        cmd1.Parameters.AddWithValue("@ProductName", obj.ProductName.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@ProductID", Convert.ToInt32(obj.ProductID.ToString().Split(',')[k]));
                        cmd1.Parameters.AddWithValue("@ProductDetailID", obj.ProductDetailsID);
                        cmd1.Parameters.AddWithValue("@Batch", obj.BatchNo.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@ExpiryDate", null);
                        cmd1.Parameters.AddWithValue("@Quantity", obj.Qty.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@Scheme", obj.Scheme.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@DiscountInPer", obj.DisCountPer.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@LessByPer", obj.LessByPer.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@SchemeDiscountInPer", obj.SchemeDiscountInPer.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@Rate", obj.Rate.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@TaxTypeID", 0);
                        cmd1.Parameters.AddWithValue("@PostingAccount", 0);
                        cmd1.Parameters.AddWithValue("@TaxAccount", 0);
                        cmd1.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount.Split(',')[k]);
                        cmd1.Parameters.AddWithValue("@Remark", obj.remark);

                        cmd1.Parameters.AddWithValue("@CreationID", UserID);

                        con.Open();
                        int j = cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return true;


        }

        public DataSet BindProductSupl(string Name)
        {
            DataSet ds = new DataSet();
            try
            {
                if (Name == "")
                {
                    Name = "%";
                }
                KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
                Connect();
                SqlCommand cmd = new SqlCommand("select ProductSupplierID  , ProductSupplierName , ShortName ,ProductSupplier.Address ,DLNo,FoodLicNo,CreditDay,ProductSupplier.CSTNo, ProductSupplier.TINDate,Accounts.AccountName ,ContactPerson from  ProductSupplier  left join Accounts On Accounts.AccountsID = ProductSupplier.AccountsID where  ProductSupplier.RowStatus = 0 and ProductSupplier.HospitalID = " + HospitalID + "  and ProductSupplier.LocationID = " + LocationID + "  order by ProductSupplierName asc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet BindProductName(string Name)
        {
            DataSet ds = new DataSet();

            KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
            Connect();
            SqlCommand cmd = new SqlCommand("select ProductID,ProductName,ProductGroup.ProductGroupName,Packing,Contain,ProductLowerUnitName,SellLoose,GenericInformation.GenericName ,MfrName,MaxQtyLevel,Scheduled,CategoryID,BarCode,DiscontinueDate,ExtraField,DetailWithExtraField from Product left join ProductGroup on ProductGroup.ProductGroupID = Product.ProductGroupID  left join GenericInformation on GenericInformation.GenericID  = Product.GenericID  where ProductName like '" + Name + "%' and Product.HospitalID =1 and  Product.LocationID =1  and Product.RowStatus=0  order by Product.ProductName", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindBatchNoOfProduct(int ProductID)
        {
            DataSet ds = new DataSet();

            KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
            Connect();
            SqlCommand cmd = new SqlCommand("select  ProductDetailsID ,BatchNo,ExpiryDate, PurchaseRate,MRP,SalesRate,CurrentStock ,CurrentStock1  from ProductDetails left join Product  on  Product .ProductID = ProductDetails.ProductID where Product.HospitalID = " + HospitalID + " and Product.LocationID = " + LocationID + " and ProductDetails.ProductID =" + ProductID + "  and ProductDetails.RowStatus = 0 order by BatchNo asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }


        public DataSet BindPostingAccount(int ProductID)
        {
            DataSet ds = new DataSet();

            KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
            Connect();
            SqlCommand cmd = new SqlCommand("select AccountsID, AccountName, PrintName, Address, MobileNo, PhoneNo, EmailID from Accounts where HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 order by AccountName asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindPurchaseTax()
        {
            DataSet ds = new DataSet();

            KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
            Connect();
            SqlCommand cmd = new SqlCommand("select TaxTypeInformationID,Code from TaxInformation where TaxTypeInformation= 'Purchase' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  Discription asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }



        public DataSet BindPostingAccount()
        {
            DataSet ds = new DataSet();

            KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
            Connect();
            SqlCommand cmd = new SqlCommand("select AccountsID, AccountName, PrintName, Address, MobileNo, PhoneNo, EmailID from Accounts where HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 order by AccountName asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        #region Get Product Purchase For Product Deatils
        public DataSet GetProductQuantity(int ProductID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ProductID", SqlDbType.Int);
                param[0].Value = ProductID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProduct", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;


        }
        public DataSet GetProductPurchaseForProductDeatil(int ProductDetailsID)
        {


            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@ProductDetailsID", SqlDbType.Int);
                param[2].Value = ProductDetailsID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductPurchaseForProductDeatil", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;

        }

        #endregion

        #region Get Product Purchase Return OLD Bills NO Details
        public DataSet GetProductPurchaseReturnOLDBillsNO(int SupplierID)
        {
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@SupplierID", SqlDbType.Int);
                param[2].Value = SupplierID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductPurchaseReturnOLDBillsNO", param);

            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }
        #endregion

        #region Get Product Pruchase Return Details
        public DataSet GetProductPurchaseReturn(int BillNo)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductPurchaseReturn", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetProductSupplierData(int ProductSupplierID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ProductSupplierID", SqlDbType.Int);
                param[0].Value = ProductSupplierID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = ProductSupplierID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = ProductSupplierID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductSupplier", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetProductPurchaseReturnDetails(int BillNo)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductPurchaseReturnDetails", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        #endregion

        #region Get Product Purchase
        public DataSet GetProductPurchase(int ProductSupplierID)
        {
            try
            {
                Connect();

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@ProductSupplierID", SqlDbType.Int);
                param[2].Value = ProductSupplierID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductPurchase", param);

            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }
        #endregion
    }
}