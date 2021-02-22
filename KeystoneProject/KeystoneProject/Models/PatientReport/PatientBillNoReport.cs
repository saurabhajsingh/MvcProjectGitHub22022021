using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PatientReport
{
    public class PatientBillNoReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string PatientName { get; set; }
        public string PatientRegNo { get; set; }
        public string BillNo { get; set; }
        public string OPDIPDNo { get; set; }
        public string BillType { get; set; }
        public string BillDate { get; set; }
        public string BillAmount { get; set; }
        public string BillAmount1 { get; set; }
        public string GrossAmount { get; set; }
        public string GrossAmount1 { get; set; }
        public string DisAmount { get; set; }
        public string DisAmount1 { get; set; }
        public string PaymentType { get; set; }
        public string TaxAmount { get; set; }
        public string TaxAmount1 { get; set; }
        public string FinancialYear { get; set; }
        public string ToBillNo { get; set; }
        public string FromBillNo { get; set; }
        public string Doctorname { get; set; }

    }
}