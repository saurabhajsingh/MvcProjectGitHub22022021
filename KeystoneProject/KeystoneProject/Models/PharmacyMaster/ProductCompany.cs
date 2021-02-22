using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class ProductCompany
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string margin { get; set; }
        public string salesTax { get; set; }
        public string purchaseTax { get; set; }
        public string lteProducts { get; set; }
        public string ltiProducts { get; set; }
        public string Exclusive { get; set; }
        public string Inclusive { get; set; }
        public string radio { get; set; }
        public string exclusiveTax { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string pinCode { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string emailid { get; set; }
        public string phoneNumber { get; set; }
        public string mobileNumber { get; set; }
        public int nameID { get; set; }
        public string salesTaxID { get; set; }
        public string purchaseTaxID { get; set; }
        public string cityID { get; set; }
        public string stateID { get; set; }
        public string countryID { get; set; }
    }
}