using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Report
{
    public class MISPatientWiseCollectionReport 
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public string PatientType { get; set; }
        public string PatientRegNo { get; set; }
        public string BillType { get; set; }
        public string PatientOPDIPDNo { get; set; }
        public string BillNo { get; set; }
        public string TotalAmount { get; set; }
        public string PatientDetails { get; set; }
        public DataSet dsPatientReport { get; set; }
    }
}
