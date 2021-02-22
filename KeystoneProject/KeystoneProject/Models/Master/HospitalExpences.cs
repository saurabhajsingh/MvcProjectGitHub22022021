using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web;
using System.Data;
namespace KeystoneProject.Models.Master
{
    public class HospitalExpences
    {

        public string[] ServiceGroupIDtbl { get; set; }
        public string[] expenceNametbl { get; set; }
        public int[] HospitalExpenceID { get; set; }
        public int[] Expencepertbl { get; set; }
        public string[] ServiceGroupNametbl { get; set; }

        public string ServiceGroupID { get; set; }
        public string expenceName { get; set; }
        public int HospitalExpencesID { get; set; }
        public string Expenceper { get; set; }
        public string ServiceGroupName { get; set; }
        public string TotalAmt { get; set; }
        public string TotalBenefitAmt { get; set; }
        public string ExpenceRs { get; set; }
        public string TotalAmount { get; set; }
    }
}