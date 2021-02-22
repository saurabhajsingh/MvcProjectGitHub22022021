using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterFinacialAccounts
{
    public class ChequeBook
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public string ChequeBookID { get; set; }
        public string ChequeBookName { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string BankAccountID { get; set; }
        public string AccountName { get; set; }
        public string ChequeLayoutID { get; set; }
        public string LayoutName { get; set; }
        public string ReferenceCode { get; set; }
        public string ChequeNoFrom { get; set; }
        public string ChequeNoTo { get; set; }
      

    }
}