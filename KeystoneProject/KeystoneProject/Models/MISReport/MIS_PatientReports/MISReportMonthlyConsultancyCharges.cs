using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.PatientReport
{
    public class MISReportMonthlyConsultancyCharges
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string Regno { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public string DoctorPrintName { get; set; }
        public string BillDate { get; set; }

        public string BillAmount { get; set; }
        public string ConsAmt { get; set; }
        public string Discount { get; set; }
        public string PaidAmount { get; set; }

        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string PaidAmt { get; set; }
        public string BillAmt { get; set; }
    }
}