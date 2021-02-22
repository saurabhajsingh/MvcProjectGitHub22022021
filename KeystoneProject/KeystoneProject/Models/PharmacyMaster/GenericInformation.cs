using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class GenericInformation
    {
        public int HospitalID { get; set; }

        public int LocationID { get; set; }
        public int GenericID { get; set; }
        public string GenericName { get; set; }
        public string ReferenceCode { get; set; }

        public int CreationID { get; set; }
        public string mode { get; set; }
    }
}