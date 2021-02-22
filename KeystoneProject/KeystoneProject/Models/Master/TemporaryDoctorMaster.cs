using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class TemporaryDoctorMaster
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string PatientRegNo { get; set; }
        public string PatientType { get; set; }
        public string TemporaryDoctor { get; set; }
        public string RefferedTemporaryDoctor { get; set; }
        public string patientname { get; set; }
        public string PatientOPDIPDNO { get; set; }
        public string ConsultantDoctorName { get; set; }
        public string ReferredByDoctorName { get; set; }
        public string ConsultantDoctorID { get; set; }
        public string ReferredByDoctorID { get; set; }
        public string DepartMentID { get; set; }
        public string Consultant { get; set; }
        public string Referred { get; set; }

    }
}