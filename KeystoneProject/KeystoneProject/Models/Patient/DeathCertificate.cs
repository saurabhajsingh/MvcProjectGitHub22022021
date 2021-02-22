using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Patient
{
    public class DeathCertificate
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int PatientDeathCertificateID { get; set; }
        public string ReferenceCode { get; set; }
        public string PatientRegNo { get; set; }
        public string PatientName { get; set; }
        public string GuardianName { get; set; }
        public string DateOfDeath { get; set; }
        public string TimeOfDeath { get; set; }
        public string DeathType { get; set; }
        public string ReasonOfDeath { get; set; }
        public string Discription { get; set; }
        public string Address { get; set; }
        public string Mode { get; set; }
        public string CreationID { get; set; }
        public DataSet StoreAllCity { get; set; }
    }
}