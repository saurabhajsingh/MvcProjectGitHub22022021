using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class ChiefComplaint
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int ChiefComplaintID { get; set; }


        public string ChiefComplaintName  { get; set; }


        public string ReferenceCode { get; set; }

        public string ChiefComplaintDescription { get; set; }

        public DataSet StoreAllChiefComplaint { get; set; }
        public string Mode { get; set; } 
    }
}