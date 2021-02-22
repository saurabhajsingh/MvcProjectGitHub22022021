using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.PharmacyMaster;
using System.Data;
using KeystoneProject.Controllers.PharmacyMaster;


namespace KeystoneProject.Buisness_Logic.PharmacyMaster
{
    public class BL_ProductUnit
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        

        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

      //  int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<ProductUnit> productlist = new List<ProductUnit>();


        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
          public List<ProductUnit> SelectAllData()
        {
             Connect();


            SqlCommand cmd = new SqlCommand("GetAllProductUnit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID",LocationID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                productlist.Add(
                    new ProductUnit
                    {
                        ProductUnitName = (dr["ProductUnitName"].ToString()),
                        ProductUnitID = Convert.ToInt32(dr["ProductUnitID"]),
                        contains = dr["Contain"].ToString(),
                        lowerUnitName = dr["ProductLowerUnitName"].ToString(),
                        sellLoose=dr["SellLoose"].ToString(),


                    });
            }
            return productlist;
        }
        public List<ProductUnit> GETProductUnit(string ProductUnitID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetProductUnit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@ProductUnitID ", ProductUnitID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                productlist.Add(
                    new ProductUnit
                    {
                        ProductUnitName = (dr["ProductUnitName"].ToString()),
                        ProductUnitID = Convert.ToInt32(dr["ProductUnitID"]),
                        lowerUnitName = (dr["ProductLowerUnitName"].ToString()),
                        contains=(dr["Contain"].ToString()),
                        sellLoose=(dr["SellLoose"].ToString())


                    });
            }
            return productlist;

        }
        

        
    public bool Save(ProductUnit obj)
    {
        Connect();
        SqlCommand cmd = new SqlCommand("IUProductUnit", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
        cmd.Parameters.AddWithValue("@LocationID", LocationID);
        cmd.Parameters.AddWithValue("@ProductUnitName", obj.ProductUnitName);
        //cmd.Parameters.AddWithValue("@Contain", obj.contains);
        //cmd.Parameters.AddWithValue("@ProductLowerUnitName", obj.lowerUnitName);
       
if (obj.sellLoose=="on")
            {
                cmd.Parameters.AddWithValue("@SellLoose", true);
            }
else
            {
                cmd.Parameters.AddWithValue("@SellLoose", false);
            }

        if (obj.ProductUnitID == 0)
        {
            cmd.Parameters.AddWithValue("@ProductUnitID", 0);
            cmd.Parameters.AddWithValue("@Mode", "Add");
        }
        else
        {
            cmd.Parameters.AddWithValue("@ProductUnitID", obj.ProductUnitID);
            cmd.Parameters.AddWithValue("@Mode", "Edit");
        }
        cmd.Parameters.AddWithValue("@NumberOfDecimal",0);
        if (obj.RefferenceCode == null)
            cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
        else
            cmd.Parameters.AddWithValue("@ReferenceCode", obj.RefferenceCode);
        cmd.Parameters.AddWithValue("@CreationID", UserID);
            if (obj.contains == null)
                cmd.Parameters.AddWithValue("@Contain", string.Empty);
            else
                cmd.Parameters.AddWithValue("@Contain", obj.contains);
            if (obj.lowerUnitName == null)
                cmd.Parameters.AddWithValue("@ProductLowerUnitName", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ProductLowerUnitName", obj.lowerUnitName);
            


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
    public bool CheckProductUnit (int ProductUnitID, string ProductUnitName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckProductUnit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ProductUnitID", ProductUnitID);
                cmd.Parameters.AddWithValue("@ProductUnitName", ProductUnitName.ToUpper());
               
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
        public int DeleteGenericInformation(int ProductUnitID)
        {
            Connect();
            int delete = 0;
            SqlCommand cmd = new SqlCommand("DeleteProductUnit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductUnitID", ProductUnitID);
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;

        }

    }
   
}