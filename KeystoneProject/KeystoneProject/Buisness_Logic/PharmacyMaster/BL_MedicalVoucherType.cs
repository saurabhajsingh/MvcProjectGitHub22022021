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
    public class BL_MedicalVoucherType
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<MedicalVoucherType> Med_voucher =new List<MedicalVoucherType>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<MedicalVoucherType> GetData()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetAllMedicalVoucherType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            
            SqlDataAdapter da=new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                Med_voucher.Add(
                new MedicalVoucherType
                {
                voucherTypeName = Convert.ToString(dr["VoucherTypeName"]),
                VoucherTypeID = Convert.ToInt32(dr["VoucherTypeID"]),
                masterAccount = Convert.ToString(dr["AccountName"]),
                MasterAcID = Convert.ToInt32(dr["AccountsID"]),
                AccountID = Convert.ToInt32(dr["AccountsID"]),
                narration = Convert.ToString(dr["Narration"]),
                codeblock = Convert.ToString(dr["CodeBlock"]),
                debitMasterAccount = Convert.ToString(dr["DebitMasterAccount"]),
                editMasterAccount = Convert.ToString(dr["EditMasterAccount"]),
                
            });
                
            }
            return Med_voucher;
        }

        public List<MedicalVoucherType> Bind_from_table(int id)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetMedicalVoucherType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@VoucherTypeID", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    
                    DataSet dt1 = GetAccountID("%",Convert.ToString(dr["AccountsID"]));
                    Med_voucher.Add(
                        new MedicalVoucherType
                        {
                            VoucherTypeID = Convert.ToInt32(dr["VoucherTypeID"]),
                            voucherTypeName = Convert.ToString(dr["VoucherTypeName"]),
                            masterAccount = Convert.ToString(dt1.Tables[0].Rows[0]["AccountName"]),
                            MasterAcID = Convert.ToInt32(dr["AccountsID"]),
                            narration = Convert.ToString(dr["Narration"]),
                            debitMasterAccount = Convert.ToString(dr["DebitMasterAccount"]),
                            editMasterAccount = Convert.ToString(dr["EditMasterAccount"]),
                            codeblock = Convert.ToString(dr["CodeBlock"]),
                           
                        });
                }
                
            }
            catch (Exception ex)
            {

            }
            return Med_voucher;
        }

        public bool SAVE(MedicalVoucherType obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUMedicalVoucherType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.VoucherTypeID == 0)
            {
                cmd.Parameters.AddWithValue("@VoucherTypeID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@VoucherTypeID", obj.VoucherTypeID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@VoucherTypeName", obj.voucherTypeName);
            cmd.Parameters.AddWithValue("@ReferenceCode", 0);
            cmd.Parameters.AddWithValue("@AccountsID", obj.MasterAcID);
            cmd.Parameters.AddWithValue("@Narration", obj.narration);
            if (obj.debitMasterAccount == null)
            {
                cmd.Parameters.AddWithValue("@DebitMasterAccount", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DebitMasterAccount", obj.debitMasterAccount);
            }

            if (obj.editMasterAccount == null)
            {
                cmd.Parameters.AddWithValue("@EditMasterAccount", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EditMasterAccount", obj.editMasterAccount);
            }
            cmd.Parameters.AddWithValue("@CodeBlock", obj.codeblock);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            
            con.Open();
           int i= cmd.ExecuteNonQuery();
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

        public DataSet GetAccountID(string AccountName, string AccountID)
        {
            Connect();

            if (AccountID == null)
            {
                AccountID = "%";
            }
            if (AccountID == "")
            {
                AccountID = "%";
            }

            SqlCommand cmd = new SqlCommand("select  AccountsID,AccountName from MedicalAccounts where AccountName like '" + @AccountName + "%' and AccountsID like'" + AccountID + "%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);

            cmd.Parameters.AddWithValue("@AccountName", AccountName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public int DeleteVoucher(int VoucherTypeID)
        {
            Connect();
            int delete = 0;
            MedicalVoucherType voucher = new MedicalVoucherType();
            SqlCommand cmd = new SqlCommand("DeleteMedicalVoucherType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@VoucherTypeID", VoucherTypeID);
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
        }
        public bool CheckVoucher(int VoucherTypeID, string voucherTypeName)
        {
            int t = 0;
            if (VoucherTypeID == 0)
            {
                t = 0;
            }
            else
            {
                t = VoucherTypeID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckMedicalVoucherType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@VoucherTypeID", t);
                cmd.Parameters.AddWithValue("@VoucherTypeName", voucherTypeName.ToUpper());
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