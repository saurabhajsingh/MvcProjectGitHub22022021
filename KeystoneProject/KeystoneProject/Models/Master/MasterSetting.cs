using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class MasterSetting
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public bool ForAuthorization { get; set; }
        
        public int MasterSettingID { get; set; }

        public string FinancialYears { get; set; }

        public bool OPDBillsOutSidePatient { get; set; }

        public bool OPDLabBillsOutSidePatient { get; set; }

        public bool IPDBillsBadCharges { get; set; }


        public string DoctorNameInLabReport { get; set; }
        public string DoctorNameInLabReport1 { get; set; }

        public string LogoSize { get; set; }

        public bool OPDRegistration { get; set; }

        public bool OPDPrescription { get; set; }

        public bool OpdBill { get; set; }

        public bool IPDAdmission { get; set; }

        public bool IpdBill { get; set; }

        public bool ProvisionalBill { get; set; }

        public bool IpdFinalBill { get; set; }

        public bool IPDDischargeSummary { get; set; }

        public bool PatientPaymentAndDeposite { get; set; }

        public bool PatientPrescription { get; set; }

        public bool LabBills { get; set; }

        public bool LabReport { get; set; }

        public bool BirthCertificate { get; set; }

        public bool DeathCertificate { get; set; }

        public bool PatientPrescreptionNew { get; set; }

        public bool PatientPrescriptionMarathi { get; set; }

        public DateTime ReNewDate { get; set; }

        public string MedicalName { get; set; }

        public string MedicalNameBill { get; set; }
        public string Mode { get; set; }
        public string Password { get; set; }

        public int FinancialYearID { get; set; }

        public string SMSurl { get; set; }

        public bool Authorized { get; set; }

        public string LabReportFooter { get; set; }

        public bool IPDAddimisionDrSMS { get; set; }

        public bool OPDBillDrSMS { get; set; }

        public bool IPDBillDrSMS { get; set; }

        public bool IPDFinalBillDrSMS { get; set; }

        public bool IPDDischargeDrSMS { get; set; }

        public bool IPDAddimisionPSMS { get; set; }

        public bool OPDRegDrSMS { get; set; }

        public bool IPDBillPSMS { get; set; }

        public bool IPDFinalBillPSMS { get; set; }

        public bool IPDDischargePSMS { get; set; }

        public bool IPDAddimisionCDrSMS { get; set; }

        public bool IPDDischargeCDrSMS { get; set; }

        public bool AppointmentPSMS { get; set; }

        public bool AppointmentCDrSMS { get; set; }

        public bool FinalBillWithHeader { get; set; }

        public bool DateTimeSetting { get; set; }

        public bool OtherBillsLogo { get; set; }

        public bool OtherPrebalanceLogo { get; set; }

        public int CreationID { get; set; }

        public Nullable<DateTime> CreationDate { get; set; }
        public int LastModificationID { get; set; }
        public Nullable<DateTime> LastModificationDate { get; set; }


        public DataSet ShowAllMasterSetting { get; set; }
        public DataSet dsShowAllMasterSetting { get; set; }

        public DataSet dsFinancial { get; set; }


        //////////////////////Patient IPD DischargeSummary Setting New ////////


        public string PatientDischargeSettingID { get; set; }

        public string Name { get; set; }

        public string PrintAs { get; set; }

        public string Sequence { get; set; }



    }
}