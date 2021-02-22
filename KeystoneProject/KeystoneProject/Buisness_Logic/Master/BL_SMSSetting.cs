using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_SMSSetting
    {
        int HospitalID;
        int LocationID;
        int UserID;

        private SqlConnection con;

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public DataSet GetSMSSetting()
        {
            Connect();
            HospitalLocationID();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetMasterSetting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();

                return ds;

            }
            catch (Exception e)
            {
                return ds;
            }


        }
        public DataSet GetMasterSmsDetails()
        {
            Connect();
            HospitalLocationID();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetMasterSmsDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();

                return ds;

            }
            catch (Exception e)
            {
                return ds;
            }


        }

        public bool Save(KeystoneProject.Models.Master.SMSSetting objSMSSetting)
        {
            try
            {
                HospitalLocationID();
                Connect();
                con.Open();
                SqlCommand cmd = new SqlCommand("IUMobileSMS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MobileSMSID", objSMSSetting.MobileSMSID);
                cmd.Parameters.AddWithValue("@IPDAddimisionDrSMS", objSMSSetting.IPDAddimisionDrSMS);
                cmd.Parameters.AddWithValue("@IPDDischargeDrSMS", objSMSSetting.IPDDischargeDrSMS);
                cmd.Parameters.AddWithValue("@IPDAddimisionPSMS", objSMSSetting.IPDAddimisionPSMS);
                cmd.Parameters.AddWithValue("@OPDRegDrSMS", objSMSSetting.OPDRegDrSMS);
                cmd.Parameters.AddWithValue("@IPDDischargePSMS", objSMSSetting.IPDDischargePSMS);
                cmd.Parameters.AddWithValue("@IPDAddimisionCDrSMS", objSMSSetting.IPDAddimisionCDrSMS);
                cmd.Parameters.AddWithValue("@IPDDischargeCDrSMS", objSMSSetting.IPDDischargeCDrSMS);
                cmd.Parameters.AddWithValue("@AppointmentPSMS", objSMSSetting.AppointmentPSMS);
                cmd.Parameters.AddWithValue("@AppointmentCDrSMS", objSMSSetting.AppointmentCDrSMS);
                cmd.Parameters.AddWithValue("@IPDAddimisionDr", "");
                cmd.Parameters.AddWithValue("@OPDBillDr", "");
                cmd.Parameters.AddWithValue("@IPDBillDr", "");
                cmd.Parameters.AddWithValue("@IPDFinalBillDr", "");
                cmd.Parameters.AddWithValue("@IPDDischargeDr", "");
                cmd.Parameters.AddWithValue("@IPDAddimisionP", "");
                cmd.Parameters.AddWithValue("@OPDRegDr", "");
                cmd.Parameters.AddWithValue("@IPDBillP", "");
                cmd.Parameters.AddWithValue("@IPDFinalBillP", "");
                cmd.Parameters.AddWithValue("@IPDDischargeP", "");
                cmd.Parameters.AddWithValue("@IPDAddimisionCDr", "");
                cmd.Parameters.AddWithValue("@IPDDischargeCDr", "");
                cmd.Parameters.AddWithValue("@AppointmentP", "");
                cmd.Parameters.AddWithValue("@AppointmentCDr", "");
                cmd.Parameters.AddWithValue("@OPDBillDrSMS", "");
                cmd.Parameters.AddWithValue("@IPDBillDrSMS", "");
                cmd.Parameters.AddWithValue("@IPDFinalBillDrSMS", "");
                cmd.Parameters.AddWithValue("@IPDBillPSMS", "");
                cmd.Parameters.AddWithValue("@IPDFinalBillPSMS", "");
            
                
                cmd.Parameters.AddWithValue("@Mode", objSMSSetting.Mode);
                int i = cmd.ExecuteNonQuery();


                
                con.Close();
            }
            catch (Exception ex)
            {
                return false;
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;
        }
    }
}