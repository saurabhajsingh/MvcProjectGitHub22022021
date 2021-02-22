using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.FinancialAccount
{
    public class AccountStatement
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public DateTime FromDate { get;set;}
        public DateTime ToDate { get; set; }
        public string AccountsID { get; set; }
        public string AccountName { get; set; }
        public Decimal DrBalance { get; set; }
        public Decimal CrBalance { get; set; }
        public DataSet dsAccount { get; set; }
        public Decimal DrAmount { get; set; }
        public Decimal CrAmount { get; set; }
        public string FromEntryTypeID { get; set; }
        public string FromEntryType { get; set; }
        public string TransactionType { get; set; }
        public string TransactionID { get; set; }
        public string Particular { get; set; }
        public string Narration { get; set; }
     




    }
}