using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Report
{
    public class MISDiscountReason 
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string SrNo { get; set; }

        public string PatientType { get; set; }
        public string RegNo { get; set; }
        public string PatientName { get; set; }
        public string BillType { get; set; }
        public string Discount { get; set; }
        public string DiscountAmount { get; set; }
        public string DiscountReason { get; set; }
        public string TaxAmount { get; set; }
        public string OPDIPDNo { get; set; }
        public string BillNo { get; set; }
        public string BillAmount { get; set; }
        public string BillAmount1 { get; set; }
        public string PaidAmount { get; set; }
        public string PaidAmount1 { get; set; }
        public string BalanceAmount { get; set; }
        public string BalanceAmount1 { get; set; }

        public DataSet dsPatientReport { get; set; }
    }
}

