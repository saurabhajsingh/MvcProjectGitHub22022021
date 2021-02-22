using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.MIS_PatientReports
{
    public class ReportMISReferredWiseAnalyse
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DoctorID { get; set; }
        public string ReffDoctor { get; set; }
        public string RegNo { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
    }
}