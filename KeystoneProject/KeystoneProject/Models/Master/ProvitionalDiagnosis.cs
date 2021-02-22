using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class ProvitionalDiagnosis
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int ProvitionalDiagnosisID { get; set; }


        public string ProvitionalDiagnosisName { get; set; }


        public string ReferenceCode { get; set; }

        public string ProvitionalDiagnosisDescription { get; set; }

        public DataSet StoreAllProvitionalDiagnosis { get; set; }
        public string Mode { get; set; } 
    }
}