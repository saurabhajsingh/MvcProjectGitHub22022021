using KeystoneProject.Models.Master;
using Microsoft.ApplicationBlocks.Data;
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
    public class BL_MasterSetting
    {
        int HospitalID;
        int LocationID;
        int UserID;

        private SqlConnection con;

        List<MasterSetting> masterSetting = new List<MasterSetting>();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public void HospitlLocationID()
        {

            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public DataSet GetAllFinancialYear()
        {
            Connect();
            HospitlLocationID();

            SqlCommand cmd = new SqlCommand("GetAllFinancialYear", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet dt = new DataSet();
            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;
        }
        public DataSet GetMasterSetting()
        {
            Connect();
            HospitlLocationID();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetMasterSetting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
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
        public bool Save(KeystoneProject.Models.Master.MasterSetting objMasterSetting)
        {
            try
            {
                HospitlLocationID();
                Connect();
                con.Open();
                SqlCommand cmd = new SqlCommand("IUMasterSetting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MasterSettingID", objMasterSetting.MasterSettingID);
                cmd.Parameters.AddWithValue("@OPDBillsOutSidePatient", false);
                cmd.Parameters.AddWithValue("@OPDLabBillsOutSidePatient", false);
                cmd.Parameters.AddWithValue("@IPDBillsBadCharges", false);
                cmd.Parameters.AddWithValue("@ForAuthorization", objMasterSetting.ForAuthorization);
                if (objMasterSetting.DoctorNameInLabReport == null || objMasterSetting.DoctorNameInLabReport == "")
                {
                    cmd.Parameters.AddWithValue("@DoctorNameInLabReport", 0);  
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DoctorNameInLabReport", objMasterSetting.DoctorNameInLabReport);
                }
                if (objMasterSetting.DoctorNameInLabReport1 == null || objMasterSetting.DoctorNameInLabReport1 == "")
                {
                    cmd.Parameters.AddWithValue("@DoctorNameInLabReport1", 0); 
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DoctorNameInLabReport1", objMasterSetting.DoctorNameInLabReport1);
                }
                
                cmd.Parameters.AddWithValue("@LogoSize", objMasterSetting.LogoSize);
                cmd.Parameters.AddWithValue("@OPDRegistration", objMasterSetting.OPDRegistration);
                cmd.Parameters.AddWithValue("@OPDPrescription", false);
                cmd.Parameters.AddWithValue("@OpdBill", objMasterSetting.OpdBill);
                cmd.Parameters.AddWithValue("@IPDAdmission", objMasterSetting.IPDAdmission);
                cmd.Parameters.AddWithValue("@IpdBill", objMasterSetting.IpdBill);
                cmd.Parameters.AddWithValue("@ProvisionalBill", objMasterSetting.ProvisionalBill);
                cmd.Parameters.AddWithValue("@IpdFinalBill", objMasterSetting.IpdFinalBill);
                cmd.Parameters.AddWithValue("@IPDDischargeSummary", objMasterSetting.IPDDischargeSummary);
                cmd.Parameters.AddWithValue("@PatientPaymentAndDeposite", objMasterSetting.PatientPaymentAndDeposite);
                cmd.Parameters.AddWithValue("@PatientPrescription", false);
                cmd.Parameters.AddWithValue("@LabBills", objMasterSetting.LabBills);
                cmd.Parameters.AddWithValue("@LabReport", objMasterSetting.LabReport);
                cmd.Parameters.AddWithValue("@BirthCertificate", false);
                cmd.Parameters.AddWithValue("@DeathCertificate", false);
                cmd.Parameters.AddWithValue("@PatientPrescreptionNew", objMasterSetting.PatientPrescreptionNew);
                cmd.Parameters.AddWithValue("@PatientPrescriptionMarathi", false);
                cmd.Parameters.AddWithValue("@ReNewDate", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@MedicalName", 0);
                cmd.Parameters.AddWithValue("@MedicalNameBill", 0);
                cmd.Parameters.AddWithValue("@Password", "Delete");
                cmd.Parameters.AddWithValue("@FinancialYearID", objMasterSetting.FinancialYearID);
                cmd.Parameters.AddWithValue("@SMSUrl", 0);
                cmd.Parameters.AddWithValue("@Authorized", objMasterSetting.Authorized);
                cmd.Parameters.AddWithValue("@LabReportFooter", objMasterSetting.LabReportFooter);
                cmd.Parameters.AddWithValue("@IPDAddimisionDrSMS", false);
                cmd.Parameters.AddWithValue("@OPDBillDrSMS", false);
                cmd.Parameters.AddWithValue("@IPDBillDrSMS", false);
                cmd.Parameters.AddWithValue("@IPDFinalBillDrSMS", false);
                cmd.Parameters.AddWithValue("@IPDDischargeDrSMS", false);
                cmd.Parameters.AddWithValue("@IPDAddimisionPSMS", false);
                cmd.Parameters.AddWithValue("@OPDRegDrSMS", false);
                cmd.Parameters.AddWithValue("@IPDBillPSMS", false);
                cmd.Parameters.AddWithValue("@IPDFinalBillPSMS", false);
                cmd.Parameters.AddWithValue("@IPDDischargePSMS", false);
                cmd.Parameters.AddWithValue("@IPDAddimisionCDrSMS", false);
                cmd.Parameters.AddWithValue("@IPDDischargeCDrSMS", false);
                cmd.Parameters.AddWithValue("@AppointmentPSMS", false);
                cmd.Parameters.AddWithValue("@AppointmentCDrSMS", false);
                cmd.Parameters.AddWithValue("@FinalBillWithHeader", objMasterSetting.FinalBillWithHeader);
                cmd.Parameters.AddWithValue("@DateTimeSetting", objMasterSetting.DateTimeSetting);
                cmd.Parameters.AddWithValue("@OtherBillsLogo", false);
                cmd.Parameters.AddWithValue("@OtherPrebalanceLogo", false);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", objMasterSetting.Mode);
                int i = cmd.ExecuteNonQuery();


                string[] Seq = objMasterSetting.Sequence.Split(',');
                string[] Prints = objMasterSetting.PrintAs.Split(',');
                string[] name = objMasterSetting.Name.Split(',');

                string[] id = objMasterSetting.PatientDischargeSettingID.Split(',');

                for (int j = 0; j < Seq.Length; j++)
                {

                    SqlCommand cmd1 = new SqlCommand("IUPatientIPDDischargeSummarySettingNew", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@PatientDischargeSettingID", id[j]);
                    cmd1.Parameters.AddWithValue("@PrintAs", Prints[j]);
                    cmd1.Parameters.AddWithValue("@Name", name[j]);
                    cmd1.Parameters.AddWithValue("@Sequence", Seq[j]);
                    int ij = cmd1.ExecuteNonQuery();

                }
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