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
    public class BL_ProductCompany
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<ProductCompany> P_company = new List<ProductCompany>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<ProductCompany> GetData()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllProductCompany", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    P_company.Add(
                    new ProductCompany
                    {
                        nameID = Convert.ToInt32(dr["ProductCompanyNameID"]),
                        name = Convert.ToString(dr["ProductCompanyName"]),
                        shortName = Convert.ToString(dr["ShortName"]),
                        salesTax = Convert.ToString(dr["Sales"]),
                        purchaseTax = Convert.ToString(dr["Purchase"]),
                    });

                }
            }
            catch (Exception ex)
            {

            }
            return P_company;

        }

        public DataSet Bind_SaleTax(string name, string ID)
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

        public DataSet BindCity(string CityName, string City)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.PharmacyMaster.ProductCompany obj = new Models.PharmacyMaster.ProductCompany();
                Connect();
                SqlCommand cmd = new SqlCommand("select  CityID,CityName from City where  CityName like '" + CityName + "%" + "' and   CityID  like  '" + City + "%" + "' and RowStatus=0 order by  CityName asc", con);
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
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public List<ProductCompany> Bind_from_table(int id)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetProductCompany", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ProductCompanyNameID", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    DataSet ds1 = Bind_SaleTax("%", Convert.ToString(dr["SalesTaxID"]));
                    DataSet ds2 = Bind_PurchaseTax("%", Convert.ToString(dr["PurchaseTaxID"]));
                    DataSet ds3 = BindCity("%", Convert.ToString(dr["CityID"]));
                    DataTable dt4 = GetCountryStateID(Convert.ToInt32(dr["CityID"]));
                    P_company.Add(
                        new ProductCompany
                        {
                            name = Convert.ToString(dr["ProductCompanyName"]),
                            nameID = Convert.ToInt32(dr["ProductCompanyNameID"]),
                            salesTaxID = Convert.ToString(dr["SalesTaxID"]),
                            salesTax = Convert.ToString(ds1.Tables[0].Rows[0]["Discription"]),
                            purchaseTaxID = Convert.ToString(dr["PurchaseTaxID"]),
                            purchaseTax = Convert.ToString(ds2.Tables[0].Rows[0]["Discription"]),
                            shortName = Convert.ToString(dr["ShortName"]),
                            margin = Convert.ToString(dr["Margin"]),
                            lteProducts = Convert.ToString(dr["LTEProducts"]),
                            ltiProducts = Convert.ToString(dr["LTIProducts"]),
                            Exclusive = Convert.ToString(dr["Exclusive"]),

                            Inclusive = Convert.ToString(dr["Inclusive"]),
                            exclusiveTax = Convert.ToString(dr["ExclusiveTax"]),
                            address = Convert.ToString(dr["Address"]),
                            cityID = Convert.ToString(dr["CityID"]),
                            city = Convert.ToString(ds3.Tables[0].Rows[0]["CityName"]),
                            stateID = Convert.ToString(dr["StateID"]),
                            state = Convert.ToString(dt4.Rows[0]["StateName"]),
                            countryID = Convert.ToString(dr["CountryID"]),
                            country = Convert.ToString(dt4.Rows[0]["CountryName"]),
                            phoneNumber = Convert.ToString(dr["PhoneNo"]),
                            emailid = Convert.ToString(dr["EmailID"]),
                            pinCode = Convert.ToString(dr["PinCode"]),
                            mobileNumber = Convert.ToString(dr["MobileNo"]),
                            
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return P_company;
        }

        public bool SAVE(ProductCompany obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUProductCompany", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.nameID == 0)
            {
                cmd.Parameters.AddWithValue("@ProductCompanyNameID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ProductCompanyNameID", obj.nameID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }

            cmd.Parameters.AddWithValue("@ReferenceCode", 0);
            cmd.Parameters.AddWithValue("@ProductCompanyName", obj.name);
            cmd.Parameters.AddWithValue("@SalesTaxID", obj.salesTaxID);
            cmd.Parameters.AddWithValue("@PurchaseTaxID", obj.purchaseTaxID);
            cmd.Parameters.AddWithValue("@ShortName", obj.shortName);
            cmd.Parameters.AddWithValue("@Margin", obj.margin);
            cmd.Parameters.AddWithValue("@LTEProducts", obj.lteProducts);
            cmd.Parameters.AddWithValue("@LTIProducts", obj.ltiProducts);

            if (obj.radio == null)
            {
                cmd.Parameters.AddWithValue("@Exclusive", false);
                cmd.Parameters.AddWithValue("@Inclusive", false);
            }
            else if(obj.radio == "Exclusive")
            {
                cmd.Parameters.AddWithValue("@Exclusive", true);
                cmd.Parameters.AddWithValue("@Inclusive", false);
            }
            else if (obj.radio == "Inclusive")
            {
                cmd.Parameters.AddWithValue("@Exclusive", false);
                cmd.Parameters.AddWithValue("@Inclusive", true);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Exclusive", false);
                cmd.Parameters.AddWithValue("@Inclusive", false);
            }


            //cmd.Parameters.AddWithValue("@Exclusive", obj.Exclusive);
            //cmd.Parameters.AddWithValue("@Inclusive", obj.Inclusive);
            cmd.Parameters.AddWithValue("@ExclusiveTax", obj.exclusiveTax);
            cmd.Parameters.AddWithValue("@Address", obj.address);
            cmd.Parameters.AddWithValue("@CityID", obj.cityID);
            cmd.Parameters.AddWithValue("@StateID", obj.stateID);
            cmd.Parameters.AddWithValue("@PhoneNo", obj.phoneNumber);
            cmd.Parameters.AddWithValue("@EmailID", obj.emailid);
            cmd.Parameters.AddWithValue("@PinCode", obj.pinCode);
            cmd.Parameters.AddWithValue("@CountryID", obj.countryID);
            cmd.Parameters.AddWithValue("@MobileNo", obj.mobileNumber);
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

        public int Delete_pcomp(int nameID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("DeleteProductCompany", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProductCompanyNameID", nameID);
            
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >0 )
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public bool Check_productCompany(int nameID, string name)
        {
            int t = 0;
            if (nameID == 0)
            {
                t = 0;
            }
            else
            {
                t = nameID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckProductCompany", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ProductCompanyNameID", t);
                cmd.Parameters.AddWithValue("@ProductCompanyName", name.ToUpper());
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