using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class Manufacture
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public  int ManufactureID { get; set; }
        public string ManufactureName { get; set; }
        public string mode { get; set; }

    }
}