using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterFinacialAccounts
{
    public class MISChequePrint
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        
        public string ChequeNo { get; set; }
        public string BookName { get; set; }
        public string PayeeName { get; set; }
        public string AccountName { get; set; }
        public string ChequeStatus { get; set; }
        public string TDS { get; set; }
        public string Amount { get; set; }
        public string ChequeAmount { get; set; }
        public string Narration { get; set; }
        public string ChequeBookID { get; set; }
        public DataSet dsPatientReport { get; set; }
       

        
    }
}