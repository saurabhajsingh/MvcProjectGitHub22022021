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
    public class BL_MedicalAccount
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationId = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<MedicalAccount> Med_Account = new List<MedicalAccount>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<MedicalAccount> Getdata()
        {
            try
            { 
            Connect();
            SqlCommand cmd = new SqlCommand("GetAllMedicalAccounts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {

                Med_Account.Add(
                    new MedicalAccount
                    {
                        AccountName = Convert.ToString(dr["AccountName"]),
                        AccountID = Convert.ToInt32(dr["AccountsID"]),
                        ScheduleName = Convert.ToString(dr["ScheduleName"]),
                        DrAmount = Convert.ToInt32(dr["DrAmount"]),
                        CrAmount = Convert.ToInt32(dr["CrAmount"]),
                        CrLimit = Convert.ToString(dr["CrLimit"]),
                        PrintName = Convert.ToString(dr["PrintName"]),
                        Locality = Convert.ToString(dr["Locality"]),
                        CreditDays = Convert.ToString(dr["CreditDays"]),
                        Address = Convert.ToString(dr["Address"]),
                        PinCode = Convert.ToString(dr["PinCode"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        EmailID = Convert.ToString(dr["EmailID"]),
                        Remark = Convert.ToString(dr["Remark"]),
                        FAX = Convert.ToString(dr["FAX"]),
                        TinNo = Convert.ToString(dr["TinNo"]),
                        Pan = Convert.ToString(dr["Pan"]),
                        TinDate = Convert.ToString(dr["TinDate"]),
                        CSTNO = Convert.ToString(dr["CSTNO"]),
                        CityID = Convert.ToString(dr["CityID"]),
                        StateID = Convert.ToString(dr["StateID"]),
                        CountryID = Convert.ToString(dr["CountryID"]),
                        city = Convert.ToString(dr["CityName"]),
                        state = Convert.ToString(dr["StateName"]),
                        country = Convert.ToString(dr["CountryName"]),

                    });
            }
            }
            catch(Exception ex)
            {
                
            }
            return Med_Account;
        }

        public bool Save(MedicalAccount obj)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("IUMedicalAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);

                if (obj.AccountID == 0)
                {
                    cmd.Parameters.AddWithValue("@AccountsID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AccountsID", obj.AccountID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                cmd.Parameters.AddWithValue("@AccountName", obj.AccountName);
                cmd.Parameters.AddWithValue("@ReferenceCode", 0);
                cmd.Parameters.AddWithValue("@ScheduleID", obj.ScheduleID);
                cmd.Parameters.AddWithValue("@OPBalance", obj.OBType);
                if (obj.OBType == "Credit")
                {
                    cmd.Parameters.AddWithValue("@CrAmount", obj.OPBalance);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CrAmount", 0.00);
                }


                if (obj.OBType == "Debit")
                {
                    cmd.Parameters.AddWithValue("@DrAmount", obj.OPBalance);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DrAmount", 0.00);

                }
                cmd.Parameters.AddWithValue("@CrLimit", obj.CrLimit);
                cmd.Parameters.AddWithValue("@PrintName", obj.PrintName);
                cmd.Parameters.AddWithValue("@CreditDays", obj.CreditDays);
                cmd.Parameters.AddWithValue("@AccountType", obj.AccountType);
                cmd.Parameters.AddWithValue("@Address", obj.Address);
                cmd.Parameters.AddWithValue("@CityID", obj.CityID);
                cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
                cmd.Parameters.AddWithValue("@StateID", obj.StateID);
                cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
                cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNo);
                cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                cmd.Parameters.AddWithValue("@Remark", obj.Remark);
                cmd.Parameters.AddWithValue("@FAX", obj.FAX);
                cmd.Parameters.AddWithValue("@TinNo", obj.TinNo);
                cmd.Parameters.AddWithValue("@Pan", obj.Pan);
                cmd.Parameters.AddWithValue("@TinDate", obj.TinDate);
                cmd.Parameters.AddWithValue("@CSTNO", obj.CSTNO);
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
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        private bool RedirectToAction(string v, object p)
        {
            throw new NotImplementedException();
        }
        public DataSet GetCity(string CityName, string City)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
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
            cmd.Parameters.AddWithValue("@LocationID", LocationId);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataSet BindSchedule(string GetScheduleName)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ScheduleID , ScheduleName  FROM MedicalSchedule where  ScheduleName like ''+@GetScheduleName+'%' and  HospitalID = '" + HospitalID + "' and LocationID = '" + LocationId + "' and RowStatus = 0 order by  ScheduleName  asc", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                cmd.Parameters.AddWithValue("@GetScheduleName", GetScheduleName);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public List<MedicalAccount> Bind_from_table(int id)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetMedicalAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                cmd.Parameters.AddWithValue("@AccountsID", id);
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    DataSet dssc= BindSchedule(Convert.ToString("%"));
                    DataView dataView3 = new DataView(dssc.Tables[0], " ScheduleID = '" + Convert.ToInt32(dr["ScheduleID"]) + "'", "", DataViewRowState.CurrentRows);
                    DataSet ds= GetCity("%", Convert.ToString(dr["CityID"]));
                    DataTable dt1 = GetCountryStateID(Convert.ToInt32(dr["CityID"]));
                    Med_Account.Add(
                        new MedicalAccount
                        {
                            AccountName = Convert.ToString(dr["AccountName"]),
                            AccountID = Convert.ToInt32(dr["AccountsID"]),
                            ScheduleName = Convert.ToString(dataView3.ToTable().Rows[0]["ScheduleName"]),
                            ScheduleID = Convert.ToString(dr["ScheduleID"]),
                            OPBalance = Convert.ToString(dr["OPBalance"]),
                            DrAmount = Convert.ToInt32(dr["DrAmount"]),
                            CrAmount = Convert.ToInt32(dr["CrAmount"]),
                            CrLimit = Convert.ToString(dr["CrLimit"]),
                            AccountType = Convert.ToString(dr["AccountType"]),
                            PrintName = Convert.ToString(dr["PrintName"]),

                            CreditDays = Convert.ToString(dr["CreditDays"]),
                            Address = Convert.ToString(dr["Address"]),
                            city = Convert.ToString(ds.Tables[0].Rows[0]["CityName"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            state = Convert.ToString(dt1.Rows[0]["StateName"]),
                            country = Convert.ToString(dt1.Rows[0]["CountryName"]),
                            PhoneNo = Convert.ToString(dr["PhoneNo"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            EmailID = Convert.ToString(dr["EmailID"]),
                            Remark = Convert.ToString(dr["Remark"]),
                            FAX = Convert.ToString(dr["FAX"]),
                            TinNo = Convert.ToString(dr["TinNo"]),
                            Pan = Convert.ToString(dr["Pan"]),

                            TinDate = Convert.ToDateTime(dr["TinDate"]).ToString("yyyy-MM-dd"),
                            CSTNO = Convert.ToString(dr["CSTNO"]),
                            
                            CityID = Convert.ToString(ds.Tables[0].Rows[0]["CityID"]),
                            StateID = Convert.ToString(dt1.Rows[0]["StateID"]),
                            CountryID = Convert.ToString(dt1.Rows[0]["CountryID"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return Med_Account;
        }

        public int DeleteAccounts(int AccountID)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeleteMedicalAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);
                cmd.Parameters.AddWithValue("@AccountsID", AccountID);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();

            }
            return i;
        }

        public bool CheckAccounts(int AccountID, string AccountName)
        {
            int t = 0;
            if (AccountID == 0)
            {
                t = 0;
            }
            else
            {
                t = AccountID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckMedicalAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationId);

                cmd.Parameters.AddWithValue("@AccountsID", t);
                cmd.Parameters.AddWithValue("@AccountName", AccountName.ToUpper());
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