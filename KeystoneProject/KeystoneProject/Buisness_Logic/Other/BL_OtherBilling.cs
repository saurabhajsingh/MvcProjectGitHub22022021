using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KeystoneProject.Models.Patient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.Other
{
    public class BL_OtherBilling
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public bool Save(OPDBill obj)
        {
            int BillNo = 0;
            int PatientLabDetailID = 0;
            string ServiceType = "";
            Connect();
            con.Open();
            #region IUPatientBillsOPD _PatientBill

            SqlCommand cmdPatientBills = new SqlCommand("IUOtherBill", con);
            cmdPatientBills.CommandType = CommandType.StoredProcedure;
            cmdPatientBills.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmdPatientBills.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.BillNoId == 0)
            {
                cmdPatientBills.Parameters.AddWithValue("@BillNo", obj.BillNoId);
                cmdPatientBills.Parameters["@BillNo"].Direction = ParameterDirection.Output;
                cmdPatientBills.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmdPatientBills.Parameters.AddWithValue("@BillNo", obj.BillNoId);
                cmdPatientBills.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmdPatientBills.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(obj.BillDate));
            cmdPatientBills.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNO);
            cmdPatientBills.Parameters.AddWithValue("@OPDIPDID", obj.PatientOPDNo);
            cmdPatientBills.Parameters.AddWithValue("@BillType", "IPDOtherBill");
            cmdPatientBills.Parameters.AddWithValue("@GrossAmount", obj.grosstotal);
            cmdPatientBills.Parameters.AddWithValue("@TaxPercent", obj.TaxPercent);
            cmdPatientBills.Parameters.AddWithValue("@TaxAmount", obj.SerTaxAmount);
            cmdPatientBills.Parameters.AddWithValue("@Commisson", obj.Commisson);
            cmdPatientBills.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);
            cmdPatientBills.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
            cmdPatientBills.Parameters.AddWithValue("@DiscountPercent", obj.DiscountPercent);
            cmdPatientBills.Parameters.AddWithValue("@DiscountAmount", obj.DiscountAmount);
            cmdPatientBills.Parameters.AddWithValue("@DiscountReason", obj.DiscountReason);
            cmdPatientBills.Parameters.AddWithValue("@NetPayableAmount", obj.NetPayableAmount);
            cmdPatientBills.Parameters.AddWithValue("@BalanceAmount", obj.BalanceAmount);
            cmdPatientBills.Parameters.AddWithValue("@PreBalanceAmount", obj.PreBalance);
            //cmd.Parameters.AddWithValue("@BalanceAmount", obj.BalanceAmount);
            decimal BAmt1 = Convert.ToDecimal(obj.BalanceAmount);
            if (BAmt1 <= 0)
            {
                cmdPatientBills.Parameters.AddWithValue("@IsPaid", 0);
            }
            else
            {
                cmdPatientBills.Parameters.AddWithValue("@IsPaid", 1);

            }
            cmdPatientBills.Parameters.AddWithValue("@PaidAmount", obj.PaidAmount);
            cmdPatientBills.Parameters.AddWithValue("@DipositAmount", "0.00");
            cmdPatientBills.Parameters.AddWithValue("@IsBillMade", 0);
            cmdPatientBills.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
            cmdPatientBills.Parameters.AddWithValue("@Number", obj.Number);
            cmdPatientBills.Parameters.AddWithValue("@Name", obj.Name);
            cmdPatientBills.Parameters.AddWithValue("@Date", Convert.ToDateTime(obj.Date));
            cmdPatientBills.Parameters.AddWithValue("@Remarks", obj.Remarks);
           // cmdPatientBills.Parameters.AddWithValue("@SancationAmount", obj.SancationAmount);
            cmdPatientBills.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);
            cmdPatientBills.Parameters.AddWithValue("@CreationID", UserID);
          //  cmdPatientBills.Parameters.AddWithValue("@ForAuthorization", obj.ForAuthorization);
           
           int i = cmdPatientBills.ExecuteNonQuery();
            BillNo = Convert.ToInt32(cmdPatientBills.Parameters["@BillNo"].Value);
            #endregion
            if (i > 0)
            {
                for (int j = 0; j < obj.Services.Length; j++)
                {
                    SqlCommand cmd1 = new SqlCommand("IUOtherBillsDetails", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd1.Parameters.AddWithValue("@BillNo", BillNo);
                    cmd1.Parameters.AddWithValue("@ServiceID", obj.Services[j].SvcID);
                    cmd1.Parameters.AddWithValue("@Servicename", obj.Services[j].SvcName);
                    cmd1.Parameters.AddWithValue("@UnitID", obj.UnitID);
                    cmd1.Parameters.AddWithValue("@ChargesType", obj.ChargesType);
                    cmd1.Parameters.AddWithValue("@Rate", obj.Services[j].Rate);
                    cmd1.Parameters.AddWithValue("@Quantity", obj.Services[j].Quantity);
                    cmd1.Parameters.AddWithValue("@Discount", obj.Services[j].sevicedisAmt);
                    cmd1.Parameters.AddWithValue("@Commisson", obj.Commisson);
                    cmd1.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);
                    cmd1.Parameters.AddWithValue("@ServiceType", obj.Services[j].ServiceType);
                   // cmd1.Parameters.AddWithValue("@DoctorCharges", "");
                    cmd1.Parameters.AddWithValue("@TotalAmount", obj.Services[j].Total);
                    cmd1.Parameters.AddWithValue("@HideInBilling", "Yes");
                    cmd1.Parameters.AddWithValue("@DoctorID", obj.Services[j].DoctorID);
                    cmd1.Parameters.AddWithValue("@CreationID", UserID);
                    cmd1.Parameters.AddWithValue("@Discount_Service", obj.Services[j].Discount_Service);
                    cmd1.Parameters.AddWithValue("@DiscountServiceType", obj.Services[j].DiscountServiceType);
                    //cmd1.Parameters.AddWithValue("@ForAuthorization", obj.Services[j].Authorization);
                    cmd1.Parameters.AddWithValue("@Mode", "Add");
                    int BillsDetails = cmd1.ExecuteNonQuery();
                    if (obj.Services[j].ServiceType == "OPDLabBills")
                    {
                        ServiceType = obj.Services[j].ServiceType;
                    }


                }

            }
            return true;
        }
        #region DeleteOtherBills Bills
        public bool DeleteOtherBill(int PatientRegNo, int BillNo)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[5];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                aParams[2].Value = PatientRegNo;
                aParams[3] = new SqlParameter("@BillNo", SqlDbType.Int);
                aParams[3].Value = BillNo;
                aParams[4] = new SqlParameter("@CreationID", SqlDbType.Int);
                aParams[4].Value = UserID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteOtherBill", aParams);
            }
            catch (Exception ex)
            {
                //ex.Data.Add("returnValue", "0");
                //ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;
        }
        #endregion
        public DataSet GetPatientForOPDBills(int PatientRegNO, int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientForOtherBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetOtherBillsDetails(int BillNo, int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetOtherBillsDetails", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetOtherBills(int BillNo, int HospitalID, int LocationID)
        {

            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                con.Open();
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetOtherBills", param);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientOLDBillsNO(int PatientOPDIPDNO, int PatientRegNO, string BillType, int HospitalID, int LocationID)
        {
            Connect();
            DataSet dsPatientBillNo = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@PatientOPDIPDNO", SqlDbType.Int);
                param[0].Value = PatientOPDIPDNO;
                param[1] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[1].Value = PatientRegNO;
                param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[2].Value = HospitalID;
                param[3] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[3].Value = LocationID;
                //param[4] = new SqlParameter("@BillType", SqlDbType.NVarChar);
                //param[4].Value = BillType;
                con.Open();

                //if (ForAuthorizationGetMasterSetting() == false)
                {
                    dsPatientBillNo = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetOtherOLDBillsNO", param);
                //}
                //else
                //{
                //    dsPatientBillNo = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientOLDBillsNOForAuthorization", param);

                //    if (dsPatientBillNo.Tables[0].Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dsPatientBillNo.Tables[0].Rows.Count; i++)
                //        {
                //            if (dsPatientBillNo.Tables[0].Rows[i]["RowStatus"].ToString() == "1" && dsPatientBillNo.Tables[0].Rows[i]["ForAuthorization"].ToString() == "0")
                //            {
                //                dsPatientBillNo.Tables[0].Rows[i].Delete();
                //            }

                //        }
                //        dsPatientBillNo.AcceptChanges();
                //    }


                }
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return dsPatientBillNo;
        }
     
    }
}