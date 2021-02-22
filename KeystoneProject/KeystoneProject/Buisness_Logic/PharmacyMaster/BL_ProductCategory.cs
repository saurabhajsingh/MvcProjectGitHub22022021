using KeystoneProject.Models.PharmacyMaster;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic
{
    public class BL_ProductCategory
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<ProductCategory> P_category = new List<ProductCategory>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<ProductCategory> GetData()
        {
            try
            { 
            Connect();
            SqlCommand cmd = new SqlCommand("GetAllProductCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                P_category.Add(
                new ProductCategory
                {
                    categoryName = Convert.ToString(dr["ProductCategoryName"]),
                    categoryID = Convert.ToInt32(dr["ProductCategoryID"]),
                    salesTax = Convert.ToString(dr["Sales"]),
                    purchaseTax = Convert.ToString(dr["Purchase"]),
                    colorVal = Convert.ToString(dr["ColorValue"]),
                    //colorStatus = Convert.ToString(dr["Narration"]),
                    
                });

            }
            }
            catch(Exception ex)
            {

            }
            return P_category;

        }

        public List<ProductCategory> Bind_from_table(int id)
        {
            try
            {
            Connect();
            SqlCommand cmd = new SqlCommand("GetProductCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductCategoryID", id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                DataSet ds = Bind_SaleTax("%", Convert.ToString(dr["SalesTaxID"]));
                DataSet ds1 = Bind_PurchaseTax("%", Convert.ToString(dr["PurchaseTaxID"]));
                P_category.Add(
                new ProductCategory
                {
                    categoryName = Convert.ToString(dr["ProductCategoryName"]),
                    categoryID = Convert.ToInt32(dr["ProductCategoryID"]),
                    salesTax = Convert.ToString(ds.Tables[0].Rows[0]["Discription"]),
                    purchaseTax = Convert.ToString(ds1.Tables[0].Rows[0]["Discription"]),
                    colorVal = Convert.ToString(dr["ColorValue"]),
                    colorStatus = Convert.ToString(dr["ColorSelectionStatus"]),
                    saleTaxID = Convert.ToString(dr["SalesTaxID"]),
                    purchaseTaxID = Convert.ToString(dr["PurchaseTaxID"]),

                });

            }
            }
            catch (Exception ex)
            {

            }
            return P_category;

        }

        public DataSet Bind_SaleTax(string name, string ID )
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select TaxTypeInformationID,Discription from TaxInformation where Discription like '" + name + "%' and TaxTypeInformationID like'" + ID + "%' and  RowStatus=0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and TaxTypeInformation = 'Sales' and Discription <> '' order by TaxTypeInformation asc", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            
            return ds;
        }

        public DataSet Bind_PurchaseTax(string name, string ID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select TaxTypeInformationID,Discription from TaxInformation where Discription like '" + name + "%' and TaxTypeInformationID like'" + ID + "%' and  RowStatus=0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and TaxTypeInformation = 'Purchase' and Discription <> '' order by TaxTypeInformation asc", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            return ds;
        }

        public bool SAVE(ProductCategory obj)
        {
           
            Connect();
            SqlCommand cmd = new SqlCommand("IUProductCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.categoryID == 0)
            {
                cmd.Parameters.AddWithValue("@ProductCategoryID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ProductCategoryID", obj.categoryID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ProductCategoryName", obj.categoryName);
            cmd.Parameters.AddWithValue("@ReferenceCode", 0);
            cmd.Parameters.AddWithValue("@SalesTaxID", obj.saleTaxID);
            cmd.Parameters.AddWithValue("@PurchaseTaxID", obj.purchaseTaxID);
            if (obj.colorVal == "" || obj.colorVal == null)
            {
                cmd.Parameters.AddWithValue("@ColorValue", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ColorValue", obj.colorVal);
            }

            if (obj.colorStatus == "on")
            {
                cmd.Parameters.AddWithValue("@ColorSelectionStatus", 1);
            }
            else if (obj.colorStatus == null)
            {
                cmd.Parameters.AddWithValue("@ColorSelectionStatus", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ColorSelectionStatus", 0);
            }
           
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

        public int Delete_cat(int categoryID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("DeleteProductCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductCategoryID", categoryID);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i;
        }

        public bool Check_productCat(int categoryID, string categoryName)
        {
            int t = 0;
            if (categoryID == 0)
            {
                t = 0;
            }
            else
            {
                t = categoryID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckProductCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ProductCategoryID", t);
                cmd.Parameters.AddWithValue("@ProductCategoryName", categoryName.ToUpper());
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
            catch (Exception)
            {
                return false;
            }
        }


    }
}