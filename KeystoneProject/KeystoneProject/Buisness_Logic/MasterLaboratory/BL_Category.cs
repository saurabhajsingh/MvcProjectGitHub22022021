using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using KeystoneProject.Models.Master;
using KeystoneProject.Models.MasterLaboratory;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_Category
    {
       

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<Category> categorylist = new List<Category>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }


        public List<Category> SelectAllCategory()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllCategory", con);
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
                categorylist.Add(
                    new Category
                    {
                        CategoryID = Convert.ToInt32(dr["CategoryID"]),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        Description = Convert.ToString(dr["Description"]),
                        level = Convert.ToString(dr["level"]),
                        ParentCategoryName = Convert.ToString(dr["ParentCategoryName"]),
                        HSNCode = dr["HSNCode"].ToString(),
                    });
            }
            return categorylist;


        }

        public List<Category> GetCategory(int CategoryID)
        {
            Connect();
       
             SqlCommand cmd = new SqlCommand("GetCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@CategoryID", CategoryID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                categorylist.Add(
                    new Category
                    {
                        CategoryID = Convert.ToInt32(dr["CategoryID"]),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        Description = Convert.ToString(dr["Description"]),
                        level = Convert.ToString(dr["level"]),
                        ParentCategoryName = Convert.ToString(dr["ParentCategoryName"]),
                        HSNCode = Convert.ToString(dr["HSNCode"]),
                    });
            }
            return categorylist;

            }
        public bool Category(Category obj)
       {
           Connect();

           SqlCommand cmd = new SqlCommand("IUCategory", con);

           cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
           cmd.Parameters.AddWithValue("@LocationID", LocationID);

           if (obj.CategoryID == 0)
           {

               cmd.Parameters.AddWithValue("@CategoryID", 0);
               cmd.Parameters.AddWithValue("@Mode", "Add");
           }
           else
           {
               cmd.Parameters.AddWithValue("@CategoryID", obj.CategoryID);
               cmd.Parameters.AddWithValue("@Mode", "Edit");
           }

           //cmd.Parameters.AddWithValue("@CategoryID", 0);


           if (obj.ReferenceCode == null)
               cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
           else
               cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
           //cmd.Parameters.AddWithValue("@ReferenceCode", 1);
           cmd.Parameters.AddWithValue("@CategoryName", obj.CategoryName);

           if (obj.HSNCode == "" || obj.HSNCode == null)
           {
                cmd.Parameters.AddWithValue("@HSNCode", "");
           }
           else
           {
               cmd.Parameters.AddWithValue("@HSNCode", obj.HSNCode);

           }
           
           cmd.Parameters.AddWithValue("@ParentCategoryID", obj.ParentCategoryID);

           if (obj.ParentCategoryName == null)
           {
               cmd.Parameters.AddWithValue("@ParentCategoryName", "");

           }
           else
           {
               cmd.Parameters.AddWithValue("@ParentCategoryName", obj.ParentCategoryName);

           }



           cmd.Parameters.AddWithValue("@Level", obj.level);

           if (obj.Description == null)
               cmd.Parameters.AddWithValue("@Description", DBNull.Value);
           else
               cmd.Parameters.AddWithValue("@Description", obj.Description);

           //cmd.Parameters.AddWithValue("@Description", obj.Description);
           cmd.Parameters.AddWithValue("@CreationID", UserID);

           //cmd.Parameters.AddWithValue("@Mode", "Add");
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

        public bool CheckCategory(int CategoryID, string CategoryName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName.ToUpper());
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

       #region  UpdateCategory
       //public bool UpdateCategory(KeystoneProject.Models.Master.Category Updatecat)
       //{
       //    SqlConnection con = null;
       //    string result = "";
       //    try
       //    {
       //        con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
       //        SqlCommand cmd = new SqlCommand("IUCategory", con);
       //        cmd.CommandType = CommandType.StoredProcedure;
       //        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
       //        cmd.Parameters.AddWithValue("@LocationID", LocationID);
       //        cmd.Parameters.AddWithValue("@CategoryID", Updatecat.CategoryID);
       //        cmd.Parameters.AddWithValue("@CategoryName", Updatecat.CategoryName);
       //        cmd.Parameters.AddWithValue("@ReferenceCode", 1);
       //        cmd.Parameters.AddWithValue("@ParentCategoryID", Updatecat.ParentCategoryID);
       //        cmd.Parameters.AddWithValue("@ParentCategoryName", Updatecat.ParentCategoryName);
       //        cmd.Parameters.AddWithValue("@Level", Updatecat.level);
       //        cmd.Parameters.AddWithValue("@Description", Updatecat.Description);

       //        cmd.Parameters.AddWithValue("@CreationID", UserID);
       //        cmd.Parameters.AddWithValue("@Mode", "Edit");
       //        con.Open();
       //        cmd.ExecuteNonQuery();
       //        result = cmd.ToString();

       //    }
       //    catch
       //    {

       //    }
       //    finally
       //    {
       //        con.Close();
       //    }
       //    return true;
       //}

       #endregion


        public bool DeleteCategory(int CategoryID)
       {

           Connect();
           SqlParameter[] apram = new SqlParameter[4];
           apram[0] = new SqlParameter("@CategoryID", SqlDbType.Int);
           apram[0].Value = CategoryID;
           apram[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
           apram[1].Value = HospitalID;
           apram[2] = new SqlParameter("@LocationID", SqlDbType.Int);
           apram[2].Value = LocationID;
           con.Open();
           SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeleteCategory", apram);
           con.Close();

           return true;

          
       }
    }
}