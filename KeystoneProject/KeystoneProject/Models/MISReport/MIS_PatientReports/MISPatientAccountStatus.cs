using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace KeystoneProject.Models.Report
{
    public class MISPatientAccountStatus 
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string PatientType { get; set; }
        public string DisplayType { get; set; }
        public int PatientRegNo { get; set; }
        public int BillNo { get; set; }
        public string PatientName { get; set; }
        public string OPDIPDID { get; set; }
        public Decimal BillAmount { get; set; }
        public Decimal Discount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal BalanceAmount { get; set; }
        public string AddmissionDate { get; set; }
        public string OrganizationName { get; set; }
        public string Dischargedate { get; set; }
        public string TPA_CASH { get; set; }

        public Decimal TDSAmount { get; set; }
        public Decimal TPAOtherDeduction { get; set; }
        public int PatientOPDIPDNo { get; set; }
        public int PatientAccountRowID { get; set; }

        public string BillType { get; set; }
        public Decimal DrAmount { get; set; }
        public Decimal CrAmount { get; set; }
        public string PaymentType { get; set; }
        public string Particular { get; set; }
        public DateTime Date { get; set; }
        public string ServiceName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public DateTime BillDate { get; set; }

    }
}
