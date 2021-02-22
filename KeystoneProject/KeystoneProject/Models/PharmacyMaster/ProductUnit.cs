using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class ProductUnit
    {
        public int HospitalID { get; set; }

        public int LocationID { get; set; }
        public int ProductUnitID { get; set; }
        public string ProductUnitName { get; set; }
        public string contains { get; set; }
        public string lowerUnitName { get; set; }
        public string sellLoose { get; set; }
        public int CreationID { get; set; }
        public decimal NumberOfDecimal { get; set; }
        public string RefferenceCode { get; set; }
    }


    
    }


