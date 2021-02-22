using KeystoneProject.Models.Pharmacy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Pharmacy
{
    public class BL_PurchaseOrder
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<PurchaseOrder> li_PO = new List<PurchaseOrder>();
        PurchaseOrder mod_PO = new PurchaseOrder();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet Bind_supplier(string supplierName, string supplierNameID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select ProductSupplierID, ProductSupplierName from ProductSupplier where ProductSupplierName like '" + supplierName + "%' and ProductSupplierID like '" + supplierNameID + "%' and RowStatus = 0 and HospitalID = " + HospitalID + "  and LocationID = " + LocationID + "  order by ProductSupplierName asc", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet Bind_PurTypeMode(string purchaseType)
        {
            DataSet ds = new DataSet();

            if (purchaseType == "Assets") {
                Connect();
                SqlCommand cmd = new SqlCommand("select AccountsID ,AccountName , AccountType,Address ,MobileNo,FAX,TinNo from Accounts where ScheduleID in( select ScheduleID from Schedule where ScheduleName in ('CONSUMABLE PRODUCTS')and RowStatus = 0 )and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 ", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(ds);
                con.Close();
            }
            else if (purchaseType == "Consumable") {
                Connect();
                SqlCommand cmd = new SqlCommand("select AccountsID ,AccountName , AccountType,Address ,MobileNo,FAX,TinNo from Accounts where ScheduleID in(select ScheduleID from Schedule where ScheduleName in ('CONSUMABLE PRODUCTS')and RowStatus = 0)and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0  ", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(ds);
                con.Close();
            }
            else if (purchaseType == "Medicine") {
                Connect();
                SqlCommand cmd = new SqlCommand("select AccountsID ,AccountName , AccountType,Address ,MobileNo,FAX,TinNo from Accounts where ScheduleID in( select ScheduleID from Schedule where ScheduleName in ( 'DEPRICIABLE PROPERTY')and RowStatus = 0 )and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 ", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(ds);
                con.Close();
            }
            else if (purchaseType == "Stationary") {
                Connect();
                SqlCommand cmd = new SqlCommand("select AccountsID ,AccountName , AccountType,Address ,MobileNo,FAX,TinNo from Accounts where ScheduleID in(select ScheduleID from Schedule where ScheduleName in ('CONSUMABLE PRODUCTS')and RowStatus = 0)and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0  ", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(ds);
                con.Close();
            }
            else {
                Connect();
                SqlCommand cmd = new SqlCommand("select AccountsID ,AccountName , AccountType,Address ,MobileNo,FAX,TinNo from Accounts where ScheduleID in( select ScheduleID from Schedule where ScheduleName in ( 'MEDICINE PURCHASE')and RowStatus = 0 )and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 ", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(ds);
                con.Close();
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

        public DataSet Bind_Manufacturer(string manufacturerName1)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("SELECT Distinct ManufactureName FROM Manufacture where ManufactureName like '" + manufacturerName1 + "%" + "' and RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " order by  ManufactureName asc", con);
                        
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
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

        public DataTable Getaddress(int supplierNameID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetProductSupplier", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductSupplierID", supplierNameID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
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

        public bool SAVE(PurchaseOrder obj)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("IUProductPurchaseOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.PoId == 0)
                {
                    cmd.Parameters.AddWithValue("@Mode", "ADD");
                    cmd.Parameters.AddWithValue("@ProductOrderID", obj.PoId);
                    cmd.Parameters["@ProductOrderID"].Direction = ParameterDirection.Output;
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Mode", "EDIT");
                    cmd.Parameters.AddWithValue("@ProductOrderID", obj.PoId);
                }

                if (obj.orderDate == "" || obj.orderDate == null)
                {
                    cmd.Parameters.AddWithValue("@OrderDate", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OrderDate", obj.orderDate);
                }

                if (obj.address == "" || obj.address == null)
                {
                    cmd.Parameters.AddWithValue("@Address", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", obj.address);
                }

                cmd.Parameters.AddWithValue("@ProductSupplierID", obj.supplierNameID);

                if (obj.referenceDate == "" || obj.referenceDate == null)
                {
                    cmd.Parameters.AddWithValue("@BillDate", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillDate", obj.referenceDate);
                }

                if (obj.referenceNumber == "" || obj.referenceNumber == null)
                {
                    cmd.Parameters.AddWithValue("@RefNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RefNo", obj.referenceNumber);
                }

                cmd.Parameters.AddWithValue("@ValueBy", obj.valueBy);

                if (obj.grossTotal == "" || obj.grossTotal == null)
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", obj.grossTotal);
                }

                if (obj.addAmount == "" || obj.addAmount == null)
                {
                    cmd.Parameters.AddWithValue("@AddAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AddAmount", obj.addAmount);
                }

                if (obj.lessAmount == "" || obj.lessAmount == null)
                {
                    cmd.Parameters.AddWithValue("@LessAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LessAmount", obj.lessAmount);
                }

                if (obj.netAmount == "" || obj.netAmount == null)
                {
                    cmd.Parameters.AddWithValue("@NetAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NetAmount", obj.netAmount);
                }

                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@PaymentMode", obj.payMode);

                if (obj.creditDays == "" || obj.creditDays == null)
                {
                    cmd.Parameters.AddWithValue("@CreditDays", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CreditDays", obj.creditDays);
                }

                if (obj.purchaseType == "" || obj.purchaseType == null)
                {
                    cmd.Parameters.AddWithValue("@PurchaseType", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PurchaseType", obj.purchaseType);
                }

                if (obj.purchaseTypeMode.Trim() == "" || obj.purchaseTypeMode.Trim() == null)
                {
                    cmd.Parameters.AddWithValue("@PurchaseTypeMode", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PurchaseTypeMode", obj.purchaseTypeMode.Trim());
                }
       
                con.Open();
                int i = cmd.ExecuteNonQuery();
                obj.PoId = Convert.ToInt32(cmd.Parameters["@ProductOrderID"].Value);
                con.Close();

                string[] prdctnameID = obj.productNameID1.Split(',');
                string[] prdctname = obj.productName1.Split(',');
                string[] mfrName = obj.manufacturerName1.Split(',');
                string[] pack = obj.packing1.Split(',');
                string[] curqty = obj.currentQuantity1.Split(',');
                string[] max = obj.maxLevel1.Split(',');
                string[] min = obj.minLevel1.Split(',');
                string[] qty = obj.quantity1.Split(',');
                string[] freeqty = obj.freeQuantity1.Split(',');
                string[] rate = obj.rate1.Split(',');

                int j = 0;

                if (i > 0)
                {
                    for (int k = 0; k < prdctname.Length; k++)
                    {
                        if (prdctname[k].Trim() != "")
                        {
                            string[] totqty = qty[k].Split('.');
                            int qt1 = Convert.ToInt32(totqty[0]);
                            string[] totrate = rate[k].Split('.');
                            int rt1 = Convert.ToInt32(totrate[0]);

                            int totamt = qt1 * rt1;

                            SqlCommand cmd1 = new SqlCommand("IUProductPurchaseOrderDetails", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            if (obj.productDetailsID1 == "" || obj.productDetailsID1 == null)
                            {
                                cmd1.Parameters.AddWithValue("@ProductOrderDetailsID", 0);
                                cmd1.Parameters.AddWithValue("@Mode", "ADD");
                            }

                            cmd1.Parameters.AddWithValue("@ProductOrderID", obj.PoId);
                            cmd1.Parameters.AddWithValue("@ProductID", prdctnameID[k]);
                            cmd1.Parameters.AddWithValue("@ProductName", prdctname[k]);
                            cmd1.Parameters.AddWithValue("@Packing", pack[k]);
                            cmd1.Parameters.AddWithValue("@CurQuantity", curqty[k]);
                            cmd1.Parameters.AddWithValue("@MaxLevel", max[k]);
                            cmd1.Parameters.AddWithValue("@MinLevel", min[k]);
                            cmd1.Parameters.AddWithValue("@Quantity", qty[k]);
                            cmd1.Parameters.AddWithValue("@FreeQuantity", freeqty[k]);
                            cmd1.Parameters.AddWithValue("@Rate", rate[k]);
                            cmd1.Parameters.AddWithValue("@TotalAmount", totamt);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@MfrName", mfrName[k]);

                            con.Open();
                            j = cmd1.ExecuteNonQuery();
                            con.Close();


                        }
                    }
                    if (j > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public DataTable GetOldBill(int supplierNameID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetProductPurchaseOrderOLDBillsNO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@SupplierID", supplierNameID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public List<PurchaseOrder> Bind_from_BillNo(int oldBillNumber)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetProductPurchaseOredr", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BillNo", oldBillNumber);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {

                    DataSet ds = Bind_supplier("%", Convert.ToString(dr["ProductSupplierID"]));
                    li_PO.Add(
                        new PurchaseOrder
                        {
                            PoId = Convert.ToInt32(dr["ProductOrderID"]),
                            orderDate = Convert.ToString(dr["OrderDate"]),
                            address = Convert.ToString(dr["Address"]),
                            supplierNameID = Convert.ToString(dr["ProductSupplierID"]),
                            supplierName = Convert.ToString(ds.Tables[0].Rows[0]["ProductSupplierName"]),
                            referenceDate = Convert.ToString(dr["BillDate"]),
                            referenceNumber = Convert.ToString(dr["RefNo"]),
                            valueBy = Convert.ToString(dr["ValueBy"]),
                            grossTotal = Convert.ToString(dr["GrossAmount"]),
                            addAmount = Convert.ToString(dr["AddAmount"]),
                            lessAmount = Convert.ToString(dr["LessAmount"]),
                            netAmount = Convert.ToString(dr["NetAmount"]),
                            payMode = Convert.ToString(dr["PaymentMode"]),
                            creditDays = Convert.ToString(dr["CreditDays"]),
                            purchaseType = Convert.ToString(dr["PurchaseType"]),
                            purchaseTypeMode = Convert.ToString(dr["PurchaseTypeMode"]),

                        });
                }
            }
            catch (Exception ex)
            {

            }
            return li_PO;
        }

        public List<PurchaseOrder> Bind_from_BillNoProduct(int oldBillNumber)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetProductPurchaseOredrDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BillNo", oldBillNumber);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    //DataSet ds = Bind_productdetail("%", Convert.ToString(dr["ProductID"]));
                    li_PO.Add(
                        new PurchaseOrder
                        {
                            PoId = Convert.ToInt32(dr["ProductOrderID"]),
                            productDetailsID1 = Convert.ToString(dr["ProductOrderDetailsID"]),
                            productNameID1 = Convert.ToString(dr["ProductID"]),
                            productName1 = Convert.ToString(dr["ProductName"]),
                            manufacturerName1 = Convert.ToString(dr["MfrName"]),
                            packing1 = Convert.ToString(dr["Packing"]),
                            currentQuantity1 = Convert.ToString(dr["CurQuantity"]),
                            maxLevel1 = Convert.ToString(dr["MaxLevel"]),
                            minLevel1 = Convert.ToString(dr["MinLevel"]),
                            quantity1 = Convert.ToString(dr["Quantity"]),
                            freeQuantity1 = Convert.ToString(dr["FreeQuantity"]),
                            rate1 = Convert.ToString(dr["Rate"]),
                            totalAmount1 = Convert.ToString(dr["TotalAmount"]),

                        });
                }
            }
            catch (Exception ex)
            {

            }
            return li_PO;
        }

        public DataTable Add_newProduct(string productName)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("IUProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (mod_PO.productNameID1 == "0" || mod_PO.productNameID1 == null || mod_PO.productNameID1 == "")
            {
                cmd.Parameters.AddWithValue("@Mode", "ADD");
                cmd.Parameters.AddWithValue("@ProductID", 0);
            }

            cmd.Parameters.AddWithValue("@ProductName", productName.Trim());
            cmd.Parameters.AddWithValue("@ReferenceCode", 0);
            cmd.Parameters.AddWithValue("@CreationID", UserID);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();


            Connect();
            SqlCommand cmd1 = new SqlCommand("PRODUCT_DETAILS", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
            cmd1.Parameters.AddWithValue("@ProductID", "%");
            cmd1.Parameters.AddWithValue("@ProductName", productName);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            //foreach (DataRow dr in dt.Rows)
            //{               
            //    li_PO.Add(
            //        new PurchaseOrder
            //        {
            //            productNameID = Convert.ToString(dr["ProductOrderID"]),
            //        });
            //}           
            return dt;
        }

        public DataSet Check_newProduct(string productName, string productNameID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("PRODUCT_DETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductID", "%");
            cmd.Parameters.AddWithValue("@ProductName", productName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public int DeletePurchaseOrder(int PoId)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("Delete_Purchase_Order", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@POID", PoId);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();

            }
            return i;
        }
    }
}