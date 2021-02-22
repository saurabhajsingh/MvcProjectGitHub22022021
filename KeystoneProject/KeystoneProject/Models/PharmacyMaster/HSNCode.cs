using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class HSNCode
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string hsnCode { get; set; }
        public string description { get; set; }
        public string taxable { get; set; }
        public string effectiveFrom { get; set; }
        public string gstRate { get; set; }
        public int hsnCodeID { get; set; }

    }
}