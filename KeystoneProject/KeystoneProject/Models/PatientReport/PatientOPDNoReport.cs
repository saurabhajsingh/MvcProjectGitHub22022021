using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PatientReport
{
    public class PatientOPDNoReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string PatientName { get; set; }
        public string PatientRegNo { get; set; }
        public string BillNo { get; set; }
        public string PatientOPDNO { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string PatientType { get; set; }
        public string PatientRegistrationDate { get; set; }
        public string ConsultantDr { get; set; }
        public string ReferredDr { get; set; }
        public string TPAName { get; set; }
        public string ToOPDNo { get; set; }
        public string FromOPDNo { get; set; }
    }
}