using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterFinacialAccounts
{
    public class Bank
    {
      
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public string BankName { get; set; }
        public string BankID { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public string AccountNumber1 { get; set; }
        public string AccountName1 { get; set; } 
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PhoneNo { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string CityID { get; set; }
        public string StateID { get; set; }
        public string CountryID { get; set; }
        public string Fax { get; set; }
        public string ReferenceCode { get; set; }
        public string BankAccountID { get; set; }

    }
}
