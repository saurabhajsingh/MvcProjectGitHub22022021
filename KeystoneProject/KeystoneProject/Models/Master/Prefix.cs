using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class Prefix
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public int PrefixMasterSettingID { get; set; }
        public string ChkUHIDNo { get; set; }
        public string ChkPatientOPD { get; set; }
        public string PatientOpd { get; set; }
        public string ChkPatientOPDBillPrefix { get; set; }
        public string PatientOPDBill { get; set; }
        public string ChkPatientIPDPrefix { get; set; }
        public string PatientIpd { get; set; }
        public string ChkPatientIPDBillPrefix { get; set; }
        public string PatientIPDBill { get; set; }
        public string ChkPatientProvisionalPrefix { get; set; }
        public string PatientProvisionalBill { get; set; }
        public string ChkPatientFinalBillPrefix { get; set; }
        public string PatientFinalBill { get; set; }
        public string PatientLab { get; set; }
        public string PatientLabBill { get; set; }
        public string PrintRegNO { get; set; }
        public string ChkPatientLabPrefix { get; set; }
        public string ChkPatientLabBillPrefix { get; set; }
        public string PatientDischargeSummaryPrefix { get; set; }
        public int StartingLabBillNo { get; set;}
        public int StartingUHIDNo { get; set; }
        public int StartingOpdNo { get; set; }
        public int StartingPatientOPDBillNo { get; set; }
        public int StartingPatientIPD { get; set; }
        public int StartingIPDBillNo { get; set; }
        public int StartingPatientProvisionalBillNo { get; set; }
        public int StartingFinalBillNo { get; set; }
        public int StartingLabNo { get; set; }
        public int FinancialYearID { get; set; }
        public string Mode { get; set; }
        public string MaxPrintRegNO { get; set; }
        public string MinPrintRegNO { get; set; }
        public string HospitalName { get; set; }
    }
}