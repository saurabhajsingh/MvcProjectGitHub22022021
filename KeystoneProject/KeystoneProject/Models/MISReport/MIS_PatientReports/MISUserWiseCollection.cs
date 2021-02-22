using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Report
{
    public class MISUserWiseCollection
    {

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string TotalCollection { get; set; }

        public string UserID { get; set; }

        public string FullName { get; set; }

        public string PaymentType { get; set; }

        public DataSet dsuser { get; set; }
        public DataSet dscollection { get; set; }

        public int BillNo { get; set; }

        public string PatientOPDIPDNo { get; set; }
        public string Type { get; set; }

        public string Other { get; set; }
        public string PatientName { get; set; }

        public string Particular { get; set; }

        public Decimal CrAmount { get; set; }

        public Decimal DrAmount { get; set; }

        public string Date { get; set; }

        public string TDSAmount { get; set; }

        public string TPAOtherDeduction { get; set; }
        public string Payment { get; set; }
        public int RegNO { get; set; }
        public string DrAmtTotal { get; set; }
        public string CrAmtTotal { get; set; }

        public string TotalAmt { get; set; }
        
    }
}