using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Report
{
    public class MISPatientWiseCollectionDetailsReport
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }

        public int PatientRegNO { get; set; }
        public string UserID { get; set; }
        public string PatientType { get; set; }
        public string BillType { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int PatientOPDIPDNo { get; set; }
        public string PatientDetails { get; set; }
        public int BillNo { get; set; }
        public int PatientAccountRowID { get; set; }
        public Decimal BillsAmount { get; set; }
        public Decimal Discount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal BalanceAmount { get; set; }

        public string Date { get; set; }

      
    }
}