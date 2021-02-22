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
    public class BL_HSNCode
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<HSNCode> li_hsn = new List<HSNCode>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<HSNCode> GetData()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllHSNCode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    li_hsn.Add(
                        new HSNCode
                        {
                            hsnCodeID = Convert.ToInt32(dr["HSNCodeID"]),
                            hsnCode = Convert.ToString(dr["HSNCode"]),
                            taxable = Convert.ToString(dr["Category"]),
                            gstRate = Convert.ToString(dr["GSTRate"]),
                            effectiveFrom = Convert.ToString(dr["EffectiveDate"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return li_hsn;

        }

        public List<HSNCode> Bind_from_table(int id)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetHSNCode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@HSNCodeID", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    li_hsn.Add(
                        new HSNCode
                        {
                            hsnCodeID = Convert.ToInt32(dr["HSNCodeID"]),
                            hsnCode = Convert.ToString(dr["HSNCode"]),
                            description=Convert.ToString(dr["Description"]),
                            taxable = Convert.ToString(dr["Category"]),
                            gstRate = Convert.ToString(dr["GSTRate"]),
                            effectiveFrom = Convert.ToString(dr["EffectiveDate"]),
                        });
                }

            }
            catch (Exception ex)
            {

            }
            return li_hsn;
        }

        public bool SAVE(HSNCode obj)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("IUHSNCodeNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.hsnCodeID == 0)
                {
                    cmd.Parameters.AddWithValue("@Mode", "ADD");
                    cmd.Parameters.AddWithValue("@HSNCodeID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    cmd.Parameters.AddWithValue("@HSNCodeID", obj.hsnCodeID);
                }

                cmd.Parameters.AddWithValue("@HSNCode", obj.hsnCode);

                if (obj.description == "" || obj.description ==null)
                {
                    cmd.Parameters.AddWithValue("@Description", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Description", obj.description);                    
                }
                
                cmd.Parameters.AddWithValue("@Category", obj.taxable);
                cmd.Parameters.AddWithValue("@TaxBased", "Value");
              
                if (obj.gstRate == "" || obj.gstRate == null)
                {
                    cmd.Parameters.AddWithValue("@GSTRate", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GSTRate", obj.gstRate);
                }
                
                cmd.Parameters.AddWithValue("@EffectiveDate", obj.effectiveFrom);
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

        //public int DeleteHSN(int hsnCodeID)
        //{
        //    Connect();
        //    int delete = 0;
        //    MedicalVoucherType voucher = new MedicalVoucherType();
        //    SqlCommand cmd = new SqlCommand("DeleteMedicalVoucherType", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
        //    cmd.Parameters.AddWithValue("@LocationID", LocationID);
        //    cmd.Parameters.AddWithValue("@VoucherTypeID", hsnCodeID);
        //    con.Open();
        //    delete = cmd.ExecuteNonQuery();
        //    return delete;
        //}

    }
}