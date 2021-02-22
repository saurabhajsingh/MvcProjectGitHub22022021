using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Financial
{
    public class UserPaymentAccount
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public string AccountsID { get; set; }
        public string AccountName { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string CurrentDate { get; set; }
        public string Narration { get; set; }
        public string PaidAmount { get; set; }
        public string BalanceAmount { get; set; }
        public string DrAmount { get; set; }
        public string CrAmount { get; set; }
        public String Balance { get; set; }
        public string UserBalance { get; set; }
        public string ReferenceCode { get; set; }
        public string UserPaymentAccountID { get; set; }
       

       
    }
}