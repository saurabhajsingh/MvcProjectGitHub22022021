using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.FinancialAccount;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Buisness_Logic.FinancialAccount
{
    public class BL_VoucherEntry
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        public SqlConnection con;
        public void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

       public List<VoucherEntry> SelectAllVoucherEntry()
        {
            Connect();
            List<VoucherEntry> voucherentry = new List<VoucherEntry>();
            SqlCommand cmd = new SqlCommand("GetAllVoucharEntry", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@VoucharType", "%");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt); con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                voucherentry.Add(
                    new VoucherEntry
                    {
                        VoucharEntryID = Convert.ToString(dr["VoucharEntryID"]),
                        
                        CurrentDate = Convert.ToDateTime(dr["CurrentDate"]).ToString("dd-MM-yyyy"),
                        VoucharName = Convert.ToString(dr["VoucharName"]),
                        RefVoucharNo = Convert.ToString(dr["RefVoucharNo"]),
                        AccountName = Convert.ToString(dr["AccountName"]),
                        VoucharDate = Convert.ToDateTime(dr["VoucharDate"]).ToString("dd-MM-yyyy"),
                        Narration = Convert.ToString(dr["Narration"]),
                        VoucharAccountName = Convert.ToString(dr["VoucharAccountName"]),
                        InvNo = Convert.ToString(dr["InvNo"]),
                        //ChequeClearDate = Convert.ToDateTime(dr["ChequeClearDate"]).ToString("yyyy-MM-dd"),
                        VoucharDrAmount = Convert.ToString(dr["VoucharDrAmount"]),
                        VoucharCrAmount = Convert.ToString(dr["VoucharCrAmount"]),
                        AccountsIDTable = Convert.ToString(dr["AccountsID"]),
                        VoucharID = Convert.ToString(dr["VoucharID"]),
                        AccountsID = Convert.ToString(dr["AccountsID"]),
                       
                    });
                
            }

            return voucherentry;
        }
       public DataSet RptVoucherEntry(int VoucharEntryID)
       {
            Connect();
           SqlCommand cmd = new SqlCommand("RptVoucherEntry", con);
           cmd.CommandType = CommandType.StoredProcedure;
           
           cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
           cmd.Parameters.AddWithValue("@LocationID", LocationID);
           cmd.Parameters.AddWithValue("@VoucharEntryID", VoucharEntryID);
          DataTable dt = new DataTable();
           DataSet ds = new DataSet();
           con.Open();
           SqlDataAdapter ad = new SqlDataAdapter();
           ad.SelectCommand = cmd;
           ad.Fill(ds);
           con.Close();

         
               if (ds.Tables[1].Rows.Count != 0)
               {
                   if (Convert.ToInt32(ds.Tables[0].Rows[0]["MsterAccountsID"]) > 0)
                   {
                       if (Convert.ToBoolean(ds.Tables[0].Rows[0]["DebitMasterAccount"]) == false)
                       {
                           ds.Tables[0].Rows[0]["CrOrDr"] = "Credit";
                           ds.Tables[0].Rows[0]["Cr"] = "Dr";
                           ds.Tables[0].Rows[0]["MasterDr"] = "Cr";
                           ds.Tables[0].Rows[0]["VoucharDrAmount"] = ds.Tables[0].Rows[0]["VoucharCrAmount"].ToString();
                           foreach (DataRow dr in ds.Tables[1].Rows)
                           {
                               dr["CrAmount"] = dr["DrAmount"].ToString();
                           }
                       }
                       HttpContext.Current.Session["RptVoucherEntry"] = ds;
                      
                   }
              

               }
               return ds;
           }
  

       public DataSet BindVoucherName(string GetVoucherName)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Select VoucherTypeID,VoucherTypeName,AccountsID,Narration ,DebitMasterAccount ,EditMasterAccount from VoucherType where VoucherTypeName  like ''+@GetVoucherName+'%' and RowStatus = 0  and VoucherType.HospitalID =  " + HospitalID + "  and  VoucherType.LocationID = " + LocationID + " order by VoucherType.VoucherTypeName asc", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GetVoucherName", GetVoucherName);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetVoucherType(string VoucherTypeID)
            {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetVoucherType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@VoucherTypeID", VoucherTypeID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet BindAccountName(string GetAccountName)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select AccountsID,  AccountName ,Schedule.ScheduleName,  OPBalance, DrAmount ,CrLimit,PrintName, CreditDays,Address,PhoneNo,MobileNo,EmailID,TinNo,Pan,TinDate   from Accounts left join Schedule on Schedule.ScheduleID = Accounts.ScheduleID where Accounts.RowStatus = 0 and Schedule.RowStatus = 0 and  AccountName like ''+@GetAccountName+'%' and Schedule.HospitalID = " + HospitalID + " and Schedule.LocationID = " + LocationID + " and  Accounts.HospitalID = " + HospitalID + " and Accounts.LocationID = " + LocationID + " order by Accounts.AccountName asc", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@GetAccountName", GetAccountName);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public bool Save(VoucherEntry obj)
        {
            Connect();
            try
            {
                string[] AccountName = obj.AccountName.Split(',');
                AccountName = AccountName.Where(name => !string.IsNullOrEmpty(name)).ToArray();
                string[] account_name_otherId = obj.AccountsIDTable.Split(',');
                string[] DrAmount = obj.DrAmount.Split(',');
                DrAmount = DrAmount.Where(name => !string.IsNullOrEmpty(name)).ToArray();
                string[] CrAmount = obj.CrAmount.Split(',');
                CrAmount = CrAmount.Where(name => !string.IsNullOrEmpty(name)).ToArray();
                string[] ChequeNo = obj.ChequeNo.Split(',');              
                string[] BankName = obj.BankName.Split(',');
                string[] Date = obj.Date.Split(',');                
                //string[] ChequeClearDate = obj.ChequeClearDate.Split(',');
                string[] InvNo = obj.InvNo.Split(',');

                SqlCommand cmd = new SqlCommand("IUVoucharEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                if (obj.ReferenceCode == null)
                {
                     cmd.Parameters.AddWithValue("@ReferenceCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                }

                if (obj.VoucharEntryID == "0" || obj.VoucharEntryID == null)
                {
                    cmd.Parameters.AddWithValue("@VoucharEntryID", 0);
                    cmd.Parameters["@VoucharEntryID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@VoucharEntryID", obj.VoucharEntryID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");

                }                
                //cmd.Parameters.AddWithValue("@VoucharEntryID", 0);
                //cmd.Parameters["@VoucharEntryID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@AccountsID", obj.AccountsID);
                cmd.Parameters.AddWithValue("@VoucharID", obj.VoucharID);
                cmd.Parameters.AddWithValue("@VoucharName", obj.VoucharName);
                cmd.Parameters.AddWithValue("@RefVoucharNo", obj.RefVoucharNo);
                cmd.Parameters.AddWithValue("@CurrentDate", obj.CurrentDate);
                cmd.Parameters.AddWithValue("@VoucharDate", obj.VoucharDate);
                cmd.Parameters.AddWithValue("@Narration", obj.Narration);
                cmd.Parameters.AddWithValue("@VoucharAccountName", obj.VoucharAccountName);
                cmd.Parameters.AddWithValue("@VoucharDrAmount", obj.VoucharDrAmount);
                cmd.Parameters.AddWithValue("@VoucharCrAmount", obj.VoucharCrAmount);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                //cmd.Parameters.AddWithValue("@Mode", "Add");
                con.Open();

                int i = cmd.ExecuteNonQuery();
                obj.VoucharEntryID = Convert.ToString(cmd.Parameters["@VoucharEntryID"].Value);
                con.Close();
                if (i > 0)
                {
                    for (int k = 0; k < AccountName.Length; k++)
                    {
                        SqlCommand cmd1 = new SqlCommand("IUVoucharEntryDetail", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                        if (obj.VoucharEntryDetailID == null || obj.VoucharEntryDetailID == "")
                        {
                            cmd1.Parameters.AddWithValue("@VoucharEntryDetailID", 0);

                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@VoucharEntryDetailID", obj.VoucharEntryDetailID);
                            cmd1.Parameters.AddWithValue("@Mode", "Edit");
                        }
                        //cmd1.Parameters.AddWithValue("@VoucharEntryDetailID", 0);

                        cmd1.Parameters.AddWithValue("@VoucharEntryID", obj.VoucharEntryID);


                        cmd1.Parameters.AddWithValue("@VoucharID", obj.VoucharID);
                        cmd1.Parameters.AddWithValue("@VoucharName", obj.VoucharName);
                        cmd1.Parameters.AddWithValue("@AccountsID", account_name_otherId[k]);
                        cmd1.Parameters.AddWithValue("@AccountName", AccountName[k]);
                        cmd1.Parameters.AddWithValue("@DrAmount", DrAmount[k]);
                        cmd1.Parameters.AddWithValue("@CrAmount", CrAmount[k]);
                        //cmd1.Parameters.AddWithValue("@ChequeClearDate",ChequeClearDate[k]);
                        cmd1.Parameters.AddWithValue("@InvNo",InvNo[k]);
                        cmd1.Parameters.AddWithValue("@ChequeNo", ChequeNo[k]);
                        cmd1.Parameters.AddWithValue("@Name", BankName[k]);
                        cmd1.Parameters.AddWithValue("@Date", Date[k]);
                        cmd1.Parameters.AddWithValue("@Narration", obj.Narration);
                        cmd1.Parameters.AddWithValue("@CreationID", UserID);
                        //cmd1.Parameters.AddWithValue("@Mode", "Add");
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
        public bool DeleteVoucherEntry(int VoucharEntryID)
        {

            Connect();
            SqlParameter[] apram = new SqlParameter[4];
            apram[0] = new SqlParameter("@VoucharEntryID", SqlDbType.Int);
            apram[0].Value = VoucharEntryID;
            apram[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
            apram[1].Value = HospitalID;
            apram[2] = new SqlParameter("@LocationID", SqlDbType.Int);
            apram[2].Value = LocationID;
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeleteVoucharEntry", apram);
            con.Close();

            return true;
        }

        public DataSet GetAllBank()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet GetChequeBookByBankID(string BankId)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetChequeBookByBankID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BankID", BankId);
                //cmd.Parameters.AddWithValue("",);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetChequeLayoutForCheckPrint(string ChequeBookID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetChequeLayoutForCheckPrint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ChequeBookID", ChequeBookID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            { }
            return ds;
        }

       public DataSet GetVoucharEntryDetail(string VoucharEntryID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetVoucharEntryDetail",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID",HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@VoucharEntryID", VoucharEntryID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Close();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                          
            }
            return ds;
        }

       public DataSet GetVoucharEntry(string VoucharEntryID)
       {
           Connect();
           DataSet ds = new DataSet();
           try
           {
               SqlCommand cmd = new SqlCommand("GetVoucharEntry", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
               cmd.Parameters.AddWithValue("@LocationID", LocationID);
               cmd.Parameters.AddWithValue("@VoucharEntryID", VoucharEntryID);
               con.Open();
               SqlDataAdapter ad = new SqlDataAdapter(cmd);
               con.Close();
               ad.Fill(ds);
           }

           catch (Exception ex)
           { }
           return ds;
       }

    }
}