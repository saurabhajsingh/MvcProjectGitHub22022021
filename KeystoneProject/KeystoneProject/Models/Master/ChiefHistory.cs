using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class ChiefHistory
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int ChiefHistoryID { get; set; }


        public string ChiefHistoryName { get; set; }


        public string ReferenceCode { get; set; }

        public string ChiefHistoryDescription { get; set; }

        public DataSet StoreAllChiefHistory { get; set; }
        public string Mode { get; set; } 
    }
}