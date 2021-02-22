using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeystoneProject.Models.MasterFinacialAccounts;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.MasterFinacialAccounts
{
    public class BL_VoucherType
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
        public List<VoucherType> SelectAllVoucherType()
        {
            Connect();
            List<VoucherType> vouchertype = new List<VoucherType>();
            SqlCommand cmd = new SqlCommand("GetAllVoucherType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                vouchertype.Add(
                    new VoucherType
                    {
                        VoucherTypeID = Convert.ToInt32(dr["VoucherTypeID"]),
                        VoucherTypeName = Convert.ToString(dr["VoucherTypeName"]),
                        Narration = Convert.ToString(dr["Narration"]),
                        DebitmasterAcc = Convert.ToString(dr["DebitMasterAccount"]),
                        MasterAc = Convert.ToString(dr["AccountName"]),
                        MasterAcID = Convert.ToString(dr["AccountsID"]),
                        EditmasterAcc = Convert.ToString(dr["EditMasterAccount"]),
                        CodeBlock = Convert.ToString(dr["CodeBlock"]),
                    });
            }

            return vouchertype;
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
          
            SqlCommand cmd = new SqlCommand("select  AccountsID,AccountName from Accounts where AccountName like '" + @AccountName + "%' and AccountsID like'" + AccountID + "%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);

            cmd.Parameters.AddWithValue("@AccountName", AccountName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public bool Save(VoucherType obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUVoucherType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID",HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.VoucherTypeID == 0)
            {
                cmd.Parameters.AddWithValue("@VoucherTypeID", 0);
                cmd.Parameters["@VoucherTypeID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@VoucherTypeID",obj.VoucherTypeID);
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

            cmd.Parameters.AddWithValue("@VoucherTypeName", obj.VoucherTypeName);

            if (obj.MasterAcID == null)
            {
                cmd.Parameters.AddWithValue("@AccountsID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@AccountsID", obj.MasterAcID);
            }

            cmd.Parameters.AddWithValue("@Narration", obj.Narration);
            if (obj.DebitmasterAcc == null)
            {
                 cmd.Parameters.AddWithValue("@DebitMasterAccount", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DebitMasterAccount", obj.DebitmasterAcc);
            }

            if (obj.EditmasterAcc == null)
            {
                cmd.Parameters.AddWithValue("@EditMasterAccount", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EditMasterAccount", obj.EditmasterAcc);
            }
          
           
            cmd.Parameters.AddWithValue("@CodeBlock", obj.CodeBlock);

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
        public bool DeleteVoucherType(int VoucherTypeID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@VoucherTypeID", SqlDbType.Int);
                aParams[1].Value = VoucherTypeID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteVoucherType", aParams);
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;

        }

    }
}