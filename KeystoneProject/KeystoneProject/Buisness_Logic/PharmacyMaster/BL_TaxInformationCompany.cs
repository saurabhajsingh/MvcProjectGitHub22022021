using KeystoneProject.Models.PharmacyMaster;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic
{
    public class BL_TaxInformationCompany
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<TaxInformationCompany> list_tax = new List<TaxInformationCompany>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<TaxInformationCompany> GetData()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllTaxInformation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    list_tax.Add(
                    new TaxInformationCompany
                    {
                        taxInformationTypeID = Convert.ToInt32(dr["TaxTypeInformationID"]),
                        code = Convert.ToString(dr["Code"]),
                        taxInformationType = Convert.ToString(dr["TaxTypeInformation"]),
                        taxType = Convert.ToString(dr["TaxType"]),
                        postingAccount = Convert.ToString(dr["AccountName"]),
                        taxInformationDetailTaxRate = Convert.ToString(dr["TaxRate"]),
                    });

                }
            }
            catch (Exception ex)
            {

            }
            return list_tax;

        }

        public DataSet Bind_PostAccount(string postingAccount, string postingAccountID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("SELECT AccountsID , AccountName  FROM Accounts  where AccountName like '" + postingAccount + "%' and AccountsID like '" + postingAccountID + "%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  AccountName  asc", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet Bind_TaxAccount(string postingAccount, string postingAccountID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("SELECT AccountsID , AccountName  FROM Accounts  where AccountName like '" + postingAccount + "%' and AccountsID like '" + postingAccountID + "%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  AccountName  asc", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public DataSet Bind_SrchrgAccount(string postingAccount, string postingAccountID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("SELECT AccountsID , AccountName  FROM Accounts  where AccountName like '" + postingAccount + "%' and AccountsID like '" + postingAccountID + "%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  AccountName  asc", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public bool SAVE(TaxInformationCompany obj)

        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("IUTaxInformation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                if (obj.taxInformationTypeID == 0)
                {
                    cmd.Parameters.AddWithValue("@TaxTypeInformationID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "ADD");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxTypeInformationID", obj.taxInformationTypeID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }
                cmd.Parameters.AddWithValue("@Code", obj.code);

                if (obj.taxInformationType == null || obj.taxInformationType == "False")
                {
                    cmd.Parameters.AddWithValue("@TaxTypeInformation", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxTypeInformation", obj.taxInformationType);
                }

                cmd.Parameters.AddWithValue("@TaxType", obj.taxType);

                if (obj.taxInforamtionTypeTaxRate == null || obj.taxInforamtionTypeTaxRate == "")
                {
                    cmd.Parameters.AddWithValue("@InformationTaxRate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InformationTaxRate", obj.taxInforamtionTypeTaxRate);
                }

                if (obj.interStateTaxRate == null || obj.interStateTaxRate == "")
                {
                    cmd.Parameters.AddWithValue("@InterstateTaxRate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InterstateTaxRate", obj.interStateTaxRate);
                }

                if (obj.interState == "on" || obj.interState == "True")
                {
                    cmd.Parameters.AddWithValue("@InterState", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InterState", false);
                }

                if (obj.formType == null || obj.formType == "")
                {
                    cmd.Parameters.AddWithValue("@FormType", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FormType", obj.formType);
                }

                if (obj.description == null || obj.description == "")
                {
                    cmd.Parameters.AddWithValue("@Discription", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Discription", obj.description);
                }

                if (obj.postingAccountID == null || obj.postingAccountID == "")
                {
                    cmd.Parameters.AddWithValue("@PostingAccount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PostingAccount", obj.postingAccountID);
                }

                if (obj.taxInformationDetailTaxRate == null || obj.taxInformationDetailTaxRate == "")
                {
                    cmd.Parameters.AddWithValue("@TaxRate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxRate", obj.taxInformationDetailTaxRate);
                }

                if (obj.surcharge == null || obj.surcharge == "")
                {
                    cmd.Parameters.AddWithValue("@Surcharge", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Surcharge", obj.surcharge);
                }

                if (obj.rateInclusiveInTax == "on" || obj.rateInclusiveInTax == "True")
                {
                    cmd.Parameters.AddWithValue("@RateInclusive", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RateInclusive", false);
                }

                if (obj.taxInclusiveInRates == null || obj.taxInclusiveInRates == "")
                {
                    cmd.Parameters.AddWithValue("@TaxInclusiveInRates", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxInclusiveInRates", obj.taxInclusiveInRates);
                }

                if (obj.taxInclusiveInRatesForLTE == "on" || obj.taxInclusiveInRatesForLTE == "True")
                {
                    cmd.Parameters.AddWithValue("@TaxInclusiveInRatesForLTE", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxInclusiveInRatesForLTE", false);
                }

                if (obj.taxInformationDetailTaxAccountID == null || obj.taxInformationDetailTaxAccountID == "")
                {
                    cmd.Parameters.AddWithValue("@TaxAC", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxAC", obj.taxInformationDetailTaxAccountID);
                }

                if (obj.surchargeAccountID == null || obj.surchargeAccountID == "")
                {
                    cmd.Parameters.AddWithValue("@SurchargeAC", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SurchargeAC", obj.surchargeAccountID);
                }

                if (obj.addCharge == null || obj.addCharge == "")
                {
                    cmd.Parameters.AddWithValue("@AddICharge", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AddICharge", obj.addCharge);
                }

                if (obj.discount == null || obj.discount == "False")
                {
                    cmd.Parameters.AddWithValue("@Discount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Discount", obj.discount);
                }

                if (obj.addICOnFreeGoods == "on" || obj.addICOnFreeGoods == "True")
                {
                    cmd.Parameters.AddWithValue("@AddICOnFreeGoods", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AddICOnFreeGoods", false);
                }

                if (obj.calculateTaxBeforeDiscount == "on" || obj.calculateTaxBeforeDiscount == "True")
                {
                    cmd.Parameters.AddWithValue("@CalculateTaxBeforeDiscount", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CalculateTaxBeforeDiscount", false);
                }

                if (obj.chargeTaxOnFreeGoods == "on" || obj.chargeTaxOnFreeGoods == "True")
                {
                    cmd.Parameters.AddWithValue("@ChargeTaxOnFreeGoods", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ChargeTaxOnFreeGoods", false);
                }

                if (obj.taxOfFreeGoodsOnGrossValue == "on" || obj.taxOfFreeGoodsOnGrossValue == "True")
                {
                    cmd.Parameters.AddWithValue("@TaxOfFreeGoodsOnGrossValue", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxOfFreeGoodsOnGrossValue", false);
                }

                if (obj.taxCalculateBasedOnMRP == "on" || obj.taxCalculateBasedOnMRP == "True")
                {
                    cmd.Parameters.AddWithValue("@TaxCalculationBasedOnMRP", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxCalculationBasedOnMRP", false);
                }

                if (obj.abatementRebateForLTE == "on" || obj.abatementRebateForLTE == "True")
                {
                    cmd.Parameters.AddWithValue("@AbateMentRebateForLTE", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AbateMentRebateForLTE", false);
                }

                if (obj.abatementRate == null || obj.abatementRate == "")
                {
                    cmd.Parameters.AddWithValue("@AbatementRate", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AbatementRate", obj.abatementRate);
                }

                if (obj.chargePromotionalRate == null || obj.chargePromotionalRate == "")
                {
                    cmd.Parameters.AddWithValue("@ChargePromotionalRate", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ChargePromotionalRate", obj.chargePromotionalRate);
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TaxInformationCompany> Bind_From_Table(int taxInformationTypeID)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetTaxInformation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@TaxTypeInformationID", taxInformationTypeID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    string post = "";
                    string tax = "";
                    string surchrg = "";

                    DataSet ds1 = Bind_PostAccount("%", Convert.ToString(dr["PostingAccount"]));
                    DataSet ds2 = Bind_TaxAccount("%", Convert.ToString(dr["TaxAC"]));
                    DataSet ds3 = Bind_SrchrgAccount("%", Convert.ToString(dr["SurchargeAC"]));

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        post = Convert.ToString(ds1.Tables[0].Rows[0]["AccountName"]);
                    }

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        tax = Convert.ToString(ds2.Tables[0].Rows[0]["AccountName"]);
                    }

                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        surchrg = Convert.ToString(ds3.Tables[0].Rows[0]["AccountName"]);
                    }


                    list_tax.Add(
                    new TaxInformationCompany
                    {
                        taxInformationTypeID = Convert.ToInt32(dr["TaxTypeInformationID"]),
                        code = Convert.ToString(dr["Code"]),
                        taxInformationType = Convert.ToString(dr["TaxTypeInformation"]),
                        taxType = Convert.ToString(dr["TaxType"]),
                        taxInforamtionTypeTaxRate = Convert.ToString(dr["InformationTaxRate"]),
                        interStateTaxRate = Convert.ToString(dr["InterstateTaxRate"]),
                        interState = Convert.ToString(dr["InterState"]),
                        formType = Convert.ToString(dr["FormType"]),
                        description = Convert.ToString(dr["Discription"]),
                        postingAccountID = Convert.ToString(dr["PostingAccount"]),
                        postingAccount = post,
                        taxInformationDetailTaxRate = Convert.ToString(dr["TaxRate"]),
                        surcharge = Convert.ToString(dr["Surcharge"]),
                        rateInclusiveInTax = Convert.ToString(dr["RateInclusive"]),
                        taxInclusiveInRates = Convert.ToString(dr["TaxInclusiveInRates"]),
                        taxInclusiveInRatesForLTE = Convert.ToString(dr["TaxInclusiveInRatesForLTE"]),
                        taxInformationDetailTaxAccountID = Convert.ToString(dr["TaxAC"]),
                        taxInformationDetailTaxAccount = tax,
                        surchargeAccountID = Convert.ToString(dr["SurchargeAC"]),
                        surchargeAccount = surchrg,
                        addCharge = Convert.ToString(dr["AddICharge"]),
                        discount = Convert.ToString(dr["Discount"]),
                        addICOnFreeGoods = Convert.ToString(dr["AddICOnFreeGoods"]),
                        calculateTaxBeforeDiscount = Convert.ToString(dr["CalculateTaxBeforeDiscount"]),
                        chargeTaxOnFreeGoods = Convert.ToString(dr["ChargeTaxOnFreeGoods"]),
                        taxOfFreeGoodsOnGrossValue = Convert.ToString(dr["TaxOfFreeGoodsOnGrossValue"]),
                        taxCalculateBasedOnMRP = Convert.ToString(dr["TaxCalculationBasedOnMRP"]),
                        abatementRebateForLTE = Convert.ToString(dr["AbateMentRebateForLTE"]),
                        abatementRate = Convert.ToString(dr["AbatementRate"]),
                        chargePromotionalRate = Convert.ToString(dr["ChargePromotionalRate"]),
                    });

                }
            }
            catch (Exception ex)
            {

            }
            return list_tax;
        }

        public int Delete_data(int taxInformationTypeID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("DeleteTaxInformation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@TaxTypeInformationID", taxInformationTypeID);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i;
        }

        public bool Check_TaxCompany(int taxInformationTypeID, string code)
        {
            int t = 0;
            if (taxInformationTypeID == 0)
            {
                t = 0;
            }
            else
            {
                t = taxInformationTypeID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckTaxInformation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@TaxTypeInformationID", t);
                cmd.Parameters.AddWithValue("@Code", code.ToUpper());
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