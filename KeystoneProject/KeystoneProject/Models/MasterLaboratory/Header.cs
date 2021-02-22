using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterLaboratory
{
    public class Header
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string HeaderID { get; set; }
        public string HeaderName { get; set; }
        public object ReferenceCode { get; set; }

    }
}