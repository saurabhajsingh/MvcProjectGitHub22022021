using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace KeystoneProject.Models.Report
{
    public class MISPatientIPDDischargeSummary 
    {

        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string PatientRegNo { get; set; }
        public string PatientName { get; set; }
        public string EndResult { get; set; }

        public string SearchName { get; set; }
        public string PatientType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string BillsAmount { get; set; }
        public string PaidAmount { get; set; }
        public string BalanceAmount { get; set; }
        public DataSet ds { get; set; }
        public string RefundAmount { get; set; }
        public string ID { get; set; }
        public string IPDNo { get; set; }
        public string Age { get; set;}

        public string Ward { get; set; }
        public string FinalDiagnosis { get; set; }
        public string ConsultantDr { get; set; }

        public string ReffDocName { get; set; }
        public string AddmissionDate { get; set; }
        public string Dischargedate { get; set; }
        public string PatientStatus { get; set; }
        public string PatientDetails { get; set; }

        public string IPDID {get; set;}

        public string RegNo { get; set; }
  
    }
}
