using KeystoneProject.Models.PharmacyMaster;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace KeystoneProject.Buisness_Logic
{
    public class BL_Product
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<Product> List_product = new List<Product>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<Product> GetData()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    List_product.Add(
                    new Product
                    {
                        productCompany = Convert.ToString(dr["ProductCompanyName"]),
                        productName = Convert.ToString(dr["ProductName"]),
                        productNameID = Convert.ToString(dr["ProductID"]),
                        packing = Convert.ToString(dr["Packing"]),
                        HSNSACCode = Convert.ToString(dr["HSNSACCode"]),
                        manufacturerName = Convert.ToString(dr["MfrName"]),
                    });

                }
            }
            catch (Exception ex)
            {

            }
            return List_product;
        }

        public List<Product> Bind_From_Table(int productNameID)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductID", productNameID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                string productCompany = "";
                string generic = "";
                string category = "";
                                
                foreach (DataRow dr in dt.Rows)
                {
                    DataSet ds1 = Bind_Prod_Company("%", Convert.ToString(dr["ProductGroupID"]));
                    DataSet ds2 = Bind_Generic("%", Convert.ToString(dr["GenericID"]));
                    DataSet ds3 = Bind_Category("%", Convert.ToString(dr["CategoryID"]));

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        productCompany = Convert.ToString(ds1.Tables[0].Rows[0]["ProductCompanyName"]);
                    }

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        generic = Convert.ToString(ds2.Tables[0].Rows[0]["GenericName"]);
                    }

                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        category = Convert.ToString(ds3.Tables[0].Rows[0]["ProductCategoryName"]);
                    }

                    List_product.Add(
                    new Product
                    {
                        productNameID = Convert.ToString(dr["ProductID"]),
                        productName = Convert.ToString(dr["ProductName"]),
                        productCompanyID = Convert.ToString(dr["ProductGroupID"]),
                        packingID = Convert.ToString(dr["ProductUnitID"]),
                        packing = Convert.ToString(dr["Packing"]),
                        contains = Convert.ToString(dr["Contain"]),
                        containsQuantity = Convert.ToString(dr["ProductLowerUnitName"]),
                        sellLoose = Convert.ToString(dr["SellLoose"]),
                        genericID = Convert.ToString(dr["GenericID"]),    
                        HSNSACCode = Convert.ToString(dr["HSNSACCode"]),
                        SKUCode = Convert.ToString(dr["SKUCode"]),
                        chapterCode = Convert.ToString(dr["ChapterCode"]),
                        manufacturerName = Convert.ToString(dr["MfrName"]),
                        maximumLevel = Convert.ToString(dr["MaxQtyLevel"]),
                        minimumLevel = Convert.ToString(dr["MinQtyLevel"]),
                        scheduled = Convert.ToString(dr["Scheduled"]),
                        categoryID = Convert.ToString(dr["CategoryID"]),
                        barCode = Convert.ToString(dr["BarCode"]),
                        discontinueDate = Convert.ToString(dr["DiscontinueDate"]),
                        extrafield = Convert.ToString(dr["ExtraField"]),
                        detailextrafield = Convert.ToString(dr["DetailWithExtraField"]),
                        gst = Convert.ToString(dr["GST"]),
                        sgst = Convert.ToString(dr["SGST"]),
                        cgst = Convert.ToString(dr["CGST"]),
                        utgst = Convert.ToString(dr["UTGST"]),
                        cess = Convert.ToString(dr["CESS"]),

                    });

                }

            }
            catch (Exception ex)
            {

            }
            return List_product;
        }
        public bool SAVE(Product obj)
        {
            try
            {
                Connect();

                SqlCommand cmd = new SqlCommand("IUProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.productNameID == "0" || obj.productNameID == null || obj.productNameID == "")
                {
                    cmd.Parameters.AddWithValue("@ProductID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "ADD");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductID", obj.productNameID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                cmd.Parameters.AddWithValue("@ProductName", obj.productName.Trim());
                cmd.Parameters.AddWithValue("@ReferenceCode", 0);

                if (obj.productCompanyID == null || obj.productCompanyID == "")
                {
                    cmd.Parameters.AddWithValue("@ProductGroupID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductGroupID", obj.productCompanyID);
                }

                if (obj.packingID == null || obj.packingID == "")
                {
                    cmd.Parameters.AddWithValue("@ProductUnitID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductUnitID", obj.packingID);
                }

                if (obj.packing == null || obj.packing == "")
                {
                    cmd.Parameters.AddWithValue("@Packing", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Packing", obj.packing);
                }

                if (obj.contains == null || obj.contains == "")
                {
                    cmd.Parameters.AddWithValue("@Contain", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Contain", obj.contains);
                }

                if (obj.containsQuantity == null || obj.containsQuantity == "")
                {
                    cmd.Parameters.AddWithValue("@ProductLowerUnitName", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductLowerUnitName", obj.containsQuantity);
                }

                if (obj.sellLoose == "on" || obj.sellLoose == "True")
                {
                    cmd.Parameters.AddWithValue("@SellLoose", true);
                }
                else if (obj.sellLoose == null)
                {
                    cmd.Parameters.AddWithValue("@SellLoose", false);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SellLoose", false);
                }

                if (obj.genericID == null || obj.genericID == "")
                {
                    cmd.Parameters.AddWithValue("@GenericID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GenericID", obj.genericID);
                }

                if (obj.SKUCode == null || obj.SKUCode == "")
                {
                    cmd.Parameters.AddWithValue("@SKUCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SKUCode", obj.SKUCode);
                }

                if (obj.HSNSACCode == null || obj.HSNSACCode == "")
                {
                    cmd.Parameters.AddWithValue("@HSNSACCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HSNSACCode", obj.HSNSACCode);
                }

                if (obj.chapterCode == null || obj.chapterCode == "")
                {
                    cmd.Parameters.AddWithValue("@ChapterCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ChapterCode", obj.chapterCode);
                }

                if (obj.manufacturerName == null || obj.manufacturerName == "")
                {
                    cmd.Parameters.AddWithValue("@MfrName", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MfrName", obj.manufacturerName);
                }

                if (obj.maximumLevel == null || obj.maximumLevel == "")
                {
                    cmd.Parameters.AddWithValue("@MaxQtyLevel", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MaxQtyLevel", obj.maximumLevel);
                }

                if (obj.minimumLevel == null || obj.minimumLevel == "")
                {
                    cmd.Parameters.AddWithValue("@MinQtyLevel", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MinQtyLevel", obj.minimumLevel);
                }

                if (obj.scheduled == "on" || obj.scheduled == "True")
                {
                    cmd.Parameters.AddWithValue("@Scheduled", true);
                }
                else if (obj.scheduled == null)
                {
                    cmd.Parameters.AddWithValue("@Scheduled", false);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Scheduled", false);
                }

                if (obj.categoryID == null || obj.categoryID == "")
                {
                    cmd.Parameters.AddWithValue("@CategoryID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CategoryID", obj.categoryID);
                }

                cmd.Parameters.AddWithValue("@ShelfID", 0);

                if (obj.barCode == null || obj.barCode == "")
                {
                    cmd.Parameters.AddWithValue("@BarCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BarCode", obj.barCode);
                }

                cmd.Parameters.AddWithValue("@Code", 0);
                cmd.Parameters.AddWithValue("@DiscontinueDate", obj.discontinueDate);

                if (obj.extrafield == null || obj.extrafield == "")
                {
                    cmd.Parameters.AddWithValue("@ExtraField", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ExtraField", obj.extrafield);
                }

                if (obj.detailextrafield == "on" || obj.detailextrafield == "True")
                {
                    cmd.Parameters.AddWithValue("@DetailWithExtraField", true);
                }
                else if (obj.detailextrafield == null)
                {
                    cmd.Parameters.AddWithValue("@DetailWithExtraField", false);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DetailWithExtraField", false);
                }

                cmd.Parameters.AddWithValue("@GST", obj.gst);
                cmd.Parameters.AddWithValue("@SGST", obj.sgst);
                cmd.Parameters.AddWithValue("@CGST", obj.cgst);
                cmd.Parameters.AddWithValue("@UTGST", obj.utgst);
                cmd.Parameters.AddWithValue("@CESS", obj.cess);
                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public DataSet Bind_Prod_Company(string productCompany, string productCompanyID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select ProductCompanyNameID,ltrim(rtrim(ProductCompanyName)) as ProductCompanyName from  ProductCompany  where ProductCompanyName like '" + productCompany + "%' and ProductCompanyNameID like '" + productCompanyID + "%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  ProductCompanyName asc", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }
        public DataSet Bind_Manufctr_name(string manufacturerName, string manufacturerNameID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("SELECT ManufactureID, ManufactureName FROM Manufacture where ManufactureName like '" + manufacturerName + "%" + "' and ManufactureID like '" + manufacturerNameID + "%' and RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " order by  ManufactureName asc", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet Bind_Packing(string packing, string packingID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select ProductUnitID,ltrim(rtrim(ProductUnitName)) as ProductUnitName from  ProductUnit  where ProductUnitName like '" + packing + "%' and ProductUnitID like '" + packingID + "%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  ProductUnitName asc", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet Bind_Generic(string generic, string genericID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select GenericID,ltrim(rtrim(GenericName)) as GenericName from  GenericInformation where GenericName like '" + generic + "%' and GenericID like '" + genericID + "%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  GenericName asc", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet Bind_Category(string category, string categoryID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  ProductCategoryID,ltrim(rtrim(ProductCategoryName)) as ProductCategoryName from  ProductCategory  where ProductCategoryName like '" + category + "%' and ProductCategoryID like '" + categoryID + "%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  ProductCategoryName asc", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
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

        public bool CheckProduct(string productNameID, string productName)
        {
            string t = "0";
            if (productNameID == null || productNameID == "0")
            {
                t = "0";
            }
            else
            {
                t = productNameID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ProductID", t);
                cmd.Parameters.AddWithValue("@ProductName", productName.ToUpper());
                cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@NameExists"].Value;
                if (Convert.ToInt32(Table) == 1)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
                return flag;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int Delete(int id)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("DeleteProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductID", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
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

    }
}