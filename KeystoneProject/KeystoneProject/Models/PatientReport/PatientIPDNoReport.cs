using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PatientReport
{
    public class PatientIPDNoReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string PatientName { get; set; }
        public string PatientRegNo { get; set; }
        public string BillNo { get; set; }
        public string PatientIPDNO { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string PatientType { get; set; }
        public string AddmissionDate { get; set; }
        public string ConsultantDr { get; set; }
        public string ReferredDr { get; set; }
        public string TPAName { get; set; }
        public string ToIPDNo { get; set; }
        public string FromIPDNo { get; set; }
    }
}