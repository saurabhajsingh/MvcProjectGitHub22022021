using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class GSTPercentage
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string gstTax { get; set; }
        public int gstTaxID { get; set; }
        public string gstDiscountValue { get; set; }
    }
}