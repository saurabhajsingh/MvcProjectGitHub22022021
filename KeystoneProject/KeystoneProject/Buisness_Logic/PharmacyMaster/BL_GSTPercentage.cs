using KeystoneProject.Models.PharmacyMaster;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.PharmacyMaster
{
    public class BL_GSTPercentage
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<GSTPercentage> li_gstper = new List<GSTPercentage>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<GSTPercentage> GetData()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllGSTPercentage", con);
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
                    li_gstper.Add(
                        new GSTPercentage
                        {
                            gstTaxID = Convert.ToInt32(dr["GSTID"]),
                            gstTax = Convert.ToString(dr["GSTTAX"]),
                            gstDiscountValue = Convert.ToString(dr["GSTDiscValue"]),
                        });
                }
            }
            catch(Exception ex)
            {

            }
            return li_gstper;

        }

        public bool SAVE(GSTPercentage obj)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("IUGSTPercentage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.gstTaxID  == 0)
                {
                    cmd.Parameters.AddWithValue("@Mode", "ADD");
                    cmd.Parameters.AddWithValue("@GSTID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    cmd.Parameters.AddWithValue("@GSTID", obj.gstTaxID);
                }

                cmd.Parameters.AddWithValue("@GSTTAX", obj.gstTax);

                if (obj.gstDiscountValue == null || obj.gstDiscountValue == "")
                {
                    cmd.Parameters.AddWithValue("@GSTDiscValue", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GSTDiscValue", obj.gstDiscountValue);
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
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<GSTPercentage> Bind_From_Table(int gstTaxID)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetGSTPercentage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GSTID", gstTaxID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    li_gstper.Add(
                        new GSTPercentage
                        {
                            gstTax = Convert.ToString(dr["GSTTAX"]),
                            gstDiscountValue = Convert.ToString(dr["GSTDiscValue"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return li_gstper;

        }

        public int Delete_Data(int gstTaxID)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteGSTPercentage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GSTID", gstTaxID);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public bool Check_Tax(string gstTax, int gstTaxID)
        {
            int t = 0;
            if (gstTaxID == 0)
            {
                t = 0;
            }
            else
            {
                t = gstTaxID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckGSTPercentage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GSTID", t);
                cmd.Parameters.AddWithValue("@GSTTAX", gstTax.ToUpper());
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
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}