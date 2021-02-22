using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class ProductCategory
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string categoryName { get; set; }
        public int categoryID { get; set; }
        public string salesTax { get; set; }
        public string purchaseTax { get; set; }
        public string colorVal { get; set; }
        public string colorStatus { get; set; }
        public string saleTaxID { get; set; }
        public string purchaseTaxID { get; set; }


    }
}