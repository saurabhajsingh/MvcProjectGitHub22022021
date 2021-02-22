using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class TreatmentAdvice
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int TreatmentAdviceID { get; set; }


        public string TreatmentAdviceName { get; set; }


        public string ReferenceCode { get; set; }

        public string TreatmentAdviceDescription { get; set; }

        public DataSet StoreAllTreatmentAdvice { get; set; }
        public string Mode { get; set; } 
    }
}