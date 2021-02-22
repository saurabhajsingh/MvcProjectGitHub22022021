using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Patient
{
    public class Deposit
    {
        public int OtherAccountRowID { get; set; }

        public string PatientRegNoPrint { get; set; }
        public string PatientRegNo { get; set; }

        public int OPDIPDID { get; set; }

        public string PatinetType { get; set; }
        public string BillType { get; set; }
        public decimal PaidAmount { get; set; }
        public string Refundsecurity { get; set; }
        public DateTime BillDate { get; set; }

        public string PaymentType { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Narrection { get; set; }
        public string Remarks { get; set; }

        public string Mode { get; set; }

        public DataSet StoreAllPreBalance { get; set; }

        public string PatientName { get; set; }


        public string PreBalance { get; set; }

        public string BalanceAmount { get; set; }

        public string OPDIPDNO { get; set; }

        public DataSet ds1 { get; set; }


 
    }
}