using KeystoneProject.Models.PharmacyMaster;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.PharmacyMaster
{
    public class BL_ProductSupplier
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
        public DataSet GetCity(string CityName, string City)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
                Connect();
                SqlCommand cmd = new SqlCommand("select  CityID,CityName from City where  CityName like '" + CityName + "%" + "' and   CityID  like  '" + City + "%" + "' and  HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "' and RowStatus=0 order by  CityName asc", con);
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
        public DataTable GetCountryStateID(int CityID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("getCountryStateID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", @LocationID);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public bool CheckProductSupplier(string ProductSupplierID, string ProductSupplierName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckProductSupplier", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ProductSupplierID", ProductSupplierID);
                cmd.Parameters.AddWithValue("@ProductSupplierName", ProductSupplierName.ToUpper());
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
        public DataSet BindPostindAccount(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
                Connect();
                SqlCommand cmd = new SqlCommand("select AccountsID,AccountName from MedicalAccounts where  AccountName like '" + prefix + "%" + "' and RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " order by AccountName asc", con);
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
        public List<ProductSupplier> ShowAllProductSupplier()
        {
            Connect();
            List<ProductSupplier> productSupplier = new List<ProductSupplier>();
            SqlCommand cmd = new SqlCommand("GetAllProductSupplier", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                productSupplier.Add(
                    new ProductSupplier
                    {
                        ProductSupplierID = Convert.ToString(dr["ProductSupplierID"]),
                        ProductSupplierName = Convert.ToString(dr["ProductSupplierName"]),
                        AccountName = Convert.ToString(dr["AccountName"]),
                    });
            }
            return productSupplier;
        }
        public DataSet BindCompanyName(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.PharmacyMaster.ProductSupplier obj = new ProductSupplier();
                Connect();
                SqlCommand cmd = new SqlCommand("select ProductCompanyNameID,ProductCompanyName from ProductCompany where  ProductCompanyName like '" + prefix + "%" + "' and  RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " order by ProductCompanyName asc", con);
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
        public bool Save(ProductSupplier obj)
        {
            try
            {

                Connect();
                SqlCommand cmd = new SqlCommand("IUProductSupplier", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.ProductSupplierID == "0" || obj.ProductSupplierID == null || obj.ProductSupplierID == "")
                {
                    cmd.Parameters.AddWithValue("@ProductSupplierID", 0);
                    cmd.Parameters["@ProductSupplierID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductSupplierID", obj.ProductSupplierID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                if (obj.ReferenceCode == null)
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                }

                cmd.Parameters.AddWithValue("@ProductSupplierName", obj.ProductSupplierName);

                cmd.Parameters.AddWithValue("@ShortName", obj.ShortName);
                if (obj.DLNo == null)
                {
                    cmd.Parameters.AddWithValue("@DLNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DLNo", obj.DLNo);
                }

                if (obj.FoodLicNo == null)
                {
                    cmd.Parameters.AddWithValue("@FoodLicNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FoodLicNo", obj.FoodLicNo);
                }

                if (obj.CreditDay == null)
                {
                    cmd.Parameters.AddWithValue("@CreditDay", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CreditDay", obj.CreditDay);
                }

                if (obj.CSTNo == null)
                {
                    cmd.Parameters.AddWithValue("@CSTNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CSTNo", obj.CSTNo);
                }

                if (obj.TINNO == null)
                {
                    cmd.Parameters.AddWithValue("@TINNO", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TINNO", obj.TINNO);
                }

                if (obj.TINDate == null)
                {
                    cmd.Parameters.AddWithValue("@TINDate", 0);
                }

                else
                {
                    cmd.Parameters.AddWithValue("@TINDate", obj.TINDate);
                }

                if (obj.AccountsID == null)
                {
                    cmd.Parameters.AddWithValue("@AccountsID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AccountsID", obj.AccountsID);
                }

                if (obj.ContactPerson == null)
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                }

                if (obj.Satus == null)
                {
                    cmd.Parameters.AddWithValue("@Satus", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Satus", obj.Satus);
                }


                if (obj.Days == null)
                {
                    cmd.Parameters.AddWithValue("@Days", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Days", obj.Days);

                }
                if (obj.Address == null)
                {
                    cmd.Parameters.AddWithValue("@Address", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                }

                if (obj.CityID == null)
                {
                    cmd.Parameters.AddWithValue("@CityID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CityID", obj.CityID);
                }
                if (obj.PinCode == null)
                {
                    cmd.Parameters.AddWithValue("@PinCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
                }

                if (obj.StateID == null)
                {
                    cmd.Parameters.AddWithValue("@StateID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StateID", obj.StateID);
                }
                if (obj.CountryID == null)
                {
                    cmd.Parameters.AddWithValue("@CountryID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
                }
                if (obj.PhoneNo == null)
                {
                    cmd.Parameters.AddWithValue("@PhoneNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNo);
                }

                if (obj.MobileNo == null)
                {
                    cmd.Parameters.AddWithValue("@MobileNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                }
                if (obj.EmailID == null)
                {
                    cmd.Parameters.AddWithValue("@EmailID", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                }

                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                int i = cmd.ExecuteNonQuery();

                obj.ProductSupplierID = Convert.ToString(cmd.Parameters["@ProductSupplierID"].Value);
                con.Close();

                string[] CompanyNameID = obj.ProductCompanyNameID1.Split(',');
                string[] CompanyName = obj.ProductCompanyName1.Split(',');
                string[] CashDiscount = obj.cashDiscount1.Split(',');
                string[] CreditDiscount = obj.creditDiscount1.Split(',');
                string[] LessBy = obj.lessBy1.Split(',');

                if (i > 0)
                {

                    for (int k = 0; k < CompanyName.Length; k++)
                    {
                        SqlCommand cmd1 = new SqlCommand("IUProductSupplierDetail", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                        if (obj.ProductSupplierDetailID == null || obj.ProductSupplierDetailID == "")
                        {
                            cmd1.Parameters.AddWithValue("@ProductSupplierDetailID", 0);

                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@ProductSupplierDetailID", obj.ProductSupplierDetailID);
                            cmd1.Parameters.AddWithValue("@Mode", "Edit");
                        }

                        if (obj.ReferenceCode == null)
                        {
                            cmd1.Parameters.AddWithValue("@ReferenceCode", string.Empty);
                        }
                        else 
                        {
                            cmd1.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                        }

                        cmd1.Parameters.AddWithValue("@ProductSupplierID", obj.ProductSupplierID);

                        cmd1.Parameters.AddWithValue("@ProductSupplierName", obj.ProductSupplierName);


                        cmd1.Parameters.AddWithValue("@ProductCompanyNameID", CompanyNameID[k]);
                        cmd1.Parameters.AddWithValue("@ProductCompanyName", CompanyName[k]);
                        cmd1.Parameters.AddWithValue("@CashDiscount", CashDiscount[k]);
                        cmd1.Parameters.AddWithValue("@CreditDiscount", CreditDiscount[k]);
                        cmd1.Parameters.AddWithValue("@LessBy", LessBy[k]);

                        cmd1.Parameters.AddWithValue("@CreationID", UserID);

                        con.Open();
                        int j = cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }

            catch (Exception ex)
            { }

            return true;


        }
        public List<ProductSupplier> GetProductSupplier(int ProductSupplierID)
        {
            Connect();

            List<ProductSupplier> productSupplier = new List<ProductSupplier>();

            SqlCommand cmd = new SqlCommand("GetProductSupplier", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@ProductSupplierID", ProductSupplierID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                productSupplier.Add(
                    new ProductSupplier
                    {
                        ProductSupplierDetailID = Convert.ToString(dr["ProductSupplierDetailID"]),
                        ProductSupplierID = Convert.ToString(dr["ProductSupplierID"]),
                        ProductSupplierName = Convert.ToString(dr["ProductSupplierName"]),
                        ShortName = Convert.ToString(dr["ShortName"]),
                        DLNo = Convert.ToString(dr["DLNo"]),
                        FoodLicNo = Convert.ToString(dr["FoodLicNo"]),
                        CreditDay = Convert.ToString(dr["CreditDay"]),
                        CSTNo = Convert.ToString(dr["CSTNo"]),
                        TINNO = Convert.ToString(dr["TINNO"]),
                        TINDate = Convert.ToString(dr["TINDate"]),
                        AccountsID = Convert.ToString(dr["AccountsID"]),
                        AccountName = Convert.ToString(dr["AccountName"]),
                        ContactPerson = Convert.ToString(dr["ContactPerson"]),
                        Satus = Convert.ToString(dr["Satus"]),
                        Days = Convert.ToString(dr["Days"]),
                        Address = Convert.ToString(dr["Address"]),
                        CityID = Convert.ToString(dr["CityID"]),
                        PinCode = Convert.ToString(dr["PinCode"]),
                        StateID = Convert.ToString(dr["StateID"]),
                        CountryID = Convert.ToString(dr["CountryID"]),
                        CityName = Convert.ToString(dr["CityName"]),
                        CountryName = Convert.ToString(dr["CountryName"]),
                        StateName = Convert.ToString(dr["StateName"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        Pan = Convert.ToString(dr["Pan"]),
                        EmailID = Convert.ToString(dr["EmailID"]),
                        ProductCompanyNameID1 = Convert.ToString(dr["ProductCompanyNameID"]),
                        ProductCompanyName1 = Convert.ToString(dr["ProductCompanyName"]),
                        cashDiscount1 = Convert.ToString(dr["CashDiscount"]),
                        creditDiscount1 = Convert.ToString(dr["CreditDiscount"]),
                        lessBy1 = Convert.ToString(dr["LessBy"]),
                       
                    });
            }
            return productSupplier;
        }
        public string Delete(int ProductSupplierID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteProductSupplier", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductSupplierID", ProductSupplierID);
                
                con.Open();
                int i = cmd.ExecuteNonQuery();

                con.Close();
                if (i > 0)
                {
                    return Table;
                }
                else
                {
                    return Table;
                }
            }
            catch (Exception ex)
            {
                return Table;
            }
        }
    }
}