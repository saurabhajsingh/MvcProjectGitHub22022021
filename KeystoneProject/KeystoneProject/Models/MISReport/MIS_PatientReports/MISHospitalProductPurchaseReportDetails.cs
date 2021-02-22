using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.MIS_PatientReports
{
    public class MISHospitalProductPurchaseReportDetails
    {
        public string HospitalID { get; set; }
        public string LocationID { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string BillNo { get; set; }
        public string InvoiceNo { get; set; }
        public string BillDate { get; set; }
        public string PaymentType { get; set; }
        public string NetAmount { get; set; }
        public string PaidAmount { get; set; }
        public string Rate { get; set; }
        public string Quantity { get; set; }
        public string TotalAmount { get; set; }
        public string ItemName { get; set; }
        public string TotalPaid { get; set; }
        public string TotalNet { get; set; }

        //public string HospitalID { get; set; }
        //public string HospitalID { get; set; }
    }
}