using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.MIS_PatientReports
{
    public class ReportMISTPAPatientAccountStatus
    {
        public string HospitalID { get; set;}
        public string LocationID { get; set; }
        public string PatientName { get; set; }
        public string PatientReg { get; set; }
        public string PatientType { get; set; }
        public string OPDIPDID { get; set; }
        public string OrganizationName { get; set; }
        public string AddmissionDate { get; set; }
        public string DischargeDate { get; set; }
        public string TPA { get; set; }
        public string BillAmount { get; set; }
        public string TDSAmt { get; set; }
        public string TPAOtherDeduction { get; set; }
        public string DiscountAmt { get; set; }
        public string PaidAmt { get; set; }
        public string BalanceAmt { get; set; }
        public string PaidAmt1 { get; set; }
        public string BillAmt1 { get; set; }
        public string DisAmt1 { get; set; }
        public string BalAmt1 { get; set; }
        public string BillType {get;set;}
        public string Particular { get; set; }
    }
}