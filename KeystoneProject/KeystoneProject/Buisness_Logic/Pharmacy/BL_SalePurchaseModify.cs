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
    public class BL_SalePurchaseModify
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        SalePurchaseModify obj = new SalePurchaseModify();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet BindProductName(string Name)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select ProductID,ProductName,ProductGroup.ProductGroupName,Packing,Contain,ProductLowerUnitName,SellLoose,GenericInformation.GenericName ,MfrName,MaxQtyLevel,Scheduled,CategoryID,BarCode,DiscontinueDate,ExtraField,DetailWithExtraField from Product left join ProductGroup on ProductGroup.ProductGroupID = Product.ProductGroupID  left join GenericInformation on GenericInformation.GenericID  = Product.GenericID  where Product.HospitalID =" + HospitalID + " and  Product.LocationID =" + LocationID + "  and Product.RowStatus=0 and ProductName like '" + Name + "%' order by Product.ProductName", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetBatchNo(int ProductID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@PRODUCTID", SqlDbType.Int);
                param[0].Value = ProductID;
                param[1] = new SqlParameter("@HOSPITALID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LOCATIONID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GET_BATCH_NO", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public List<SalePurchaseModify> FillBillDetails(string batch, string prodID, string type, DateTime fromdt, DateTime todt)
        {

            string from = fromdt.ToString("yyyy-MM-dd");
            string to = todt.ToString("yyyy-MM-dd");

            List<SalePurchaseModify> li_medBill = new List<SalePurchaseModify>();

            if (batch != "All")
            {
                if (type == "Sale")
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand("Get_Sale_Details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "Perticular");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProductID", prodID);
                    cmd.Parameters.AddWithValue("@BatchNo", batch);
                    cmd.Parameters.AddWithValue("@FromDate", from);
                    cmd.Parameters.AddWithValue("@ToDate", to);

                    cmd.CommandTimeout = 9000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Close();
                    da.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        li_medBill.Add(
                            new SalePurchaseModify
                            {
                                tblProductID = Convert.ToString(dr["ProductID"]),
                                tblProductName = Convert.ToString(dr["ProductName"]),
                                tblbatchNumber = Convert.ToString(dr["BatchNo"]),
                                tblbillNo = Convert.ToString(dr["BillNo"]),
                                tblbillDate = Convert.ToString(dr["BillDate"]),
                                tblpurchaseRate = Convert.ToString(dr["SalesRate"]),
                                tblquantity = Convert.ToString(dr["Quantity"]),
                                tblamount = Convert.ToString(dr["TotalAmount"]),
                            });
                    }
                    return li_medBill;
                }
                else if (type == "Purchase")
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand("Get_Pruchase_Details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "Perticular");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProductID", prodID);
                    cmd.Parameters.AddWithValue("@BatchNo", batch);
                    cmd.Parameters.AddWithValue("@FromDate", from);
                    cmd.Parameters.AddWithValue("@ToDate", to);

                    cmd.CommandTimeout = 9000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Close();
                    da.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        li_medBill.Add(
                            new SalePurchaseModify
                            {
                                tblProductID = Convert.ToString(dr["ProductID"]),
                                tblProductName = Convert.ToString(dr["ProductName"]),
                                tblbatchNumber = Convert.ToString(dr["BatchNo"]),
                                tblbillNo = Convert.ToString(dr["BillNo"]),
                                tblbillDate = Convert.ToString(dr["BillDate"]),
                                tblpurchaseRate = Convert.ToString(dr["PurchaseRate"]),
                                tblquantity = Convert.ToString(dr["Quantity"]),
                                tblamount = Convert.ToString(dr["TotalAmount"]),
                            });
                    }
                    return li_medBill;
                }
                else
                {
                    return li_medBill;
                }
            }
            else
            {
                if (type == "Sale")
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand("Get_Sale_Details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "All");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProductID", prodID);
                    cmd.Parameters.AddWithValue("@FromDate", from);
                    cmd.Parameters.AddWithValue("@ToDate", to);

                    cmd.CommandTimeout = 9000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Close();
                    da.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        li_medBill.Add(
                            new SalePurchaseModify
                            {
                                tblProductID = Convert.ToString(dr["ProductID"]),
                                tblProductName = Convert.ToString(dr["ProductName"]),
                                tblbatchNumber = Convert.ToString(dr["BatchNo"]),
                                tblbillNo = Convert.ToString(dr["BillNo"]),
                                tblbillDate = Convert.ToString(dr["BillDate"]),
                                tblpurchaseRate = Convert.ToString(dr["SalesRate"]),
                                tblquantity = Convert.ToString(dr["Quantity"]),
                                tblamount = Convert.ToString(dr["TotalAmount"]),
                            });
                    }
                    return li_medBill;
                }
                else if (type == "Purchase")
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand("Get_Pruchase_Details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "All");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProductID", prodID);
                    cmd.Parameters.AddWithValue("@FromDate", from);
                    cmd.Parameters.AddWithValue("@ToDate", to);

                    cmd.CommandTimeout = 9000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Close();
                    da.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        li_medBill.Add(
                            new SalePurchaseModify
                            {
                                tblProductID = Convert.ToString(dr["ProductID"]),
                                tblProductName = Convert.ToString(dr["ProductName"]),
                                tblbatchNumber = Convert.ToString(dr["BatchNo"]),
                                tblbillNo = Convert.ToString(dr["BillNo"]),
                                tblbillDate = Convert.ToString(dr["BillDate"]),
                                tblpurchaseRate = Convert.ToString(dr["PurchaseRate"]),
                                tblquantity = Convert.ToString(dr["Quantity"]),
                                tblamount = Convert.ToString(dr["TotalAmount"]),
                            });
                    }
                    return li_medBill;
                }
                else
                {
                    return li_medBill;
                }
            }
        }

        public bool ModifyBillDetails(string prodID, string ProdNm, string batchno, string billno, string billDt, int purRate, int QTY, int totAmt, string type)
        {
            int old_quantity = 0;
            int new_quantity = 0;

            int i = 0;
            int ProductPurchaseID = 0;
            int MedicalBillID = 0;
            int old_SalesReturn = 0;
            int old_DiscountPercent = 0;
            int old_CashReceived = 0;
            int bill_disc_val = 0;

            int old_disc_amt = 0;
            int old_tax_amt = 0;
            int old_less_amt = 0;

            int old_GST = 0;
            int old_DiscBy = 0;
            var old_DiscountType = "";
            int old_LessBy = 0;
            var old_LessByType = "";
            int old_Prod_totAmt = 0;
            int old_GrossAmount = 0;
            int old_DiscountAmount = 0;
            int old_TaxAmount = 0;
            int old_Bill_totAmt = 0;
            int old_OtherAdg = 0;
            int old_LessCrDr = 0;
            int old_NetAmount = 0;
            int old_BillAmount = 0;


            int new_disc_amt = 0;
            int new_tax_amt = 0;
            int new_less_amt = 0;


            int new_GrossAmount = 0;
            int new_DiscountAmount = 0;
            int new_TaxAmount = 0;
            int new_Bill_totAmt = 0;
            int new_LessCrDr = 0;
            int new_NetAmount = 0;
            int new_BillAmount = 0;

            if (type == "Sale")
            {
                Connect();
                SqlCommand cmd = new SqlCommand("Get_Sale_Bills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "Perticular");
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductID", prodID);
                cmd.Parameters.AddWithValue("@BatchNo", batchno);
                cmd.Parameters.AddWithValue("@BillNo", billno);

                cmd.CommandTimeout = 9000;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Close();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    old_quantity = Convert.ToInt32(dr["Quantity"]);

                    MedicalBillID = Convert.ToInt32(dr["MedicalBillID"]);
                    old_DiscBy = Convert.ToInt32(dr["DiscountPer"]);
                    old_Prod_totAmt = Convert.ToInt32(dr["Prod_totAmt"]);
                    old_GrossAmount = Convert.ToInt32(dr["GrossAmount"]);
                    old_DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]);
                    old_TaxAmount = Convert.ToInt32(dr["TaxAmount"]);
                    old_SalesReturn = Convert.ToInt32(dr["SalesReturn"]);
                    old_Bill_totAmt = Convert.ToInt32(dr["Bill_totAmt"]);
                    old_DiscountPercent = Convert.ToInt32(dr["DiscountPercent"]);
                    old_OtherAdg = Convert.ToInt32(dr["OtherLess"]);
                    old_NetAmount = Convert.ToInt32(dr["NetAmount"]);
                    old_CashReceived = Convert.ToInt32(dr["CashReceived"]);

                    

                    //////************Gross Amount *************************

                    old_GrossAmount = Convert.ToInt32(old_GrossAmount) - Convert.ToInt32(old_Prod_totAmt);
                    new_GrossAmount = Convert.ToInt32(old_GrossAmount) + Convert.ToInt32(totAmt);

                    //////************Gross Amount *************************


                    //////************Product Discount Amount *************************


                    old_disc_amt = Convert.ToInt32((old_Prod_totAmt * old_DiscBy) / 100);
                    new_disc_amt = Convert.ToInt32((totAmt * old_DiscBy) / 100);

                    old_DiscountAmount = Convert.ToInt32(old_DiscountAmount) - Convert.ToInt32(old_disc_amt);
                    new_DiscountAmount = Convert.ToInt32(old_DiscountAmount) + Convert.ToInt32(new_disc_amt);

                    //////************Product Discount Amount  *************************

                    
                    new_Bill_totAmt = Convert.ToInt32(new_GrossAmount) - Convert.ToInt32(new_DiscountAmount) - Convert.ToInt32(old_SalesReturn) + Convert.ToInt32(old_TaxAmount);

                    //////************Bill Discount Amount  *************************

                    bill_disc_val = Convert.ToInt32((new_Bill_totAmt * old_DiscountPercent) / 100);
                   

                    //////************Bill Discount Amount  *************************


                    new_NetAmount = Convert.ToInt32(new_Bill_totAmt) - Convert.ToInt32(old_OtherAdg) - Convert.ToInt32(bill_disc_val);
                    old_CashReceived = new_NetAmount;


                    Connect();
                    SqlCommand cmd1 = new SqlCommand("SALE_MODIFY", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@Flag", "Single_bill");
                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd1.Parameters.AddWithValue("@BillDate", billDt);
                    cmd1.Parameters.AddWithValue("@BillNo", billno);
                    cmd1.Parameters.AddWithValue("@GrossAmount", new_GrossAmount);
                    cmd1.Parameters.AddWithValue("@DiscountAmount", new_DiscountAmount);                    
                    cmd1.Parameters.AddWithValue("@TotalAmount", new_Bill_totAmt);                   
                    cmd1.Parameters.AddWithValue("@NetAmount", new_NetAmount);
                    cmd1.Parameters.AddWithValue("@CashReceived", old_CashReceived);
                    cmd1.Parameters.AddWithValue("@ProductID", prodID);
                    cmd1.Parameters.AddWithValue("@ProductName", ProdNm);
                    cmd1.Parameters.AddWithValue("@BatchNo", batchno);
                    cmd1.Parameters.AddWithValue("@SalesRate", purRate);
                    cmd1.Parameters.AddWithValue("@Quantity", QTY);
                    cmd1.Parameters.AddWithValue("@Prod_TotalAmount", totAmt);
                    cmd1.Parameters.AddWithValue("@MedicalBillID", MedicalBillID);

                    cmd1.CommandTimeout = 9000;
                    con.Open();
                    i = cmd1.ExecuteNonQuery();
                    con.Close();

                    new_quantity = old_quantity - QTY;

                    if (new_quantity > 0)
                    {
                        Connect();
                        SqlCommand cmd2 = new SqlCommand("SALE_PURCHASE_STOCK", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd2.Parameters.AddWithValue("@ProductID", prodID);
                        cmd2.Parameters.AddWithValue("@ProductName", ProdNm);
                        cmd2.Parameters.AddWithValue("@BatchNo", batchno);
                        cmd2.Parameters.AddWithValue("@Quantity", new_quantity);
                        cmd2.Parameters.AddWithValue("@CreationID", UserID);

                        cmd2.CommandTimeout = 9000;
                        con.Open();
                        cmd2.ExecuteNonQuery();
                        con.Close();
                    }

                }
            }
            else if (type == "Purchase")
            {
                Connect();
                SqlCommand cmd = new SqlCommand("Get_Pruchase_Bills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "Perticular");
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductID", prodID);
                cmd.Parameters.AddWithValue("@BatchNo", batchno);
                cmd.Parameters.AddWithValue("@BillNo", billno);

                cmd.CommandTimeout = 9000;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Close();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ProductPurchaseID = Convert.ToInt32(dr["ProductPurchaseID"]);
                    old_GST = Convert.ToInt32(dr["GST"]);
                    old_DiscBy = Convert.ToInt32(dr["DiscBy"]);
                    old_DiscountType = Convert.ToString(dr["DiscountType"]);
                    old_LessBy = Convert.ToInt32(dr["LessBy"]);
                    old_LessByType = Convert.ToString(dr["LessByType"]);
                    old_Prod_totAmt = Convert.ToInt32(dr["Prod_totAmt"]);
                    old_GrossAmount = Convert.ToInt32(dr["GrossAmount"]);
                    old_DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]);
                    old_TaxAmount = Convert.ToInt32(dr["TaxAmount"]);
                    old_Bill_totAmt = Convert.ToInt32(dr["Bill_totAmt"]);
                    old_OtherAdg = Convert.ToInt32(dr["OtherAdg"]);
                    old_LessCrDr = Convert.ToInt32(dr["LessCrDr"]);
                    old_NetAmount = Convert.ToInt32(dr["NetAmount"]);
                    old_BillAmount = Convert.ToInt32(dr["BillAmount"]);




                    //////************Gross Amount *************************

                    old_GrossAmount = Convert.ToInt32(old_GrossAmount) - Convert.ToInt32(old_Prod_totAmt);
                    new_GrossAmount = Convert.ToInt32(old_GrossAmount) + Convert.ToInt32(totAmt);

                    //////************Gross Amount *************************


                    //////************Discount Amount *************************

                    if (old_DiscountType == "%")
                    {
                        old_disc_amt = Convert.ToInt32((old_Prod_totAmt * old_DiscBy) / 100);
                    }

                    if (old_DiscountType == "Rs.")
                    {
                        old_disc_amt = Convert.ToInt32(old_DiscBy);
                    }

                    if (old_DiscountType == "%")
                    {
                        new_disc_amt = Convert.ToInt32((totAmt * old_DiscBy) / 100);
                    }

                    if (old_DiscountType == "Rs.")
                    {
                        new_disc_amt = Convert.ToInt32(old_DiscBy);
                    }

                    old_DiscountAmount = Convert.ToInt32(old_DiscountAmount) - Convert.ToInt32(old_disc_amt);
                    new_DiscountAmount = Convert.ToInt32(old_DiscountAmount) + Convert.ToInt32(new_disc_amt);

                    //////************Discount Amount *************************


                    //////************Tax Amount *************************

                    old_tax_amt = Convert.ToInt32(old_Prod_totAmt * old_GST) / 100;
                    new_tax_amt = Convert.ToInt32(totAmt * old_GST) / 100;

                    old_TaxAmount = Convert.ToInt32(old_TaxAmount) - Convert.ToInt32(old_tax_amt);
                    new_TaxAmount = Convert.ToInt32(old_TaxAmount) + Convert.ToInt32(new_tax_amt);

                    //////************Tax Amount *************************


                    //////************Less Amount *************************

                    if (old_LessByType == "%")
                    {
                        old_less_amt = Convert.ToInt32((old_Prod_totAmt * old_LessBy) / 100);
                    }

                    if (old_LessByType == "Rs.")
                    {
                        old_less_amt = Convert.ToInt32(old_LessBy);
                    }

                    if (old_LessByType == "%")
                    {
                        new_less_amt = Convert.ToInt32((totAmt * old_LessBy) / 100);
                    }

                    if (old_LessByType == "Rs.")
                    {
                        new_less_amt = Convert.ToInt32(old_LessBy);
                    }

                    old_LessCrDr = Convert.ToInt32(old_LessCrDr) - Convert.ToInt32(old_less_amt);
                    new_LessCrDr = Convert.ToInt32(old_LessCrDr) + Convert.ToInt32(new_less_amt);

                    //////************Less Amount *************************


                    new_Bill_totAmt = Convert.ToInt32(new_GrossAmount) - Convert.ToInt32(new_DiscountAmount) + Convert.ToInt32(new_TaxAmount);
                    new_NetAmount = Convert.ToInt32(new_Bill_totAmt) + Convert.ToInt32(old_OtherAdg) - Convert.ToInt32(new_LessCrDr);
                    new_BillAmount = new_NetAmount;


                    Connect();
                    SqlCommand cmd1 = new SqlCommand("PURCHASE_MODIFY", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@Flag", "Single_bill");
                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd1.Parameters.AddWithValue("@BillDate", billDt);
                    cmd1.Parameters.AddWithValue("@BillNo", billno);
                    cmd1.Parameters.AddWithValue("@GrossAmount", new_GrossAmount);
                    cmd1.Parameters.AddWithValue("@DiscountAmount", new_DiscountAmount);
                    cmd1.Parameters.AddWithValue("@TaxAmount", new_TaxAmount);
                    cmd1.Parameters.AddWithValue("@TotalAmount", new_Bill_totAmt);
                    cmd1.Parameters.AddWithValue("@LessCrDr", new_LessCrDr);
                    cmd1.Parameters.AddWithValue("@NetAmount", new_NetAmount);
                    cmd1.Parameters.AddWithValue("@BillAmount", new_BillAmount);
                    cmd1.Parameters.AddWithValue("@ProductID", prodID);
                    cmd1.Parameters.AddWithValue("@ProductName", ProdNm);
                    cmd1.Parameters.AddWithValue("@BatchNo", batchno);
                    cmd1.Parameters.AddWithValue("@PurchaseRate", purRate);
                    cmd1.Parameters.AddWithValue("@Quantity", QTY);
                    cmd1.Parameters.AddWithValue("@Prod_TotalAmount", totAmt);
                    cmd1.Parameters.AddWithValue("@ProductPurchaseID", ProductPurchaseID);

                    cmd1.CommandTimeout = 9000;
                    con.Open();
                    i = cmd1.ExecuteNonQuery();
                    con.Close();

                }
            }

            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteBillDetails(string prodID, string ProdNm, string batchno, string billno, string billDt, int purRate, int QTY, int totAmt, string type)
        {
            int old_quantity = 0;
            int new_quantity = 0;

            int i = 0;
            int ProductPurchaseID = 0;
            int MedicalBillID = 0;
            int old_SalesReturn = 0;
            int old_DiscountPercent = 0;
            int old_CashReceived = 0;            
            int bill_disc_val = 0;

            int old_disc_amt = 0;
            int old_tax_amt = 0;
            int old_less_amt = 0;


            int old_GST = 0;
            int old_DiscBy = 0;
            var old_DiscountType = "";
            int old_LessBy = 0;
            var old_LessByType = "";
            int old_Prod_totAmt = 0;
            int old_GrossAmount = 0;
            int old_DiscountAmount = 0;
            int old_TaxAmount = 0;
            int old_Bill_totAmt = 0;
            int old_OtherAdg = 0;
            int old_LessCrDr = 0;
            int old_NetAmount = 0;
            int old_BillAmount = 0;


            int new_GrossAmount = 0;
            int new_DiscountAmount = 0;
            int new_TaxAmount = 0;
            int new_Bill_totAmt = 0;
            int new_LessCrDr = 0;
            int new_NetAmount = 0;
            int new_BillAmount = 0;

            if (type == "Sale")
            {
                Connect();
                SqlCommand cmd = new SqlCommand("Get_Sale_Bills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "Perticular");
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductID", prodID);
                cmd.Parameters.AddWithValue("@BatchNo", batchno);
                cmd.Parameters.AddWithValue("@BillNo", billno);

                cmd.CommandTimeout = 9000;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Close();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    old_quantity = Convert.ToInt32(dr["Quantity"]);

                    MedicalBillID = Convert.ToInt32(dr["MedicalBillID"]);
                    old_DiscBy = Convert.ToInt32(dr["DiscountPer"]);
                    old_Prod_totAmt = Convert.ToInt32(dr["Prod_totAmt"]);
                    old_GrossAmount = Convert.ToInt32(dr["GrossAmount"]);
                    old_DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]);
                    old_TaxAmount = Convert.ToInt32(dr["TaxAmount"]);
                    old_SalesReturn = Convert.ToInt32(dr["SalesReturn"]);
                    old_Bill_totAmt = Convert.ToInt32(dr["Bill_totAmt"]);
                    old_DiscountPercent = Convert.ToInt32(dr["DiscountPercent"]);
                    old_OtherAdg = Convert.ToInt32(dr["OtherLess"]);
                    old_NetAmount = Convert.ToInt32(dr["NetAmount"]);
                    old_CashReceived = Convert.ToInt32(dr["CashReceived"]);



                    //////************Gross Amount *************************

                    old_GrossAmount = Convert.ToInt32(old_GrossAmount) - Convert.ToInt32(old_Prod_totAmt);
                    new_GrossAmount = Convert.ToInt32(old_GrossAmount);

                    //////************Gross Amount *************************


                    //////************Product Discount Amount *************************


                    old_disc_amt = Convert.ToInt32((old_Prod_totAmt * old_DiscBy) / 100);
                   
                    old_DiscountAmount = Convert.ToInt32(old_DiscountAmount) - Convert.ToInt32(old_disc_amt);
                    new_DiscountAmount = Convert.ToInt32(old_DiscountAmount);

                    //////************Product Discount Amount  *************************


                    new_Bill_totAmt = Convert.ToInt32(new_GrossAmount) - Convert.ToInt32(new_DiscountAmount) - Convert.ToInt32(old_SalesReturn) + Convert.ToInt32(old_TaxAmount);

                    //////************Bill Discount Amount  *************************

                    bill_disc_val = Convert.ToInt32((new_Bill_totAmt * old_DiscountPercent) / 100);


                    //////************Bill Discount Amount  *************************


                    new_NetAmount = Convert.ToInt32(new_Bill_totAmt) - Convert.ToInt32(old_OtherAdg) - Convert.ToInt32(bill_disc_val);
                    old_CashReceived = new_NetAmount;


                    Connect();
                    SqlCommand cmd1 = new SqlCommand("SALE_DELETE", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@Flag", "Single_bill");
                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd1.Parameters.AddWithValue("@BillDate", billDt);
                    cmd1.Parameters.AddWithValue("@BillNo", billno);
                    cmd1.Parameters.AddWithValue("@GrossAmount", new_GrossAmount);
                    cmd1.Parameters.AddWithValue("@DiscountAmount", new_DiscountAmount);
                    cmd1.Parameters.AddWithValue("@TotalAmount", new_Bill_totAmt);
                    cmd1.Parameters.AddWithValue("@NetAmount", new_NetAmount);
                    cmd1.Parameters.AddWithValue("@CashReceived", old_CashReceived);
                    cmd1.Parameters.AddWithValue("@ProductID", prodID);
                    cmd1.Parameters.AddWithValue("@ProductName", ProdNm);
                    cmd1.Parameters.AddWithValue("@BatchNo", batchno);
                    cmd1.Parameters.AddWithValue("@SalesRate", purRate);
                    cmd1.Parameters.AddWithValue("@Quantity", QTY);
                    cmd1.Parameters.AddWithValue("@Prod_TotalAmount", totAmt);
                    cmd1.Parameters.AddWithValue("@MedicalBillID", MedicalBillID);

                    cmd1.CommandTimeout = 9000;
                    con.Open();
                    i = cmd1.ExecuteNonQuery();
                    con.Close();


                    new_quantity = old_quantity;

                    Connect();
                    SqlCommand cmd2 = new SqlCommand("SALE_PURCHASE_STOCK", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd2.Parameters.AddWithValue("@ProductID", prodID);
                    cmd2.Parameters.AddWithValue("@ProductName", ProdNm);
                    cmd2.Parameters.AddWithValue("@BatchNo", batchno);
                    cmd2.Parameters.AddWithValue("@Quantity", new_quantity);
                    cmd2.Parameters.AddWithValue("@CreationID", UserID);

                    cmd2.CommandTimeout = 9000;
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();

                }
            }
            else if (type == "Purchase")
            {
                Connect();
                SqlCommand cmd = new SqlCommand("Get_Pruchase_Bills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "Perticular");
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductID", prodID);
                cmd.Parameters.AddWithValue("@BatchNo", batchno);
                cmd.Parameters.AddWithValue("@BillNo", billno);

                cmd.CommandTimeout = 9000;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Close();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ProductPurchaseID = Convert.ToInt32(dr["ProductPurchaseID"]);
                    old_GST = Convert.ToInt32(dr["GST"]);
                    old_DiscBy = Convert.ToInt32(dr["DiscBy"]);
                    old_DiscountType = Convert.ToString(dr["DiscountType"]);
                    old_LessBy = Convert.ToInt32(dr["LessBy"]);
                    old_LessByType = Convert.ToString(dr["LessByType"]);
                    old_Prod_totAmt = Convert.ToInt32(dr["Prod_totAmt"]);
                    old_GrossAmount = Convert.ToInt32(dr["GrossAmount"]);
                    old_DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]);
                    old_TaxAmount = Convert.ToInt32(dr["TaxAmount"]);
                    old_Bill_totAmt = Convert.ToInt32(dr["Bill_totAmt"]);
                    old_OtherAdg = Convert.ToInt32(dr["OtherAdg"]);
                    old_LessCrDr = Convert.ToInt32(dr["LessCrDr"]);
                    old_NetAmount = Convert.ToInt32(dr["NetAmount"]);
                    old_BillAmount = Convert.ToInt32(dr["BillAmount"]);




                    //////************Gross Amount *************************

                    old_GrossAmount = Convert.ToInt32(old_GrossAmount) - Convert.ToInt32(old_Prod_totAmt);
                    new_GrossAmount = Convert.ToInt32(old_GrossAmount);

                    //////************Gross Amount *************************


                    //////************Discount Amount *************************

                    if (old_DiscountType == "%")
                    {
                        old_disc_amt = Convert.ToInt32((old_Prod_totAmt * old_DiscBy) / 100);
                    }

                    if (old_DiscountType == "Rs.")
                    {
                        old_disc_amt = Convert.ToInt32(old_DiscBy);
                    }

                    old_DiscountAmount = Convert.ToInt32(old_DiscountAmount) - Convert.ToInt32(old_disc_amt);
                    new_DiscountAmount = Convert.ToInt32(old_DiscountAmount);

                    //////************Discount Amount *************************


                    //////************Tax Amount *************************

                    old_tax_amt = Convert.ToInt32(old_Prod_totAmt * old_GST) / 100;

                    old_TaxAmount = Convert.ToInt32(old_TaxAmount) - Convert.ToInt32(old_tax_amt);
                    new_TaxAmount = Convert.ToInt32(old_TaxAmount);

                    //////************Tax Amount *************************


                    //////************Less Amount *************************

                    if (old_LessByType == "%")
                    {
                        old_less_amt = Convert.ToInt32((old_Prod_totAmt * old_LessBy) / 100);
                    }

                    if (old_LessByType == "Rs.")
                    {
                        old_less_amt = Convert.ToInt32(old_LessBy);
                    }

                    old_LessCrDr = Convert.ToInt32(old_LessCrDr) - Convert.ToInt32(old_less_amt);
                    new_LessCrDr = Convert.ToInt32(old_LessCrDr);

                    //////************Less Amount *************************


                    new_Bill_totAmt = Convert.ToInt32(new_GrossAmount) - Convert.ToInt32(new_DiscountAmount) + Convert.ToInt32(new_TaxAmount);
                    new_NetAmount = Convert.ToInt32(new_Bill_totAmt) + Convert.ToInt32(old_OtherAdg) - Convert.ToInt32(new_LessCrDr);
                    new_BillAmount = new_NetAmount;


                    Connect();
                    SqlCommand cmd1 = new SqlCommand("PURCHASE_DELETE", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@Flag", "Single_bill");
                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd1.Parameters.AddWithValue("@BillDate", billDt);
                    cmd1.Parameters.AddWithValue("@BillNo", billno);
                    cmd1.Parameters.AddWithValue("@GrossAmount", new_GrossAmount);
                    cmd1.Parameters.AddWithValue("@DiscountAmount", new_DiscountAmount);
                    cmd1.Parameters.AddWithValue("@TaxAmount", new_TaxAmount);
                    cmd1.Parameters.AddWithValue("@TotalAmount", new_Bill_totAmt);
                    cmd1.Parameters.AddWithValue("@LessCrDr", new_LessCrDr);
                    cmd1.Parameters.AddWithValue("@NetAmount", new_NetAmount);
                    cmd1.Parameters.AddWithValue("@BillAmount", new_BillAmount);
                    cmd1.Parameters.AddWithValue("@ProductID", prodID);
                    cmd1.Parameters.AddWithValue("@ProductName", ProdNm);
                    cmd1.Parameters.AddWithValue("@BatchNo", batchno);
                    cmd1.Parameters.AddWithValue("@PurchaseRate", purRate);
                    cmd1.Parameters.AddWithValue("@Quantity", QTY);
                    cmd1.Parameters.AddWithValue("@Prod_TotalAmount", totAmt);
                    cmd1.Parameters.AddWithValue("@ProductPurchaseID", ProductPurchaseID);

                    cmd1.CommandTimeout = 9000;
                    con.Open();
                    i = cmd1.ExecuteNonQuery();
                    con.Close();

                }
            }

            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteAllBillDetails(string batch, string prodID, string type, DateTime fromdt, DateTime todt)
        {
            int old_quantity = 0;
            int new_quantity = 0;
            string ProdNm = "";

            string from = fromdt.ToString("yyyy-MM-dd");
            string to = todt.ToString("yyyy-MM-dd");

            int i = 0;
            int ProductPurchaseID = 0;
            int billno = 0;
            string billdt = "";
            int BatchNumber = 0;
            int MedicalBillID = 0;
            int old_SalesReturn = 0;
            int old_DiscountPercent = 0;
            int old_CashReceived = 0;
            int bill_disc_val = 0;

            int old_disc_amt = 0;
            int old_tax_amt = 0;
            int old_less_amt = 0;

            int old_GST = 0;
            int old_DiscBy = 0;
            var old_DiscountType = "";
            int old_LessBy = 0;
            var old_LessByType = "";
            int old_Prod_totAmt = 0;
            int old_GrossAmount = 0;
            int old_DiscountAmount = 0;
            int old_TaxAmount = 0;
            int old_Bill_totAmt = 0;
            int old_OtherAdg = 0;
            int old_LessCrDr = 0;
            int old_NetAmount = 0;
            int old_BillAmount = 0;


            int new_GrossAmount = 0;
            int new_DiscountAmount = 0;
            int new_TaxAmount = 0;
            int new_Bill_totAmt = 0;
            int new_LessCrDr = 0;
            int new_NetAmount = 0;
            int new_BillAmount = 0;


            if (batch != "All")
            {
                if (type == "Sale")
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand("Get_Sale_Bills", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "All_Batch");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProductID", prodID);
                    cmd.Parameters.AddWithValue("@BatchNo", batch);
                    cmd.Parameters.AddWithValue("@FromDate", from);
                    cmd.Parameters.AddWithValue("@ToDate", to);

                    cmd.CommandTimeout = 9000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Close();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        old_quantity = Convert.ToInt32(dr["Quantity"]);

                        MedicalBillID = Convert.ToInt32(dr["MedicalBillID"]);
                        billno = Convert.ToInt32(dr["BillNo"]);
                        billdt = Convert.ToString(dr["BillDate"]);
                        ProdNm = Convert.ToString(dr["ProductName"]);
                        BatchNumber = Convert.ToInt32(dr["BatchNo"]);
                        old_DiscBy = Convert.ToInt32(dr["DiscountPer"]);
                        old_Prod_totAmt = Convert.ToInt32(dr["Prod_totAmt"]);
                        old_GrossAmount = Convert.ToInt32(dr["GrossAmount"]);
                        old_DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]);
                        old_TaxAmount = Convert.ToInt32(dr["TaxAmount"]);
                        old_SalesReturn = Convert.ToInt32(dr["SalesReturn"]);
                        old_Bill_totAmt = Convert.ToInt32(dr["Bill_totAmt"]);
                        old_DiscountPercent = Convert.ToInt32(dr["DiscountPercent"]);
                        old_OtherAdg = Convert.ToInt32(dr["OtherLess"]);
                        old_NetAmount = Convert.ToInt32(dr["NetAmount"]);
                        old_CashReceived = Convert.ToInt32(dr["CashReceived"]);



                        //////************Gross Amount *************************

                        old_GrossAmount = Convert.ToInt32(old_GrossAmount) - Convert.ToInt32(old_Prod_totAmt);
                        new_GrossAmount = Convert.ToInt32(old_GrossAmount);

                        //////************Gross Amount *************************


                        //////************Product Discount Amount *************************


                        old_disc_amt = Convert.ToInt32((old_Prod_totAmt * old_DiscBy) / 100);

                        old_DiscountAmount = Convert.ToInt32(old_DiscountAmount) - Convert.ToInt32(old_disc_amt);
                        new_DiscountAmount = Convert.ToInt32(old_DiscountAmount);

                        //////************Product Discount Amount  *************************


                        new_Bill_totAmt = Convert.ToInt32(new_GrossAmount) - Convert.ToInt32(new_DiscountAmount) - Convert.ToInt32(old_SalesReturn) + Convert.ToInt32(old_TaxAmount);

                        //////************Bill Discount Amount  *************************

                        bill_disc_val = Convert.ToInt32((new_Bill_totAmt * old_DiscountPercent) / 100);


                        //////************Bill Discount Amount  *************************


                        new_NetAmount = Convert.ToInt32(new_Bill_totAmt) - Convert.ToInt32(old_OtherAdg) - Convert.ToInt32(bill_disc_val);
                        old_CashReceived = new_NetAmount;


                        Connect();
                        SqlCommand cmd1 = new SqlCommand("SALE_DELETE", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@Flag", "All_Batch");
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd1.Parameters.AddWithValue("@BillDate", billdt);
                        cmd1.Parameters.AddWithValue("@BillNo", billno);
                        cmd1.Parameters.AddWithValue("@GrossAmount", new_GrossAmount);
                        cmd1.Parameters.AddWithValue("@DiscountAmount", new_DiscountAmount);
                        cmd1.Parameters.AddWithValue("@TotalAmount", new_Bill_totAmt);
                        cmd1.Parameters.AddWithValue("@NetAmount", new_NetAmount);
                        cmd1.Parameters.AddWithValue("@CashReceived", old_CashReceived);
                        cmd1.Parameters.AddWithValue("@ProductID", prodID);                        
                        cmd1.Parameters.AddWithValue("@BatchNo", BatchNumber);                        
                        cmd1.Parameters.AddWithValue("@MedicalBillID", MedicalBillID);

                        cmd1.CommandTimeout = 9000;
                        con.Open();
                        i = cmd1.ExecuteNonQuery();
                        con.Close();


                        new_quantity = old_quantity;

                        Connect();
                        SqlCommand cmd2 = new SqlCommand("SALE_PURCHASE_STOCK", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd2.Parameters.AddWithValue("@ProductID", prodID);
                        cmd2.Parameters.AddWithValue("@ProductName", ProdNm);
                        cmd2.Parameters.AddWithValue("@BatchNo", BatchNumber);
                        cmd2.Parameters.AddWithValue("@Quantity", new_quantity);
                        cmd2.Parameters.AddWithValue("@CreationID", UserID);

                        cmd2.CommandTimeout = 9000;
                        con.Open();
                        cmd2.ExecuteNonQuery();
                        con.Close();

                    }
                }
                else if (type == "Purchase")
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand("Get_Pruchase_Bills", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "All_Batch");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProductID", prodID);
                    cmd.Parameters.AddWithValue("@BatchNo", batch);
                    cmd.Parameters.AddWithValue("@FromDate", from);
                    cmd.Parameters.AddWithValue("@ToDate", to);

                    cmd.CommandTimeout = 9000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Close();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        ProductPurchaseID = Convert.ToInt32(dr["ProductPurchaseID"]);
                        billno = Convert.ToInt32(dr["BillNo"]);
                        billdt = Convert.ToString(dr["BillDate"]);
                        BatchNumber = Convert.ToInt32(dr["BatchNo"]);
                        old_GST = Convert.ToInt32(dr["GST"]);
                        old_DiscBy = Convert.ToInt32(dr["DiscBy"]);
                        old_DiscountType = Convert.ToString(dr["DiscountType"]);
                        old_LessBy = Convert.ToInt32(dr["LessBy"]);
                        old_LessByType = Convert.ToString(dr["LessByType"]);
                        old_Prod_totAmt = Convert.ToInt32(dr["Prod_totAmt"]);
                        old_GrossAmount = Convert.ToInt32(dr["GrossAmount"]);
                        old_DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]);
                        old_TaxAmount = Convert.ToInt32(dr["TaxAmount"]);
                        old_Bill_totAmt = Convert.ToInt32(dr["Bill_totAmt"]);
                        old_OtherAdg = Convert.ToInt32(dr["OtherAdg"]);
                        old_LessCrDr = Convert.ToInt32(dr["LessCrDr"]);
                        old_NetAmount = Convert.ToInt32(dr["NetAmount"]);
                        old_BillAmount = Convert.ToInt32(dr["BillAmount"]);




                        //////************Gross Amount *************************

                        old_GrossAmount = Convert.ToInt32(old_GrossAmount) - Convert.ToInt32(old_Prod_totAmt);
                        new_GrossAmount = Convert.ToInt32(old_GrossAmount);

                        //////************Gross Amount *************************


                        //////************Discount Amount *************************

                        if (old_DiscountType == "%")
                        {
                            old_disc_amt = Convert.ToInt32((old_Prod_totAmt * old_DiscBy) / 100);
                        }

                        if (old_DiscountType == "Rs.")
                        {
                            old_disc_amt = Convert.ToInt32(old_DiscBy);
                        }

                        old_DiscountAmount = Convert.ToInt32(old_DiscountAmount) - Convert.ToInt32(old_disc_amt);
                        new_DiscountAmount = Convert.ToInt32(old_DiscountAmount);

                        //////************Discount Amount *************************


                        //////************Tax Amount *************************

                        old_tax_amt = Convert.ToInt32(old_Prod_totAmt * old_GST) / 100;

                        old_TaxAmount = Convert.ToInt32(old_TaxAmount) - Convert.ToInt32(old_tax_amt);
                        new_TaxAmount = Convert.ToInt32(old_TaxAmount);

                        //////************Tax Amount *************************


                        //////************Less Amount *************************

                        if (old_LessByType == "%")
                        {
                            old_less_amt = Convert.ToInt32((old_Prod_totAmt * old_LessBy) / 100);
                        }

                        if (old_LessByType == "Rs.")
                        {
                            old_less_amt = Convert.ToInt32(old_LessBy);
                        }

                        old_LessCrDr = Convert.ToInt32(old_LessCrDr) - Convert.ToInt32(old_less_amt);
                        new_LessCrDr = Convert.ToInt32(old_LessCrDr);

                        //////************Less Amount *************************


                        new_Bill_totAmt = Convert.ToInt32(new_GrossAmount) - Convert.ToInt32(new_DiscountAmount) + Convert.ToInt32(new_TaxAmount);
                        new_NetAmount = Convert.ToInt32(new_Bill_totAmt) + Convert.ToInt32(old_OtherAdg) - Convert.ToInt32(new_LessCrDr);
                        new_BillAmount = new_NetAmount;


                        Connect();
                        SqlCommand cmd1 = new SqlCommand("PURCHASE_DELETE", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@Flag", "All_Batch");
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd1.Parameters.AddWithValue("@BillDate", billdt);
                        cmd1.Parameters.AddWithValue("@BillNo", billno);
                        cmd1.Parameters.AddWithValue("@GrossAmount", new_GrossAmount);
                        cmd1.Parameters.AddWithValue("@DiscountAmount", new_DiscountAmount);
                        cmd1.Parameters.AddWithValue("@TaxAmount", new_TaxAmount);
                        cmd1.Parameters.AddWithValue("@TotalAmount", new_Bill_totAmt);
                        cmd1.Parameters.AddWithValue("@LessCrDr", new_LessCrDr);
                        cmd1.Parameters.AddWithValue("@NetAmount", new_NetAmount);
                        cmd1.Parameters.AddWithValue("@BillAmount", new_BillAmount);
                        cmd1.Parameters.AddWithValue("@ProductID", prodID);
                        cmd1.Parameters.AddWithValue("@BatchNo", BatchNumber);
                        cmd1.Parameters.AddWithValue("@ProductPurchaseID", ProductPurchaseID);

                        cmd1.CommandTimeout = 9000;
                        con.Open();
                        i = cmd1.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }
            else
            {
                if (type == "Sale")
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand("Get_Sale_Bills", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "All");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProductID", prodID);
                    cmd.Parameters.AddWithValue("@FromDate", from);
                    cmd.Parameters.AddWithValue("@ToDate", to);

                    cmd.CommandTimeout = 9000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Close();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        old_quantity = Convert.ToInt32(dr["Quantity"]);

                        MedicalBillID = Convert.ToInt32(dr["MedicalBillID"]);
                        billno = Convert.ToInt32(dr["BillNo"]);
                        billdt = Convert.ToString(dr["BillDate"]);
                        ProdNm = Convert.ToString(dr["ProductName"]);
                        BatchNumber = Convert.ToInt32(dr["BatchNo"]);
                        old_DiscBy = Convert.ToInt32(dr["DiscountPer"]);
                        old_Prod_totAmt = Convert.ToInt32(dr["Prod_totAmt"]);
                        old_GrossAmount = Convert.ToInt32(dr["GrossAmount"]);
                        old_DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]);
                        old_TaxAmount = Convert.ToInt32(dr["TaxAmount"]);
                        old_SalesReturn = Convert.ToInt32(dr["SalesReturn"]);
                        old_Bill_totAmt = Convert.ToInt32(dr["Bill_totAmt"]);
                        old_DiscountPercent = Convert.ToInt32(dr["DiscountPercent"]);
                        old_OtherAdg = Convert.ToInt32(dr["OtherLess"]);
                        old_NetAmount = Convert.ToInt32(dr["NetAmount"]);
                        old_CashReceived = Convert.ToInt32(dr["CashReceived"]);



                        //////************Gross Amount *************************

                        old_GrossAmount = Convert.ToInt32(old_GrossAmount) - Convert.ToInt32(old_Prod_totAmt);
                        new_GrossAmount = Convert.ToInt32(old_GrossAmount);

                        //////************Gross Amount *************************


                        //////************Product Discount Amount *************************


                        old_disc_amt = Convert.ToInt32((old_Prod_totAmt * old_DiscBy) / 100);

                        old_DiscountAmount = Convert.ToInt32(old_DiscountAmount) - Convert.ToInt32(old_disc_amt);
                        new_DiscountAmount = Convert.ToInt32(old_DiscountAmount);

                        //////************Product Discount Amount  *************************


                        new_Bill_totAmt = Convert.ToInt32(new_GrossAmount) - Convert.ToInt32(new_DiscountAmount) - Convert.ToInt32(old_SalesReturn) + Convert.ToInt32(old_TaxAmount);

                        //////************Bill Discount Amount  *************************

                        bill_disc_val = Convert.ToInt32((new_Bill_totAmt * old_DiscountPercent) / 100);


                        //////************Bill Discount Amount  *************************


                        new_NetAmount = Convert.ToInt32(new_Bill_totAmt) - Convert.ToInt32(old_OtherAdg) - Convert.ToInt32(bill_disc_val);
                        old_CashReceived = new_NetAmount;


                        Connect();
                        SqlCommand cmd1 = new SqlCommand("SALE_DELETE", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@Flag", "All");
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd1.Parameters.AddWithValue("@BillDate", billdt);
                        cmd1.Parameters.AddWithValue("@BillNo", billno);
                        cmd1.Parameters.AddWithValue("@GrossAmount", new_GrossAmount);
                        cmd1.Parameters.AddWithValue("@DiscountAmount", new_DiscountAmount);
                        cmd1.Parameters.AddWithValue("@TotalAmount", new_Bill_totAmt);
                        cmd1.Parameters.AddWithValue("@NetAmount", new_NetAmount);
                        cmd1.Parameters.AddWithValue("@CashReceived", old_CashReceived);
                        cmd1.Parameters.AddWithValue("@ProductID", prodID);                        
                        cmd1.Parameters.AddWithValue("@BatchNo", BatchNumber);                        
                        cmd1.Parameters.AddWithValue("@MedicalBillID", MedicalBillID);

                        cmd1.CommandTimeout = 9000;
                        con.Open();
                        i = cmd1.ExecuteNonQuery();
                        con.Close();


                        new_quantity = old_quantity;

                        Connect();
                        SqlCommand cmd2 = new SqlCommand("SALE_PURCHASE_STOCK", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd2.Parameters.AddWithValue("@ProductID", prodID);
                        cmd2.Parameters.AddWithValue("@ProductName", ProdNm);
                        cmd2.Parameters.AddWithValue("@BatchNo", BatchNumber);
                        cmd2.Parameters.AddWithValue("@Quantity", new_quantity);
                        cmd2.Parameters.AddWithValue("@CreationID", UserID);

                        cmd2.CommandTimeout = 9000;
                        con.Open();
                        cmd2.ExecuteNonQuery();
                        con.Close();

                    }
                }
                else if (type == "Purchase")
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand("Get_Pruchase_Bills", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "All");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProductID", prodID);
                    cmd.Parameters.AddWithValue("@FromDate", from);
                    cmd.Parameters.AddWithValue("@ToDate", to);

                    cmd.CommandTimeout = 9000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Close();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        ProductPurchaseID = Convert.ToInt32(dr["ProductPurchaseID"]);
                        billno = Convert.ToInt32(dr["BillNo"]);
                        billdt = Convert.ToString(dr["BillDate"]);
                        BatchNumber = Convert.ToInt32(dr["BatchNo"]);
                        old_GST = Convert.ToInt32(dr["GST"]);
                        old_DiscBy = Convert.ToInt32(dr["DiscBy"]);
                        old_DiscountType = Convert.ToString(dr["DiscountType"]);
                        old_LessBy = Convert.ToInt32(dr["LessBy"]);
                        old_LessByType = Convert.ToString(dr["LessByType"]);
                        old_Prod_totAmt = Convert.ToInt32(dr["Prod_totAmt"]);
                        old_GrossAmount = Convert.ToInt32(dr["GrossAmount"]);
                        old_DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]);
                        old_TaxAmount = Convert.ToInt32(dr["TaxAmount"]);
                        old_Bill_totAmt = Convert.ToInt32(dr["Bill_totAmt"]);
                        old_OtherAdg = Convert.ToInt32(dr["OtherAdg"]);
                        old_LessCrDr = Convert.ToInt32(dr["LessCrDr"]);
                        old_NetAmount = Convert.ToInt32(dr["NetAmount"]);
                        old_BillAmount = Convert.ToInt32(dr["BillAmount"]);




                        //////************Gross Amount *************************

                        old_GrossAmount = Convert.ToInt32(old_GrossAmount) - Convert.ToInt32(old_Prod_totAmt);
                        new_GrossAmount = Convert.ToInt32(old_GrossAmount);

                        //////************Gross Amount *************************


                        //////************Discount Amount *************************

                        if (old_DiscountType == "%")
                        {
                            old_disc_amt = Convert.ToInt32((old_Prod_totAmt * old_DiscBy) / 100);
                        }

                        if (old_DiscountType == "Rs.")
                        {
                            old_disc_amt = Convert.ToInt32(old_DiscBy);
                        }

                        old_DiscountAmount = Convert.ToInt32(old_DiscountAmount) - Convert.ToInt32(old_disc_amt);
                        new_DiscountAmount = Convert.ToInt32(old_DiscountAmount);

                        //////************Discount Amount *************************


                        //////************Tax Amount *************************

                        old_tax_amt = Convert.ToInt32(old_Prod_totAmt * old_GST) / 100;

                        old_TaxAmount = Convert.ToInt32(old_TaxAmount) - Convert.ToInt32(old_tax_amt);
                        new_TaxAmount = Convert.ToInt32(old_TaxAmount);

                        //////************Tax Amount *************************


                        //////************Less Amount *************************

                        if (old_LessByType == "%")
                        {
                            old_less_amt = Convert.ToInt32((old_Prod_totAmt * old_LessBy) / 100);
                        }

                        if (old_LessByType == "Rs.")
                        {
                            old_less_amt = Convert.ToInt32(old_LessBy);
                        }

                        old_LessCrDr = Convert.ToInt32(old_LessCrDr) - Convert.ToInt32(old_less_amt);
                        new_LessCrDr = Convert.ToInt32(old_LessCrDr);

                        //////************Less Amount *************************


                        new_Bill_totAmt = Convert.ToInt32(new_GrossAmount) - Convert.ToInt32(new_DiscountAmount) + Convert.ToInt32(new_TaxAmount);
                        new_NetAmount = Convert.ToInt32(new_Bill_totAmt) + Convert.ToInt32(old_OtherAdg) - Convert.ToInt32(new_LessCrDr);
                        new_BillAmount = new_NetAmount;


                        Connect();
                        SqlCommand cmd1 = new SqlCommand("PURCHASE_DELETE", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@Flag", "All");
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd1.Parameters.AddWithValue("@BillDate", billdt);
                        cmd1.Parameters.AddWithValue("@BillNo", billno);
                        cmd1.Parameters.AddWithValue("@GrossAmount", new_GrossAmount);
                        cmd1.Parameters.AddWithValue("@DiscountAmount", new_DiscountAmount);
                        cmd1.Parameters.AddWithValue("@TaxAmount", new_TaxAmount);
                        cmd1.Parameters.AddWithValue("@TotalAmount", new_Bill_totAmt);
                        cmd1.Parameters.AddWithValue("@LessCrDr", new_LessCrDr);
                        cmd1.Parameters.AddWithValue("@NetAmount", new_NetAmount);
                        cmd1.Parameters.AddWithValue("@BillAmount", new_BillAmount);
                        cmd1.Parameters.AddWithValue("@ProductID", prodID);
                        cmd1.Parameters.AddWithValue("@BatchNo", BatchNumber);
                        cmd1.Parameters.AddWithValue("@ProductPurchaseID", ProductPurchaseID);

                        cmd1.CommandTimeout = 9000;
                        con.Open();
                        i = cmd1.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }

            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}